using System;

namespace Utility.Core
{
    public class GlobalHelper
    {
        private GlobalHelper()
        {
        }

        public static void SetErr(Exception ex)
        {
            ex.ToString();
        }
    }
}