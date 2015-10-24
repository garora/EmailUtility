using Utility.Logging;

namespace Utility.Core
{
    public class LogClass : ILogClass
    {
        [TraceSwitch] public static readonly TraceSwitch Common = new TraceSwitch("Common",
            "Trace Switch for Core namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonDataAccess = new TraceSwitch("CommonDataAccess",
            "Trace Switch for Core.DataAccess namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonEventArg = new TraceSwitch("CommonEventArg",
            "Trace Switch for Core.EventArgs namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonException = new TraceSwitch("CommonException",
            "Trace Switch for Core.Exception namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonForms = new TraceSwitch("CommonForms",
            "Trace Switch for Core.Froms namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonGeneratorFramework =
            new TraceSwitch("CommonGeneratorFramework", "Trace Switch for Core.Framework namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonLogging = new TraceSwitch("CommonLogging",
            "Trace Switch for Core.Logging namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonUtil = new TraceSwitch("CommonUtil",
            "Trace Switch for Core.Util namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonWinUI = new TraceSwitch("CommonWinUI",
            "Trace Switch for Core.WinUI namespace");

        [TraceSwitch] public static readonly TraceSwitch CommonCpMapi = new TraceSwitch("CommonCpMapi",
            "Trace Switch for Core.CpMapi namespace");

        static LogClass()
        {
            MessageLog.AddLogClass(typeof (LogClass));
        }
    }
}