using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public sealed class ByteEncoding
    {
        private ByteEncoding()
        {
        }

        private static int GetByteCount(string hexString)
        {
            int num = 0;
            for (int index = 0; index < hexString.Length; ++index)
            {
                if (ByteEncoding.IsHexDigit(hexString[index]))
                    ++num;
            }
            return num / 2;
        }

        public static byte[] HexToBytes(string hexString)
        {
            int discarded;
            return ByteEncoding.HexToBytes(hexString, out discarded);
        }

        public static byte[] HexToBytes(string hexString, out int discarded)
        {
            discarded = 0;
            StringBuilder stringBuilder = new StringBuilder(hexString.Length);
            for (int index = 0; index < hexString.Length; ++index)
            {
                char c = hexString[index];
                if (ByteEncoding.IsHexDigit(c))
                    stringBuilder.Append(c);
                else
                    ++discarded;
            }
            if (stringBuilder.Length % 2 != 0)
            {
                ++discarded;
                stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            int length = stringBuilder.Length / 2;
            byte[] numArray = new byte[length];
            int index1 = 0;
            int index2 = 0;
            while (index1 < length)
            {
                string hex = new string(new char[2]
        {
          stringBuilder[index2],
          stringBuilder[index2 + 1]
        });
                numArray[index1] = ByteEncoding.HexToByte(hex);
                ++index1;
                index2 += 2;
            }
            return numArray;
        }

        public static string BytesToHex(byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder(bytes.Length);
            for (int index = 0; index < bytes.Length; ++index)
                stringBuilder.Append(bytes[index].ToString("X2"));
            return stringBuilder.ToString();
        }

        public static bool InHexFormat(string hexString)
        {
            foreach (char c in hexString)
            {
                if (!ByteEncoding.IsHexDigit(c))
                    return false;
            }
            return true;
        }

        public static bool IsHexDigit(char c)
        {
            int num1 = Convert.ToInt32('A');
            int num2 = Convert.ToInt32('0');
            c = char.ToUpper(c);
            int num3 = Convert.ToInt32(c);
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
