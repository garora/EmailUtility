using System.Text.RegularExpressions;

namespace Utility.Core
{
    internal class RegExHelper
    {
        public enum RegexType
        {
            Email,
            ZipCode,
            Year,
            ComplexPassword,
            SpecialCharacters
        }

        public static bool ValidateAgainstRegex(RegexType expressionType, string text)
        {
            var expressionString = "";
            switch (expressionType)
            {
                case RegexType.Email:
                    expressionString = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                    break;
                case RegexType.ZipCode:
                    expressionString = "\\d{5}(-\\d{4})?";
                    break;
                case RegexType.Year:
                    expressionString = "\\d{4}";
                    break;
                case RegexType.ComplexPassword:
                    expressionString = "\\w{6,}";
                    break;
                case RegexType.SpecialCharacters:
                    expressionString = "[^\\w\\.@-]";
                    break;
            }
            return ValidateAgainstRegex(expressionString, text);
        }

        public static bool ValidateAgainstRegex(string expressionString, string text)
        {
            return new Regex(expressionString).Match(text).Success;
        }
    }
}