using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public class BoolMessageEx : BoolMessage
    {
        public static readonly BoolMessageEx True = new BoolMessageEx(true, (Exception)null, string.Empty);
        public static readonly BoolMessageEx False = new BoolMessageEx(false, (Exception)null, string.Empty);
        private Exception _ex;

        public Exception Ex
        {
            get
            {
                return this._ex;
            }
        }

        public BoolMessageEx(bool success, Exception ex, string message)
            : base(success, message)
        {
            this._ex = ex;
        }
    }
}
