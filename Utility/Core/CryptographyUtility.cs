using System;
using System.Security.Cryptography;

namespace Utility.Core
{
    public sealed class CryptographyUtility
    {
        private CryptographyUtility()
        {
        }

        public static string GetEntropy(int length)
        {
            return ByteEncoding.BytesToBase64(GetRandomBytes(length));
        }

        public static byte[] GetRandomBytes(int length)
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var data = new byte[length];
            randomNumberGenerator.GetBytes(data);
            return data;
        }

        public static void ZeroMemory(byte[] pByte)
        {
            Array.Clear(pByte, 0, pByte.Length);
        }
    }
}