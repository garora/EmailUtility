using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public interface IValidator : IValidatorStateful, IValidatorNonStateful
    {
    }
}
