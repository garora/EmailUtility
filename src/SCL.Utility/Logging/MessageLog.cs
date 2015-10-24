using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Utility.Core;

namespace Utility.Logging
{
    public class MessageLog : IDisposable
    {
        internal const string LOG_ERROR_SOURCE = "PyramidLogger";

        private static readonly TraceSwitch _defaultlogLevel = new TraceSwitch("DefaultLogLevel",
            "Switch for default log level");

        private static readonly ArrayList mLogClasses = new ArrayList();
        private static MessageLog mInstance;
        private bool _disposed;
        private readonly ILogConfiguration mLogConfiguration;

        private MessageLog(ILogConfiguration logConfiguration)
        {
            mLogConfiguration = logConfiguration;
            try
            {
                InitializeConfigFile();
            }
            catch (Exception ex)
            {
                LogLogFailure(ex.ToString());
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~MessageLog()
        {
            Dispose(false);
        }

        public static void AddLogClass(Type logClassType)
        {
            try
            {
                if (ReflectionHelper.ImplementsInterface(logClassType, typeof (ILogClass)))
                {
                    mLogClasses.Add(logClassType);
                    if (mInstance != null)
                    {
                        mInstance.mLogConfiguration_LogConfigurationChanged(null, new EventArgs());
                    }
                    else
                    {
                        InitializeLog(new DefaultLogConfiguration());
                        mInstance.mLogConfiguration_LogConfigurationChanged(null, new EventArgs());
                    }
                }
                else
                    LogLogFailure(string.Format(
                        "Cannot Add Log Class Type: {0} does not implement interface ILogClass.", logClassType.FullName));
            }
            catch (Exception ex)
            {
                if (logClassType != null)
                    LogLogFailure("AddLogClass(Type logClassType:" + logClassType + ")" + Environment.NewLine + ex);
                else
                    LogLogFailure("AddLogClass(Type logClassType: null)" + Environment.NewLine + ex);
            }
        }

        public static void InitializeLog(ILogConfiguration logConfiguration)
        {
            try
            {
                if (mInstance != null)
                    DisposeInstance();
                mInstance = new MessageLog(logConfiguration);
            }
            catch (Exception ex)
            {
                LogLogFailure("public static void InitializeLog(ILogConfiguration " +
                              logConfiguration.GetType().FullName + ")" + Environment.NewLine + ex);
            }
        }

        private void InitializeConfigFile()
        {
            mLogConfiguration.LogConfigurationChanged += mLogConfiguration_LogConfigurationChanged;
            mLogConfiguration_LogConfigurationChanged(null, new EventArgs());
        }

        private static void Log(TraceLevel level, string message)
        {
            string.Format("{0}:{1}:{2}: {3}",
                (object) (level != TraceLevel.Off ? level.ToString().PadRight(7) : "Always".PadRight(7)),
                (object) DateTime.UtcNow.ToString("yyyyMMdd.HHmmss"),
                (object) ("(" + DateTime.Now.ToString("yyyyMMdd.HHmmss:fffff ") + ")"), (object) message);
        }

        private static void Log(TraceLevel level, string message, object arg1)
        {
            var message1 = string.Format(message, arg1);
            Log(level, message1);
        }

        private static void Log(TraceLevel level, string message, object arg1, object arg2)
        {
            var message1 = string.Format(message, arg1, arg2);
            Log(level, message1);
        }

        private static void Log(TraceLevel level, string message, object arg1, object arg2, object arg3)
        {
            var message1 = string.Format(message, arg1, arg2, arg3);
            Log(level, message1);
        }

        private static void Log(TraceLevel level, string message, object[] args)
        {
            var message1 = string.Format(message, args);
            Log(level, message1);
        }

        public static void LogAlways(string message)
        {
            Log(TraceLevel.Off, message);
        }

        public static void LogAlways(string message, object arg1)
        {
            Log(TraceLevel.Off, message, arg1);
        }

        public static void LogAlways(string message, object arg1, object arg2)
        {
            Log(TraceLevel.Off, message, arg1, arg2);
        }

        public static void LogAlways(string message, object arg1, object arg2, object arg3)
        {
            Log(TraceLevel.Off, message, arg1, arg2, arg3);
        }

        public static void LogAlways(string message, object[] args)
        {
            Log(TraceLevel.Off, message, args);
        }

        public static void LogError(string message)
        {
            LogError(_defaultlogLevel, message);
        }

        public static void LogError(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceError)
                return;
            Log(TraceLevel.Error, message);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceError)
                return;
            Log(TraceLevel.Error, message, arg1);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceError)
                return;
            Log(TraceLevel.Error, message, arg1, arg2);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceError)
                return;
            Log(TraceLevel.Error, message, arg1, arg2, arg3);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceError)
                return;
            Log(TraceLevel.Error, message, args);
        }

        public static void LogError(TraceSwitch traceSwitch, Exception ex)
        {
            LogError(traceSwitch, ex.ToString());
        }

        public static void LogError(TraceSwitch traceSwitch, Exception ex, NameValueCollection error)
        {
            var stringBuilder = new StringBuilder();
            foreach (string index in error.Keys)
                stringBuilder.AppendFormat("{0} : {1}\n", index, error[index]);
            stringBuilder.Append(ex);
            LogError(traceSwitch, stringBuilder.ToString());
        }

        public static void LogError(TraceSwitch traceSwitch, Exception ex, string error)
        {
            LogError(traceSwitch, "{0}\n{1}", error, ex.ToString());
        }

        public static void LogWarning(string message)
        {
            LogWarning(_defaultlogLevel, message);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceWarning)
                return;
            Log(TraceLevel.Warning, message);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceWarning)
                return;
            Log(TraceLevel.Warning, message, arg1);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceWarning)
                return;
            Log(TraceLevel.Warning, message, arg1, arg2);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceWarning)
                return;
            Log(TraceLevel.Warning, message, arg1, arg2, arg3);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceWarning)
                return;
            Log(TraceLevel.Warning, message, args);
        }

        public static void LogWarning(TraceSwitch traceSwitch, Exception ex)
        {
            LogError(traceSwitch, ex.ToString());
        }

        public static void LogWarning(TraceSwitch traceSwitch, Exception ex, string error)
        {
            LogError(traceSwitch, "{0}\n{1}", error, ex.ToString());
        }

        public static void LogInfo(string message)
        {
            LogInfo(_defaultlogLevel, message);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceInfo)
                return;
            Log(TraceLevel.Info, message);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceInfo)
                return;
            Log(TraceLevel.Info, message, arg1);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceInfo)
                return;
            Log(TraceLevel.Info, message, arg1, arg2);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceInfo)
                return;
            Log(TraceLevel.Info, message, arg1, arg2, arg3);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceInfo)
                return;
            Log(TraceLevel.Info, message, args);
        }

        public static void LogVerbose(string message)
        {
            LogVerbose(_defaultlogLevel, message);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            Log(TraceLevel.Verbose, message);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            Log(TraceLevel.Verbose, message, arg1);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            Log(TraceLevel.Verbose, message, arg1, arg2);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            Log(TraceLevel.Verbose, message, arg1, arg2, arg3);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            Log(TraceLevel.Verbose, message, args);
        }

        public static void LogLogFailure(string message)
        {
        }

        private void mLogConfiguration_LogConfigurationChanged(object sender, EventArgs e)
        {
            try
            {
                LogAlways("Called: Log.mLogConfiguration_LogConfigurationChanged");
                ResetProperties();
                ResetSwitches();
                ResetListeners();
            }
            catch (Exception ex)
            {
                LogLogFailure("Configuration File Cannot Be Parsed Change Was Invalid\n" + ex);
            }
        }

        private void ResetProperties()
        {
            try
            {
                LogAlways("Called: private void ResetProperties()");
                Trace.AutoFlush = mLogConfiguration.AutoFlush;
                Trace.IndentSize = mLogConfiguration.IndentSize;
            }
            catch (Exception ex)
            {
                LogLogFailure("Error resetting properties.\n" + ex);
            }
        }

        private void ResetSwitches()
        {
            try
            {
                LogAlways("Called: private void ResetSwitches()");
                foreach (Type objectType in mLogClasses)
                {
                    foreach (
                        FieldInfo fieldInfo in
                            ReflectionHelper.GetFieldsByAttribute(objectType, typeof (TraceSwitchAttribute)).Keys)
                    {
                        var traceSwitch = (TraceSwitch) fieldInfo.GetValue(objectType);
                        try
                        {
                            traceSwitch.Level = mLogConfiguration.GetTraceLevel(traceSwitch.DisplayName);
                        }
                        catch (Exception ex1)
                        {
                            LogLogFailure("Error Setting Level:\n" + ex1);
                            try
                            {
                                LogLogFailure("Switch: " + traceSwitch.DisplayName);
                                traceSwitch.Level = TraceLevel.Off;
                            }
                            catch (Exception ex2)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogLogFailure("Error resetting switches.\n" + ex);
            }
        }

        private void ResetListeners()
        {
            try
            {
                LogAlways("Called: private void ResetListeners() Pre Listener Count : {0}", Trace.Listeners.Count);
                LogAlways("Configuraiton Listener Count : {0}", mLogConfiguration.TraceListeners.Count);
                RemoveListeners();
                AddListeners();
                LogAlways("Post Listener Count : {0}", Trace.Listeners.Count);
            }
            catch (Exception ex)
            {
                LogLogFailure("Error resetting listeners.\n" + ex);
            }
        }

        private void RemoveListeners()
        {
            for (var index = Trace.Listeners.Count - 1; index >= 0; --index)
            {
                try
                {
                    var traceListener = Trace.Listeners[index];
                    if (!mLogConfiguration.TraceListeners.Exists(new ListenerCompare(traceListener.Name).Match))
                    {
                        traceListener.Flush();
                        Trace.Listeners.Remove(Trace.Listeners[index]);
                        traceListener.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    LogLogFailure("Error removing listener\n" + ex);
                }
            }
        }

        private void AddListeners()
        {
            foreach (var listener in mLogConfiguration.TraceListeners)
            {
                var name = listener.Name;
                try
                {
                    if (!ListenerExists(name))
                        Trace.Listeners.Add(listener);
                }
                catch (Exception ex)
                {
                    LogAlways("Cannot Add One Of The Listeners: " + name + "Exception:" + ex);
                }
            }
        }

        private bool ListenerExists(string listenerName)
        {
            var flag = false;
            foreach (TraceListener traceListener in Trace.Listeners)
            {
                if (StringHelper.Match(listenerName, traceListener.Name))
                    flag = true;
            }
            return flag;
        }

        public static void DisposeInstance()
        {
            if (mInstance == null)
                return;
            mInstance.Dispose();
        }

        protected void Dispose(bool disposing)
        {
            try
            {
                if (_disposed)
                    return;
                foreach (TraceListener traceListener in Trace.Listeners)
                    traceListener.Dispose();
                Trace.Listeners.Clear();
                if (mLogConfiguration != null)
                    mLogConfiguration.Dispose();
                _disposed = true;
            }
            catch
            {
            }
        }

        public class ListenerCompare
        {
            private readonly string _compareName;

            public ListenerCompare(string compareName)
            {
                _compareName = compareName;
            }

            public bool Match(TraceListener listener)
            {
                return StringHelper.Match(_compareName, listener.Name);
            }
        }
    }

    public class DefaultLogConfiguration : ILogConfiguration
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int IndentSize { get; private set; }
        public bool AutoFlush { get; private set; }
        public List<TraceListener> TraceListeners { get; private set; }
        public event EventHandler LogConfigurationChanged;

        public TraceLevel GetTraceLevel(string switchName)
        {
            throw new NotImplementedException();
        }
    }
}