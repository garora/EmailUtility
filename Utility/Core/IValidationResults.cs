using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public interface IValidationResults : IErrors, IMessages
    {
        bool IsValid { get; }
    }
}
