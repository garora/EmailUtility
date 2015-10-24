namespace Utility.Core
{
    public class BoolErrorsItem : BoolMessageItem
    {
        protected IErrors _errors;

        public BoolErrorsItem(object item, bool success, string message, IErrors errors)
            : base(item, success, message)
        {
            _errors = errors;
        }

        public IErrors Errors
        {
            get { return _errors; }
        }
    }

    public class BoolErrorsItem<T> : BoolErrorsItem
    {
        public BoolErrorsItem(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message, errors)
        {
            _errors = errors;
        }

        public T Item
        {
            get { return (T) base.Item; }
        }
    }
}