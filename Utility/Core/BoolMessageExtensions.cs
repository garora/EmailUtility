using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public static class BoolMessageExtensions
    {
        public static int AsExitCode(this BoolMessage result)
        {
            return result.Success ? 0 : 1;
        }
    }
}
