using System;
using System.Text.RegularExpressions;

namespace Utility.Core
{
    public static class StringExtensions
    {
        public static bool TryParse(this string s, out Guid result)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (
                new Regex(
                    "^[A-Fa-f0-9]{32}$|^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$")
                    .Match(s).Success)
            {
                result = new Guid(s);
                return true;
            }
            result = Guid.Empty;
            return false;
        }
    }
}