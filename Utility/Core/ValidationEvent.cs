namespace Utility.Core
{
    public class ValidationEvent
    {
        public object Context;
        public IValidationResults Results;
        public object Target;

        public ValidationEvent(object target, IValidationResults results, object context)
        {
            Target = target;
            Results = results;
            Context = context;
        }

        public ValidationEvent(object target, IValidationResults results)
        {
            Target = target;
            Results = results;
        }

        public T TargetT<T>()
        {
            if (Target == null)
                return default(T);
            return (T) Target;
        }
    }
}