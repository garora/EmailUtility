using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
