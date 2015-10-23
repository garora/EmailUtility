using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public class SymmetricCryptographyHelper : CryptographyHelperBase
    {
        protected CryptographyAlgorithm algorithmId;
        protected string entropy;
        private SymmetricAlgorithm algorithm;
        private int keyLength;

        public override CryptographyAlgorithm Algorithm
        {
            get
            {
                return this.algorithmId;
            }
        }

        public override string Entropy
        {
            get
            {
                return this.entropy;
            }
            set
            {
                this.entropy = value;
            }
        }

        public SymmetricCryptographyHelper(CryptographyAlgorithm algId)
        {
            this.algorithmId = algId;
        }

        public SymmetricCryptographyHelper(CryptographyAlgorithm algId, string password)
        {
            this.algorithmId = algId;
            this.entropy = password;
        }

        public override byte[] Encrypt(byte[] plaintext)
        {
            if (this.algorithm == null)
                this.GetCryptoAlgorithm();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] salt = this.GetSalt();
                memoryStream.Write(salt, 0, salt.Length);
                this.algorithm.GenerateIV();
                memoryStream.Write(this.algorithm.IV, 0, this.algorithm.IV.Length);
                this.algorithm.Key = this.GetKey(salt);
                using (ICryptoTransform encryptor = this.algorithm.CreateEncryptor())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plaintext, 0, plaintext.Length);
                        cryptoStream.FlushFinalBlock();
                        memoryStream.Flush();
                        byte[] numArray = memoryStream.ToArray();
                        cryptoStream.Close();
                        return numArray;
                    }
                }
            }
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            if (this.algorithm == null)
                this.GetCryptoAlgorithm();
            using (MemoryStream memoryStream1 = new MemoryStream(cipherText))
            {
                byte[] numArray1 = new byte[16];
                byte[] buffer1 = new byte[this.algorithm.IV.Length];
                memoryStream1.Read(numArray1, 0, numArray1.Length);
                memoryStream1.Read(buffer1, 0, buffer1.Length);
                this.algorithm.Key = this.GetKey(numArray1);
                this.algorithm.IV = buffer1;
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream1, this.algorithm.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] buffer2 = new byte[256];
                    using (MemoryStream memoryStream2 = new MemoryStream())
                    {
                        try
                        {
                            int count;
                            do
                            {
                                count = cryptoStream.Read(buffer2, 0, 256);
                                memoryStream2.Write(buffer2, 0, count);
                            }
                            while (count > 0);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            cryptoStream.Close();
                        }
                        byte[] numArray2 = memoryStream2.ToArray();
                        memoryStream2.Close();
                        return numArray2;
                    }
                }
            }
        }

        private void GetCryptoAlgorithm()
        {
            switch (this.algorithmId)
            {
                case CryptographyAlgorithm.Des:
                    this.algorithm = (SymmetricAlgorithm)new DESCryptoServiceProvider();
                    break;
                case CryptographyAlgorithm.Rc2:
                    this.algorithm = (SymmetricAlgorithm)new RC2CryptoServiceProvider();
                    break;
                case CryptographyAlgorithm.Rijndael:
                    this.algorithm = (SymmetricAlgorithm)new RijndaelManaged();
                    break;
                case CryptographyAlgorithm.TripleDes:
                    this.algorithm = (SymmetricAlgorithm)new TripleDESCryptoServiceProvider();
                    break;
                default:
                    throw new CryptographicException("Algorithm Id '" + (object)this.algorithmId + "' not supported.");
            }
            this.algorithm.Mode = CipherMode.CBC;
            this.keyLength = this.algorithm.LegalKeySizes[0].MaxSize / 8;
        }

        private byte[] GetSalt()
        {
            return CryptographyUtility.GetRandomBytes(16);
        }

        private byte[] GetKey(byte[] salt)
        {
            if (this.entropy == null || this.entropy.Trim().Length == 0)
                this.entropy = CryptographyUtility.GetEntropy(this.keyLength);
            return new PasswordDeriveBytes(this.entropy, salt).GetBytes(this.keyLength);
        }

        public override void Dispose()
        {
            if (this.algorithm != null)
                this.algorithm.Clear();
            GC.SuppressFinalize((object)this);
        }
    }
}
