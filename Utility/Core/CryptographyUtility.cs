using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public sealed class CryptographyUtility
    {
        private CryptographyUtility()
        {
        }

        public static string GetEntropy(int length)
        {
            return ByteEncoding.BytesToBase64(CryptographyUtility.GetRandomBytes(length));
        }

        public static byte[] GetRandomBytes(int length)
        {
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] data = new byte[length];
            randomNumberGenerator.GetBytes(data);
            return data;
        }

        public static void ZeroMemory(byte[] pByte)
        {
            Array.Clear((Array)pByte, 0, pByte.Length);
        }
    }
}
