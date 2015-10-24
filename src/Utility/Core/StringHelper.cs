using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility.Core
{
    public class StringHelper
    {
        private StringHelper()
        {
        }

        public static string FirstCharToUpper(string inputString)
        {
            var stringBuilder = new StringBuilder();
            if (inputString.Length > 0)
                stringBuilder.Append(inputString.Substring(0, 1).ToUpper())
                    .Append(inputString.Substring(1, inputString.Length - 1));
            return stringBuilder.ToString();
        }

        public static string FirstCharToLower(string inputString)
        {
            var stringBuilder = new StringBuilder();
            if (inputString.Length > 0)
                stringBuilder.Append(inputString.Substring(0, 1).ToLower())
                    .Append(inputString.Substring(1, inputString.Length - 1));
            return stringBuilder.ToString();
        }

        public static string StringReplace(string text, string oldValue, string newValue)
        {
            var length = text.ToLower().IndexOf(oldValue.ToLower());
            var str = "";
            for (; length != -1; length = text.ToLower().IndexOf(oldValue.ToLower()))
            {
                str = str + text.Substring(0, length) + newValue;
                text = text.Substring(length + oldValue.Length);
            }
            if (text.Length > 0)
                str += text;
            return str;
        }

        public static object ConvertFromDatabase(object o)
        {
            if (o == DBNull.Value)
                return null;
            return o;
        }

        public static object ConvertToDatabase(object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }

        public static bool Match(object s1, string s2, bool ignoreCase)
        {
            if (s1 == null)
                return s2 == null;
            if (s2 == null || s1.ToString().Length != s2.Length)
                return false;
            if (s1.ToString().Length == 0)
                return true;
            return string.Compare(s1.ToString(), s2, ignoreCase) == 0;
        }

        public static bool Match(string s1, string s2, bool ignoreCase)
        {
            if (s1 == null)
                return s2 == null;
            if (s2 == null || s1.Length != s2.Length)
                return false;
            if (s1.Length == 0)
                return true;
            return string.Compare(s1, s2, ignoreCase) == 0;
        }

        public static bool Match(string s1, string s2)
        {
            return Match(s1, s2, true);
        }

        public static string MakeValidDatabaseCaseVariableName(string inputString)
        {
            return PascalCaseToDatabase(MakeValidPascalCaseVariableName(inputString));
        }

        public static string MakeValidCamelCaseVariableName(string inputString)
        {
            var str = MakeValidPascalCaseVariableName(inputString);
            if (str.Length > 0)
                str = str.Insert(0, str[0].ToString().ToLower()).Remove(1, 1);
            return str;
        }

        public static string MakeValidPascalCaseVariableName(string inputString)
        {
            var stringBuilder = new StringBuilder();
            var pattern = "[A-Z,a-z,0-9]+";
            foreach (Capture capture in Regex.Matches(inputString, pattern))
            {
                var str1 = capture.Value;
                var str2 = str1.Insert(0, str1[0].ToString().ToUpper()).Remove(1, 1);
                stringBuilder.Append(str2);
            }
            var str = stringBuilder.ToString().TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            if (str.Length < 0)
                throw new Exception("Cannot turn string( " + inputString + " ) into a valid variable name");
            return str;
        }

        public static string DatabaseNameToCamelCase(string databaseName)
        {
            databaseName = databaseName.ToLower();
            return new Regex("_.").Replace(databaseName, ReplaceWithUpper);
        }

        public static string DatabaseNameToPascalCase(string databaseName)
        {
            var str = DatabaseNameToCamelCase(databaseName);
            if (str.Length > 0)
                str = str.Insert(0, str[0].ToString().ToUpper()).Remove(1, 1);
            return str;
        }

        public static string PascalCaseToDatabase(string pascalCase)
        {
            return new Regex("(?<caps>[A-Z])").Replace(pascalCase, "_$+").ToLower().TrimStart('_');
        }

        public static string CamelCaseToDatabase(string camelCase)
        {
            return PascalCaseToDatabase(camelCase);
        }

        private static string ReplaceWithUpper(Match m)
        {
            return m.ToString().TrimStart('_').ToUpper();
        }

        public static string EnsureDirectorySeperatorAtEnd(string directory)
        {
            if (!directory.EndsWith(Path.DirectorySeparatorChar.ToString()))
                directory += (string) (object) Path.DirectorySeparatorChar;
            return directory;
        }

        public static MemoryStream StringToMemoryStream(string str)
        {
            return new MemoryStream(StringToByteArray(str));
        }

        public static string MemoryStreamToString(MemoryStream memStream)
        {
            return ByteArrayToString(memStream.GetBuffer(), (int) memStream.Length);
        }

        public static byte[] StringToByteArray(string str)
        {
            return new UTF8Encoding().GetBytes(str);
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            var str = "";
            for (var index = 0; index < bytes.Length; ++index)
                str += bytes[index].ToString("X2");
            return str;
        }

        public static string ByteArrayToString(byte[] byteArray)
        {
            return new UTF8Encoding().GetString(byteArray, 0, byteArray.Length);
        }

        public static string ByteArrayToString(byte[] byteArray, Encoding encoder)
        {
            return encoder.GetString(byteArray, 0, byteArray.Length);
        }

        public static string ByteArrayToString(byte[] byteArray, int length)
        {
            return new UTF8Encoding().GetString(byteArray, 0, length);
        }
    }
}