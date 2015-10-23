using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Core
{
    public class CryptoHelper
    {
        public enum CryptoTypes
        {
            encTypeDES,
            encTypeRC2,
            encTypeRijndael,
            encTypeTripleDES
        }

        private const string CRYPT_DEFAULT_PASSWORD = "abcd!@#";
        private const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeRijndael;
        private CryptoTypes mCryptoType = CryptoTypes.encTypeRijndael;

        private byte[] mIV = new byte[8]
        {
            65,
            110,
            68,
            26,
            69,
            178,
            200,
            219
        };

        private byte[] mKey = new byte[24]
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9,
            10,
            11,
            12,
            13,
            14,
            15,
            16,
            17,
            18,
            19,
            20,
            21,
            22,
            23,
            24
        };

        private string mPassword = "abcd!@#";

        private readonly byte[] SaltByteArray = new byte[13]
        {
            73,
            118,
            97,
            110,
            32,
            77,
            101,
            100,
            118,
            101,
            100,
            101,
            118
        };

        public CryptoHelper()
        {
            calculateNewKeyAndIV();
        }

        public CryptoHelper(CryptoTypes CryptoType)
        {
            this.CryptoType = CryptoType;
        }

        public CryptoTypes CryptoType
        {
            get { return mCryptoType; }
            set
            {
                if (mCryptoType == value)
                    return;
                mCryptoType = value;
                calculateNewKeyAndIV();
            }
        }

        public string Password
        {
            get { return mPassword; }
            set
            {
                if (!(mPassword != value))
                    return;
                mPassword = value;
                calculateNewKeyAndIV();
            }
        }

        public string Encrypt(string inputText)
        {
            return Convert.ToBase64String(EncryptDecrypt(new UTF8Encoding().GetBytes(inputText), true));
        }

        public string Encrypt(string inputText, string password)
        {
            Password = password;
            return Encrypt(inputText);
        }

        public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;
            return Encrypt(inputText, password);
        }

        public string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            CryptoType = cryptoType;
            return Encrypt(inputText);
        }

        public string Decrypt(string inputText)
        {
            return new UTF8Encoding().GetString(EncryptDecrypt(Convert.FromBase64String(inputText), false));
        }

        public string Decrypt(string inputText, string password)
        {
            Password = password;
            return Decrypt(inputText);
        }

        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;
            return Decrypt(inputText, password);
        }

        public string Decrypt(string inputText, CryptoTypes cryptoType)
        {
            CryptoType = cryptoType;
            return Decrypt(inputText);
        }

        private byte[] EncryptDecrypt(byte[] inputBytes, bool Encrpyt)
        {
            var cryptoTransform = getCryptoTransform(Encrpyt);
            var memoryStream = new MemoryStream();
            try
            {
                var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                cryptoStream.FlushFinalBlock();
                var numArray = memoryStream.ToArray();
                cryptoStream.Close();
                return numArray;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in symmetric engine. Error : " + ex.Message, ex);
            }
        }

        private ICryptoTransform getCryptoTransform(bool encrypt)
        {
            var symmetricAlgorithm = selectAlgorithm();
            symmetricAlgorithm.Key = mKey;
            symmetricAlgorithm.IV = mIV;
            if (encrypt)
                return symmetricAlgorithm.CreateEncryptor();
            return symmetricAlgorithm.CreateDecryptor();
        }

        private SymmetricAlgorithm selectAlgorithm()
        {
            SymmetricAlgorithm symmetricAlgorithm;
            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    symmetricAlgorithm = DES.Create();
                    break;
                case CryptoTypes.encTypeRC2:
                    symmetricAlgorithm = RC2.Create();
                    break;
                case CryptoTypes.encTypeRijndael:
                    symmetricAlgorithm = Rijndael.Create();
                    break;
                case CryptoTypes.encTypeTripleDES:
                    symmetricAlgorithm = TripleDES.Create();
                    break;
                default:
                    symmetricAlgorithm = TripleDES.Create();
                    break;
            }
            return symmetricAlgorithm;
        }

        private void calculateNewKeyAndIV()
        {
            var passwordDeriveBytes = new PasswordDeriveBytes(mPassword, SaltByteArray);
            var symmetricAlgorithm = selectAlgorithm();
            mKey = passwordDeriveBytes.GetBytes(symmetricAlgorithm.KeySize/8);
            mIV = passwordDeriveBytes.GetBytes(symmetricAlgorithm.BlockSize/8);
        }
    }
}