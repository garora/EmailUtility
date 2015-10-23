using System;

namespace Utility.Core
{
    public abstract class CryptographyHelperBase : ICryptoHelper, IDisposable
    {
        public abstract string Entropy { get; set; }

        public abstract CryptographyAlgorithm Algorithm { get; }

        public abstract byte[] Encrypt(byte[] plaintext);

        public string Encrypt(string plaintext, StringEncodingType encoding)
        {
            byte[] plaintext1 = ByteEncoding.StringToBytes(plaintext);
            var bytes = Encrypt(plaintext1);
            string str;
            switch (encoding)
            {
                case StringEncodingType.Hex:
                    str = ByteEncoding.BytesToHex(bytes);
                    break;
                case StringEncodingType.Base64:
                    str = ByteEncoding.BytesToBase64(bytes);
                    break;
                default:
                    throw new ArgumentException("Unknown encoding type.");
            }
            Array.Clear(plaintext1, 0, plaintext1.Length);
            Array.Clear(bytes, 0, bytes.Length);
            return str;
        }

        public string Encrypt(string plaintext)
        {
            return Encrypt(plaintext, StringEncodingType.Base64);
        }

        public abstract byte[] Decrypt(byte[] cipherText);

        public string Decrypt(string cipherText, StringEncodingType encoding)
        {
            byte[] cipherText1;
            switch (encoding)
            {
                case StringEncodingType.Hex:
                    cipherText1 = ByteEncoding.HexToBytes(cipherText);
                    break;
                case StringEncodingType.Base64:
                    cipherText1 = ByteEncoding.Base64ToBytes(cipherText);
                    break;
                default:
                    throw new ArgumentException("Unknown encoding type.");
            }
            var bytes = Decrypt(cipherText1);
            string str = ByteEncoding.BytesToString(bytes);
            Array.Clear(cipherText1, 0, cipherText1.Length);
            Array.Clear(bytes, 0, bytes.Length);
            return str;
        }

        public string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, StringEncodingType.Base64);
        }

        public abstract void Dispose();
    }
}