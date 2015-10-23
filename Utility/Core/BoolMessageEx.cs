using System;

namespace Utility.Core
{
    public class BoolMessageEx : BoolMessage
    {
        public static readonly BoolMessageEx True = new BoolMessageEx(true, null, string.Empty);
        public static readonly BoolMessageEx False = new BoolMessageEx(false, null, string.Empty);

        public BoolMessageEx(bool success, Exception ex, string message)
            : base(success, message)
        {
            Ex = ex;
        }

        public Exception Ex { get; private set; }
    }
}