namespace Utility.Core
{
    public class BoolMessageItem : BoolMessage
    {
        public static readonly BoolMessageItem True = new BoolMessageItem(null, true, string.Empty);
        public static readonly BoolMessageItem False = new BoolMessageItem(null, false, string.Empty);

        public BoolMessageItem(object item, bool success, string message)
            : base(success, message)
        {
            Item = item;
        }

        public object Item { get; private set; }
    }

    public class BoolMessageItem<T> : BoolMessageItem
    {
        public BoolMessageItem(T item, bool success, string message)
            : base(item, success, message)
        {
        }

        public T Item
        {
            get { return (T) base.Item; }
        }
    }
}