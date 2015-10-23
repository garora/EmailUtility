using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
