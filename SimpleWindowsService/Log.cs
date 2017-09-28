

namespace SimpleWindowsService
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    // refer to http://www.eidias.com/Blog/2012/10/15/simple-logging-with-use-of-systemdiagnostictrace for logger.
    public static class Log
    {
        private static string logTimeFormat = "HH:mm:ss.fff";

        private static TraceSwitch globalTraceSwitch;

        static Log()
        {
            globalTraceSwitch = new TraceSwitch("TraceLevelSwitch", "Global trace level switch");
            Trace.WriteLine(string.Format("#Logging started at {0}", DateTime.Now.ToString()));
        }

        private static string getCallingType()
        {
            return (new StackFrame(2, false).GetMethod()).DeclaringType.ToString();
        }

        
        public static void Error(string message, Exception ex = null,
                                [CallerMemberName] string sourceMember = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (globalTraceSwitch.TraceError)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0} ERROR: ", DateTime.Now.ToString(logTimeFormat));
                sb.AppendLine(message);
                //appendMessageDetails(sb, getCallingType(), sourceMember, sourceFilePath, sourceLineNumber, ex);
                Trace.Write(sb.ToString());
            }
        }

        public static void Info(string message)
        {
            Trace.WriteLineIf(globalTraceSwitch.TraceInfo,
                string.Format("{0} INFO: {1}", DateTime.Now.ToString(logTimeFormat), message));

        }

        public static void Warning(string message)
        {
            Trace.WriteLineIf(globalTraceSwitch.TraceWarning,
                string.Format("{0} WARNING: {1}", DateTime.Now.ToString(logTimeFormat), message));

        }

        public static void Debug(string message)
        {
            Trace.WriteLineIf(globalTraceSwitch.TraceVerbose,
                string.Format("{0} DEBUG: {1}", DateTime.Now.ToString(logTimeFormat), message));
        }
    }
}
