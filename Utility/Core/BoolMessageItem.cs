using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Core
{
    public class BoolMessageItem : BoolMessage
    {
        public static readonly BoolMessageItem True = new BoolMessageItem((object)null, true, string.Empty);
        public static readonly BoolMessageItem False = new BoolMessageItem((object)null, false, string.Empty);
        private object _item;

        public object Item
        {
            get
            {
                return this._item;
            }
        }

        public BoolMessageItem(object item, bool success, string message)
            : base(success, message)
        {
            this._item = item;
        }
    }

    public class BoolMessageItem<T> : BoolMessageItem
    {
        public T Item
        {
            get
            {
                return (T)base.Item;
            }
        }

        public BoolMessageItem(T item, bool success, string message)
            : base((object)item, success, message)
        {
        }
    }
}
