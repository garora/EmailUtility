using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public class ValidationEvent
    {
        public object Target;
        public IValidationResults Results;
        public object Context;

        public ValidationEvent(object target, IValidationResults results, object context)
        {
            this.Target = target;
            this.Results = results;
            this.Context = context;
        }

        public ValidationEvent(object target, IValidationResults results)
        {
            this.Target = target;
            this.Results = results;
        }

        public T TargetT<T>()
        {
            if (this.Target == null)
                return default(T);
            return (T)this.Target;
        }
    }
}
