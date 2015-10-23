using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Core;

namespace Utility
{
    public class BoolErrorsItem : BoolMessageItem
    {
        protected IErrors _errors;

        public IErrors Errors
        {
            get
            {
                return this._errors;
            }
        }

        public BoolErrorsItem(object item, bool success, string message, IErrors errors)
            : base(item, success, message)
        {
            this._errors = errors;
        }
    }

    public class BoolErrorsItem<T> : BoolErrorsItem
    {
        public T Item
        {
            get
            {
                return (T)base.Item;
            }
        }

        public BoolErrorsItem(T item, bool success, string message, IValidationResults errors)
            : base((object)item, success, message, (IErrors)errors)
        {
            this._errors = (IErrors)errors;
        }
    }
}
