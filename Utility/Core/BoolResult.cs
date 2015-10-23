using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public class BoolResult<T> : BoolMessageItem<T>
    {
        public static readonly BoolResult<T> False = new BoolResult<T>(default(T), false, string.Empty, (IValidationResults)ValidationResults.Empty);
        public static readonly BoolResult<T> True = new BoolResult<T>(default(T), true, string.Empty, (IValidationResults)ValidationResults.Empty);
        private IValidationResults _errors;

        public IValidationResults Errors
        {
            get
            {
                return this._errors;
            }
        }

        public BoolResult(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message)
        {
            this._errors = errors;
        }
    }
}
