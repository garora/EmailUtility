using System;
using System.Collections.Generic;

namespace Utility.Core
{
    public interface IMessages
    {
        int Count { get; }

        bool HasAny { get; }

        IList<string> MessageList { get; set; }

        IDictionary<string, string> MessageMap { get; set; }

        void Add(string message);

        void Add(string key, string message);

        void Clear();

        void CopyTo(IMessages messages);

        void Each(Action<string, string> callback);

        void EachFull(Action<string> callback);

        string Message();

        string Message(string separator);

        string On(string key);

        IList<string> On();
    }
}