using System;
using System.IO;
using System.Security.Cryptography;

namespace Utility.Core
{
    public class SymmetricCryptographyHelper : CryptographyHelperBase
    {
        private SymmetricAlgorithm algorithm;
        protected CryptographyAlgorithm algorithmId;
        protected string entropy;
        private int keyLength;

        public SymmetricCryptographyHelper(CryptographyAlgorithm algId)
        {
            algorithmId = algId;
        }

        public SymmetricCryptographyHelper(CryptographyAlgorithm algId, string password)
        {
            algorithmId = algId;
            entropy = password;
        }

        public override CryptographyAlgorithm Algorithm
        {
            get { return algorithmId; }
        }

        public override string Entropy
        {
            get { return entropy; }
            set { entropy = value; }
        }

        public override byte[] Encrypt(byte[] plaintext)
        {
            if (algorithm == null)
                GetCryptoAlgorithm();
            using (var memoryStream = new MemoryStream())
            {
                var salt = GetSalt();
                memoryStream.Write(salt, 0, salt.Length);
                algorithm.GenerateIV();
                memoryStream.Write(algorithm.IV, 0, algorithm.IV.Length);
                algorithm.Key = GetKey(salt);
                using (var encryptor = algorithm.CreateEncryptor())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plaintext, 0, plaintext.Length);
                        cryptoStream.FlushFinalBlock();
                        memoryStream.Flush();
                        var numArray = memoryStream.ToArray();
                        cryptoStream.Close();
                        return numArray;
                    }
                }
            }
        }

        public override byte[] Decrypt(byte[] cipherText)
        {
            if (algorithm == null)
                GetCryptoAlgorithm();
            using (var memoryStream1 = new MemoryStream(cipherText))
            {
                var numArray1 = new byte[16];
                var buffer1 = new byte[algorithm.IV.Length];
                memoryStream1.Read(numArray1, 0, numArray1.Length);
                memoryStream1.Read(buffer1, 0, buffer1.Length);
                algorithm.Key = GetKey(numArray1);
                algorithm.IV = buffer1;
                using (
                    var cryptoStream = new CryptoStream(memoryStream1, algorithm.CreateDecryptor(),
                        CryptoStreamMode.Read))
                {
                    var buffer2 = new byte[256];
                    using (var memoryStream2 = new MemoryStream())
                    {
                        try
                        {
                            int count;
                            do
                            {
                                count = cryptoStream.Read(buffer2, 0, 256);
                                memoryStream2.Write(buffer2, 0, count);
                            } while (count > 0);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        finally
                        {
                            cryptoStream.Close();
                        }
                        var numArray2 = memoryStream2.ToArray();
                        memoryStream2.Close();
                        return numArray2;
                    }
                }
            }
        }

        private void GetCryptoAlgorithm()
        {
            switch (algorithmId)
            {
                case CryptographyAlgorithm.Des:
                    algorithm = new DESCryptoServiceProvider();
                    break;
                case CryptographyAlgorithm.Rc2:
                    algorithm = new RC2CryptoServiceProvider();
                    break;
                case CryptographyAlgorithm.Rijndael:
                    algorithm = new RijndaelManaged();
                    break;
                case CryptographyAlgorithm.TripleDes:
                    algorithm = new TripleDESCryptoServiceProvider();
                    break;
                default:
                    throw new CryptographicException("Algorithm Id '" + algorithmId + "' not supported.");
            }
            algorithm.Mode = CipherMode.CBC;
            keyLength = algorithm.LegalKeySizes[0].MaxSize/8;
        }

        private byte[] GetSalt()
        {
            return CryptographyUtility.GetRandomBytes(16);
        }

        private byte[] GetKey(byte[] salt)
        {
            if (entropy == null || entropy.Trim().Length == 0)
                entropy = CryptographyUtility.GetEntropy(keyLength);
            return new PasswordDeriveBytes(entropy, salt).GetBytes(keyLength);
        }

        public override void Dispose()
        {
            if (algorithm != null)
                algorithm.Clear();
            GC.SuppressFinalize(this);
        }
    }
}