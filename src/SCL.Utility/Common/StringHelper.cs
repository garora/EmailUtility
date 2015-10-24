using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Utility.Common
{
    public static class StringExtensions
    {
        public static bool TryParse(this string s, out Guid result)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            var format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            var match = format.Match(s);
            if (match.Success)
            {
                result = new Guid(s);
                return true;
            }
            result = Guid.Empty;
            return false;
        }
    }

    public class StringHelper
    {
        private StringHelper()
        {
        }

        public static string FirstCharToUpper(string inputString)
        {
            var sb = new StringBuilder();
            if (inputString.Length > 0)
            {
                sb.Append(inputString.Substring(0, 1).ToUpper())
                    .Append(inputString.Substring(1, inputString.Length - 1));
            }
            return sb.ToString();
        }

        public static string FirstCharToLower(string inputString)
        {
            var sb = new StringBuilder();
            if (inputString.Length > 0)
            {
                sb.Append(inputString.Substring(0, 1).ToLower())
                    .Append(inputString.Substring(1, inputString.Length - 1));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Case Insensitive String Replace
        /// </summary>
        public static string StringReplace(string text, string oldValue, string newValue)
        {
            var iPos = text.ToLower().IndexOf(oldValue.ToLower());
            var retval = "";
            while (iPos != -1)
            {
                retval += text.Substring(0, iPos) + newValue;
                text = text.Substring(iPos + oldValue.Length);
                iPos = text.ToLower().IndexOf(oldValue.ToLower());
            }
            if (text.Length > 0)
                retval += text;
            return retval;
        }

        /// <summary>
        ///     Converts an object from DBNULL to NULL
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object ConvertFromDatabase(object o)
        {
            if (o == DBNull.Value) return null;
            return o;
        }

        /// <summary>
        ///     Converts an object from NULL to DBNULL
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object ConvertToDatabase(object o)
        {
            if (o == null) return DBNull.Value;
            return o;
        }

        #region File Path Conversions

        public static string EnsureDirectorySeperatorAtEnd(string directory)
        {
            if (!(directory.EndsWith(Path.DirectorySeparatorChar.ToString())))
            {
                directory += Path.DirectorySeparatorChar;
            }
            return directory;
        }

        #endregion

        #region String Match

        public static bool Match(object s1, string s2, bool ignoreCase)
        {
            if (s1 == null)
                if (s2 == null) return true;
                else return false;
            if (s2 == null) return false;
            if (s1.ToString().Length != s2.Length) return false;
            if (s1.ToString().Length == 0) return true;

            return (string.Compare(s1.ToString(), s2, ignoreCase) == 0);
        }

        public static bool Match(string s1, string s2, bool ignoreCase)
        {
            if (s1 == null)
                if (s2 == null) return true;
                else return false;
            if (s2 == null) return false;
            if (s1.Length != s2.Length) return false;
            if (s1.Length == 0) return true;

            return (string.Compare(s1, s2, ignoreCase) == 0);
        }

        public static bool Match(string s1, string s2)
        {
            return Match(s1, s2, true);
        }

        #endregion

        #region Variable Case Conversion

        public static string MakeValidDatabaseCaseVariableName(string inputString)
        {
            var pascalCase = MakeValidPascalCaseVariableName(inputString);
            return PascalCaseToDatabase(pascalCase);
        }

        public static string MakeValidCamelCaseVariableName(string inputString)
        {
            var camelCase = MakeValidPascalCaseVariableName(inputString);
            if (camelCase.Length > 0)
            {
                camelCase = camelCase.Insert(0, camelCase[0].ToString().ToLower());
                camelCase = camelCase.Remove(1, 1);
            }
            return camelCase;
        }

        public static string MakeValidPascalCaseVariableName(string inputString)
        {
            var output = new StringBuilder();
            var regexp = "[A-Z,a-z,0-9]+";
            var matches = Regex.Matches(inputString, regexp);
            foreach (Match match in matches)
            {
                var appendString = match.Value;
                appendString = appendString.Insert(0, appendString[0].ToString().ToUpper());
                appendString = appendString.Remove(1, 1);
                output.Append(appendString);
            }
            var returnVal = output.ToString();
            returnVal = returnVal.TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            if (returnVal.Length < 0)
                throw new Exception("Cannot turn string( " + inputString + " ) into a valid variable name");
            return returnVal;
        }

        public static string DatabaseNameToCamelCase(string databaseName)
        {
            databaseName = databaseName.ToLower();
            var regexp = "_.";
            var digitregex = new Regex(regexp);
            var parameterName = digitregex.Replace(databaseName, ReplaceWithUpper);
            return parameterName;
        }

        public static string DatabaseNameToPascalCase(string databaseName)
        {
            var pascalCase = DatabaseNameToCamelCase(databaseName);
            if (pascalCase.Length > 0)
            {
                pascalCase = pascalCase.Insert(0, pascalCase[0].ToString().ToUpper());
                pascalCase = pascalCase.Remove(1, 1);
            }
            return pascalCase;
        }

        public static string PascalCaseToDatabase(string pascalCase)
        {
            var digitregex = new Regex("(?<caps>[A-Z])");
            var parameterName = digitregex.Replace(pascalCase, "_$+");
            parameterName = parameterName.ToLower().TrimStart('_');
            return parameterName;
        }

        public static string CamelCaseToDatabase(string camelCase)
        {
            return PascalCaseToDatabase(camelCase);
        }

        private static string ReplaceWithUpper(Match m)
        {
            var character = m.ToString().TrimStart('_');
            return character.ToUpper();
        }

        #endregion

        #region byte array conversions

        public static MemoryStream StringToMemoryStream(string str)
        {
            var ms = new MemoryStream(StringToByteArray(str));
            return ms;
        }

        public static string MemoryStreamToString(MemoryStream memStream)
        {
            return ByteArrayToString(memStream.GetBuffer(), (int) memStream.Length);
        }

        public static byte[] StringToByteArray(string str)
        {
            var enc = new UTF8Encoding();
            return enc.GetBytes(str);
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            var hexString = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }


        public static string ByteArrayToString(byte[] byteArray)
        {
            var enc = new UTF8Encoding();
            return enc.GetString(byteArray, 0, byteArray.Length);
        }

        public static string ByteArrayToString(byte[] byteArray, Encoding encoder)
        {
            return encoder.GetString(byteArray, 0, byteArray.Length);
        }

        public static string ByteArrayToString(byte[] byteArray, int length)
        {
            var enc = new UTF8Encoding();
            return enc.GetString(byteArray, 0, length);
        }

        #endregion
    }
}