namespace Utility.Core
{
    public class BoolResult<T> : BoolMessageItem<T>
    {
        public static readonly BoolResult<T> False = new BoolResult<T>(default(T), false, string.Empty,
            ValidationResults.Empty);

        public static readonly BoolResult<T> True = new BoolResult<T>(default(T), true, string.Empty,
            ValidationResults.Empty);

        public BoolResult(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message)
        {
            Errors = errors;
        }

        public IValidationResults Errors { get; private set; }
    }
}