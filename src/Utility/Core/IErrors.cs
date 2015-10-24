using System;
using System.Collections.Generic;

namespace Utility.Core
{
    public interface IErrors : IMessages
    {
        [Obsolete("Use MessageList")]
        IList<string> ErrorList { get; set; }

        [Obsolete("Use MessageList")]
        IDictionary<string, string> ErrorMap { get; set; }
    }
}