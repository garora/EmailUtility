using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public interface IValidatorNonStateful
    {
        IValidationResults ValidateTarget(object target);

        bool Validate(object target, IValidationResults results);

        bool Validate(ValidationEvent validationEvent);
    }
}
