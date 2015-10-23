using System;
using System.Globalization;
using System.Text;

namespace Utility.Core
{
    public sealed class ByteEncoding
    {
        private ByteEncoding()
        {
        }

        private static int GetByteCount(string hexString)
        {
            var num = 0;
            for (var index = 0; index < hexString.Length; ++index)
            {
                if (IsHexDigit(hexString[index]))
                    ++num;
            }
            return num/2;
        }

        public static byte[] HexToBytes(string hexString)
        {
            int discarded;
            return HexToBytes(hexString, out discarded);
        }

        public static byte[] HexToBytes(string hexString, out int discarded)
        {
            discarded = 0;
            var stringBuilder = new StringBuilder(hexString.Length);
            for (var index = 0; index < hexString.Length; ++index)
            {
                var c = hexString[index];
                if (IsHexDigit(c))
                    stringBuilder.Append(c);
                else
                    ++discarded;
            }
            if (stringBuilder.Length%2 != 0)
            {
                ++discarded;
                stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            var length = stringBuilder.Length/2;
            var numArray = new byte[length];
            var index1 = 0;
            var index2 = 0;
            while (index1 < length)
            {
                var hex = new string(new char[2]
                {
                    stringBuilder[index2],
                    stringBuilder[index2 + 1]
                });
                numArray[index1] = HexToByte(hex);
                ++index1;
                index2 += 2;
            }
            return numArray;
        }

        public static string BytesToHex(byte[] bytes)
        {
            var stringBuilder = new StringBuilder(bytes.Length);
            for (var index = 0; index < bytes.Length; ++index)
                stringBuilder.Append(bytes[index].ToString("X2"));
            return stringBuilder.ToString();
        }

        public static bool InHexFormat(string hexString)
        {
            foreach (var c in hexString)
            {
                if (!IsHexDigit(c))
                    return false;
            }
            return true;
        }

        public static bool IsHexDigit(char c)
        {
            var num1 = Convert.ToInt32('A');
            var num2 = Convert.ToInt32('0');
            c = char.ToUpper(c);
            var num3 = Convert.ToInt32(c);
            return num3 >= num1 && num3 < num1 + 6 || num3 >= num2 && num3 < num2 + 10;
        }

        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            return byte.Parse(hex, NumberStyles.HexNumber);
        }

        public static string BytesToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] Base64ToBytes(string b64String)
        {
            return Convert.FromBase64String(b64String);
        }

        public static string BytesToString(byte[] bytes)
        {
            return Encoding.Unicode.GetString(bytes);
        }

        public static byte[] StringToBytes(string data)
        {
            return Encoding.Unicode.GetBytes(data);
        }
    }
}