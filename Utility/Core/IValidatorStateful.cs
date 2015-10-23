using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public interface IValidatorStateful
    {
        object Target { get; set; }

        string Message { get; }

        bool IsValid { get; }

        IValidationResults Results { get; }

        IValidationResults Validate();

        IValidationResults Validate(IValidationResults results);

        void Clear();
    }
}
