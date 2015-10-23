using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Utility.Logging
{
    public interface ILogConfiguration : IDisposable
    {
        int IndentSize { get; }

        bool AutoFlush { get; }

        List<TraceListener> TraceListeners { get; }

        event EventHandler LogConfigurationChanged;

        TraceLevel GetTraceLevel(string switchName);
    }
}