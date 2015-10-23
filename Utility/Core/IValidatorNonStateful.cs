﻿namespace Utility.Core
{
    public interface IValidatorNonStateful
    {
        IValidationResults ValidateTarget(object target);

        bool Validate(object target, IValidationResults results);

        bool Validate(ValidationEvent validationEvent);
    }
}