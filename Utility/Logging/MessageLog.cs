using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility.Core;

namespace Utility.Logging
{
    public class MessageLog : IDisposable
    {
        private static TraceSwitch _defaultlogLevel = new TraceSwitch("DefaultLogLevel", "Switch for default log level");
        private static ArrayList mLogClasses = new ArrayList();
        private bool _disposed = false;
        internal const string LOG_ERROR_SOURCE = "PyramidLogger";
        private static MessageLog mInstance;
        private ILogConfiguration mLogConfiguration;

        private MessageLog(ILogConfiguration logConfiguration)
        {
            this.mLogConfiguration = logConfiguration;
            try
            {
                this.InitializeConfigFile();
            }
            catch (Exception ex)
            {
                MessageLog.LogLogFailure(ex.ToString());
            }
        }

        ~MessageLog()
        {
            this.Dispose(false);
        }

        public static void AddLogClass(Type logClassType)
        {
            try
            {
                if (ReflectionHelper.ImplementsInterface(logClassType, typeof(ILogClass)))
                {
                    MessageLog.mLogClasses.Add((object)logClassType);
                    if (MessageLog.mInstance != null)
                    {
                        MessageLog.mInstance.mLogConfiguration_LogConfigurationChanged((object)null, new EventArgs());
                    }
                    else
                    {
                        MessageLog.InitializeLog((ILogConfiguration)new DefaultLogConfiguration());
                        MessageLog.mInstance.mLogConfiguration_LogConfigurationChanged((object)null, new EventArgs());
                    }
                }
                else
                    MessageLog.LogLogFailure(string.Format("Cannot Add Log Class Type: {0} does not implement interface ILogClass.", (object)logClassType.FullName));
            }
            catch (Exception ex)
            {
                if (logClassType != (Type)null)
                    MessageLog.LogLogFailure("AddLogClass(Type logClassType:" + logClassType.ToString() + ")" + Environment.NewLine + ex.ToString());
                else
                    MessageLog.LogLogFailure("AddLogClass(Type logClassType: null)" + Environment.NewLine + ex.ToString());
            }
        }

        public static void InitializeLog(ILogConfiguration logConfiguration)
        {
            try
            {
                if (MessageLog.mInstance != null)
                    MessageLog.DisposeInstance();
                MessageLog.mInstance = new MessageLog(logConfiguration);
            }
            catch (Exception ex)
            {
                MessageLog.LogLogFailure("public static void InitializeLog(ILogConfiguration " + logConfiguration.GetType().FullName + ")" + Environment.NewLine + ex.ToString());
            }
        }

        private void InitializeConfigFile()
        {
            this.mLogConfiguration.LogConfigurationChanged += new EventHandler(this.mLogConfiguration_LogConfigurationChanged);
            this.mLogConfiguration_LogConfigurationChanged((object)null, new EventArgs());
        }

        private static void Log(TraceLevel level, string message)
        {
            string.Format("{0}:{1}:{2}: {3}", (object)(level != TraceLevel.Off ? level.ToString().PadRight(7) : "Always".PadRight(7)), (object)DateTime.UtcNow.ToString("yyyyMMdd.HHmmss"), (object)("(" + DateTime.Now.ToString("yyyyMMdd.HHmmss:fffff ") + ")"), (object)message);
        }

        private static void Log(TraceLevel level, string message, object arg1)
        {
            string message1 = string.Format(message, arg1);
            MessageLog.Log(level, message1);
        }

        private static void Log(TraceLevel level, string message, object arg1, object arg2)
        {
            string message1 = string.Format(message, arg1, arg2);
            MessageLog.Log(level, message1);
        }

        private static void Log(TraceLevel level, string message, object arg1, object arg2, object arg3)
        {
            string message1 = string.Format(message, arg1, arg2, arg3);
            MessageLog.Log(level, message1);
        }

        private static void Log(TraceLevel level, string message, object[] args)
        {
            string message1 = string.Format(message, args);
            MessageLog.Log(level, message1);
        }

        public static void LogAlways(string message)
        {
            MessageLog.Log(TraceLevel.Off, message);
        }

        public static void LogAlways(string message, object arg1)
        {
            MessageLog.Log(TraceLevel.Off, message, arg1);
        }

        public static void LogAlways(string message, object arg1, object arg2)
        {
            MessageLog.Log(TraceLevel.Off, message, arg1, arg2);
        }

        public static void LogAlways(string message, object arg1, object arg2, object arg3)
        {
            MessageLog.Log(TraceLevel.Off, message, arg1, arg2, arg3);
        }

        public static void LogAlways(string message, object[] args)
        {
            MessageLog.Log(TraceLevel.Off, message, args);
        }

        public static void LogError(string message)
        {
            MessageLog.LogError(MessageLog._defaultlogLevel, message);
        }

        public static void LogError(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceError)
                return;
            MessageLog.Log(TraceLevel.Error, message);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceError)
                return;
            MessageLog.Log(TraceLevel.Error, message, arg1);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceError)
                return;
            MessageLog.Log(TraceLevel.Error, message, arg1, arg2);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceError)
                return;
            MessageLog.Log(TraceLevel.Error, message, arg1, arg2, arg3);
        }

        public static void LogError(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceError)
                return;
            MessageLog.Log(TraceLevel.Error, message, args);
        }

        public static void LogError(TraceSwitch traceSwitch, Exception ex)
        {
            MessageLog.LogError(traceSwitch, ex.ToString());
        }

        public static void LogError(TraceSwitch traceSwitch, Exception ex, NameValueCollection error)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string index in error.Keys)
                stringBuilder.AppendFormat("{0} : {1}\n", (object)index, (object)error[index]);
            stringBuilder.Append(ex.ToString());
            MessageLog.LogError(traceSwitch, stringBuilder.ToString());
        }

        public static void LogError(TraceSwitch traceSwitch, Exception ex, string error)
        {
            MessageLog.LogError(traceSwitch, "{0}\n{1}", (object)error, (object)ex.ToString());
        }

        public static void LogWarning(string message)
        {
            MessageLog.LogWarning(MessageLog._defaultlogLevel, message);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceWarning)
                return;
            MessageLog.Log(TraceLevel.Warning, message);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceWarning)
                return;
            MessageLog.Log(TraceLevel.Warning, message, arg1);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceWarning)
                return;
            MessageLog.Log(TraceLevel.Warning, message, arg1, arg2);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceWarning)
                return;
            MessageLog.Log(TraceLevel.Warning, message, arg1, arg2, arg3);
        }

        public static void LogWarning(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceWarning)
                return;
            MessageLog.Log(TraceLevel.Warning, message, args);
        }

        public static void LogWarning(TraceSwitch traceSwitch, Exception ex)
        {
            MessageLog.LogError(traceSwitch, ex.ToString());
        }

        public static void LogWarning(TraceSwitch traceSwitch, Exception ex, string error)
        {
            MessageLog.LogError(traceSwitch, "{0}\n{1}", (object)error, (object)ex.ToString());
        }

        public static void LogInfo(string message)
        {
            MessageLog.LogInfo(MessageLog._defaultlogLevel, message);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceInfo)
                return;
            MessageLog.Log(TraceLevel.Info, message);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceInfo)
                return;
            MessageLog.Log(TraceLevel.Info, message, arg1);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceInfo)
                return;
            MessageLog.Log(TraceLevel.Info, message, arg1, arg2);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceInfo)
                return;
            MessageLog.Log(TraceLevel.Info, message, arg1, arg2, arg3);
        }

        public static void LogInfo(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceInfo)
                return;
            MessageLog.Log(TraceLevel.Info, message, args);
        }

        public static void LogVerbose(string message)
        {
            MessageLog.LogVerbose(MessageLog._defaultlogLevel, message);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            MessageLog.Log(TraceLevel.Verbose, message);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object arg1)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            MessageLog.Log(TraceLevel.Verbose, message, arg1);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object arg1, object arg2)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            MessageLog.Log(TraceLevel.Verbose, message, arg1, arg2);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object arg1, object arg2, object arg3)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            MessageLog.Log(TraceLevel.Verbose, message, arg1, arg2, arg3);
        }

        public static void LogVerbose(TraceSwitch traceSwitch, string message, object[] args)
        {
            if (!traceSwitch.TraceVerbose)
                return;
            MessageLog.Log(TraceLevel.Verbose, message, args);
        }

        public static void LogLogFailure(string message)
        {
        }

        private void mLogConfiguration_LogConfigurationChanged(object sender, EventArgs e)
        {
            try
            {
                MessageLog.LogAlways("Called: Log.mLogConfiguration_LogConfigurationChanged");
                this.ResetProperties();
                this.ResetSwitches();
                this.ResetListeners();
            }
            catch (Exception ex)
            {
                MessageLog.LogLogFailure("Configuration File Cannot Be Parsed Change Was Invalid\n" + ex.ToString());
            }
        }

        private void ResetProperties()
        {
            try
            {
                MessageLog.LogAlways("Called: private void ResetProperties()");
                Trace.AutoFlush = this.mLogConfiguration.AutoFlush;
                Trace.IndentSize = this.mLogConfiguration.IndentSize;
            }
            catch (Exception ex)
            {
                MessageLog.LogLogFailure("Error resetting properties.\n" + ex.ToString());
            }
        }

        private void ResetSwitches()
        {
            try
            {
                MessageLog.LogAlways("Called: private void ResetSwitches()");
                foreach (Type objectType in MessageLog.mLogClasses)
                {
                    foreach (FieldInfo fieldInfo in (IEnumerable)ReflectionHelper.GetFieldsByAttribute(objectType, typeof(TraceSwitchAttribute)).Keys)
                    {
                        TraceSwitch traceSwitch = (TraceSwitch)fieldInfo.GetValue((object)objectType);
                        try
                        {
                            traceSwitch.Level = this.mLogConfiguration.GetTraceLevel(traceSwitch.DisplayName);
                        }
                        catch (Exception ex1)
                        {
                            MessageLog.LogLogFailure("Error Setting Level:\n" + ex1.ToString());
                            try
                            {
                                MessageLog.LogLogFailure("Switch: " + traceSwitch.DisplayName);
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
                MessageLog.LogLogFailure("Error resetting switches.\n" + ex.ToString());
            }
        }

        private void ResetListeners()
        {
            try
            {
                MessageLog.LogAlways("Called: private void ResetListeners() Pre Listener Count : {0}", (object)Trace.Listeners.Count);
                MessageLog.LogAlways("Configuraiton Listener Count : {0}", (object)this.mLogConfiguration.TraceListeners.Count);
                this.RemoveListeners();
                this.AddListeners();
                MessageLog.LogAlways("Post Listener Count : {0}", (object)Trace.Listeners.Count);
            }
            catch (Exception ex)
            {
                MessageLog.LogLogFailure("Error resetting listeners.\n" + ex.ToString());
            }
        }

        private void RemoveListeners()
        {
            for (int index = Trace.Listeners.Count - 1; index >= 0; --index)
            {
                try
                {
                    TraceListener traceListener = Trace.Listeners[index];
                    if (!this.mLogConfiguration.TraceListeners.Exists(new Predicate<TraceListener>(new MessageLog.ListenerCompare(traceListener.Name).Match)))
                    {
                        traceListener.Flush();
                        Trace.Listeners.Remove(Trace.Listeners[index]);
                        traceListener.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageLog.LogLogFailure("Error removing listener\n" + ex.ToString());
                }
            }
        }

        private void AddListeners()
        {
            foreach (TraceListener listener in this.mLogConfiguration.TraceListeners)
            {
                string name = listener.Name;
                try
                {
                    if (!this.ListenerExists(name))
                        Trace.Listeners.Add(listener);
                }
                catch (Exception ex)
                {
                    MessageLog.LogAlways("Cannot Add One Of The Listeners: " + name + "Exception:" + ex.ToString());
                }
            }
        }

        private bool ListenerExists(string listenerName)
        {
            bool flag = false;
            foreach (TraceListener traceListener in Trace.Listeners)
            {
                if (StringHelper.Match(listenerName, traceListener.Name))
                    flag = true;
            }
            return flag;
        }

        public static void DisposeInstance()
        {
            if (MessageLog.mInstance == null)
                return;
            MessageLog.mInstance.Dispose();
        }

        protected void Dispose(bool disposing)
        {
            try
            {
                if (this._disposed)
                    return;
                foreach (TraceListener traceListener in Trace.Listeners)
                    traceListener.Dispose();
                Trace.Listeners.Clear();
                if (this.mLogConfiguration != null)
                    this.mLogConfiguration.Dispose();
                this._disposed = true;
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public class ListenerCompare
        {
            private string _compareName;

            public ListenerCompare(string compareName)
            {
                this._compareName = compareName;
            }

            public bool Match(TraceListener listener)
            {
                return StringHelper.Match(this._compareName, listener.Name);
            }
        }
    }

    public class DefaultLogConfiguration     : ILogConfiguration
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
