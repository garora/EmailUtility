using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Core
{
    public class Messages : IMessages
    {
        protected IList<string> _messageList;
        protected IDictionary<string, string> _messageMap;

        public int Count
        {
            get
            {
                var num = 0;
                if (_messageList != null)
                    num += _messageList.Count;
                if (_messageMap != null)
                    num += _messageMap.Count;
                return num;
            }
        }

        public bool HasAny
        {
            get { return Count > 0; }
        }

        public IList<string> MessageList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }

        public IDictionary<string, string> MessageMap
        {
            get { return _messageMap; }
            set { _messageMap = value; }
        }

        public void Add(string key, string error)
        {
            if (_messageMap == null)
                _messageMap = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(key))
                Add(error);
            else
                _messageMap[key] = error;
        }

        public void Add(string error)
        {
            if (_messageList == null)
                _messageList = new List<string>();
            _messageList.Add(error);
        }

        public void Each(Action<string, string> callback)
        {
            if (_messageMap == null)
                return;
            foreach (var keyValuePair in _messageMap)
                callback(keyValuePair.Key, keyValuePair.Value);
        }

        public void EachFull(Action<string> callback)
        {
            if (_messageMap != null)
            {
                foreach (var keyValuePair in _messageMap)
                {
                    var str = keyValuePair.Key + " " + keyValuePair.Value;
                    callback(str);
                }
            }
            if (_messageList == null)
                return;
            foreach (var str in _messageList)
                callback(str);
        }

        public string Message()
        {
            return Message(Environment.NewLine);
        }

        public string Message(string separator)
        {
            var stringBuilder = new StringBuilder();
            if (_messageList != null)
            {
                foreach (var str in _messageList)
                    stringBuilder.Append(str + separator);
            }
            if (_messageMap != null)
            {
                foreach (var keyValuePair in _messageMap)
                {
                    var str = keyValuePair.Key + " " + keyValuePair.Value;
                    stringBuilder.Append(str + separator);
                }
            }
            return stringBuilder.ToString();
        }

        public void Clear()
        {
            if (_messageMap != null)
                _messageMap.Clear();
            if (_messageList == null)
                return;
            _messageList.Clear();
        }

        public string On(string key)
        {
            if (_messageMap != null && _messageMap.ContainsKey(key))
                return _messageMap[key];
            return string.Empty;
        }

        public IList<string> On()
        {
            if (_messageList == null)
                return null;
            return _messageList;
        }

        public void CopyTo(IMessages messages)
        {
            if (messages == null)
                return;
            if (_messageList != null)
            {
                foreach (var message in _messageList)
                    messages.Add(message);
            }
            if (_messageMap == null)
                return;
            foreach (var keyValuePair in _messageMap)
                messages.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}