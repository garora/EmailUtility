using System;
using System.Collections.Generic;

namespace Utility.Core
{
    public class Errors : Messages, IErrors, IMessages
    {
        [Obsolete("Use MessageList")]
        public IList<string> ErrorList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }

        [Obsolete("Use MessageMap")]
        public IDictionary<string, string> ErrorMap
        {
            get { return _messageMap; }
            set { _messageMap = value; }
        }
    }
}