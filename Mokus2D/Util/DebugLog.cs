using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mokus2D.Util
{
    public static class DebugLog
    {
        private static void WriteLog(LogType logtype, string message, int stackIndex, Exception e = null)
        {
            string text = "";
            switch (logtype)
            {
                case LogType.debug:
                    text = "";
                    break;
                case LogType.info:
                    text = "INFO";
                    break;
                case LogType.warn:
                    text = "WARNING";
                    break;
                case LogType.error:
                    text = "ERROR";
                    break;
                case LogType.fatal:
                    text = "!!! FATAL ERROR";
                    break;
                case LogType.MISSING:
                    text = "MISSING";
                    break;
            }
            string callStack = GetCallStack(stackIndex);
            if (e != null)
            {
                message = message + "\r\n\r\nException Message:   " + e.Message + "\r\n\r\n";
            }
            if (!string.IsNullOrEmpty(text))
            {
                message = text + ":  " + message;
            }
            if (stackIndex != 0 && !string.IsNullOrWhiteSpace(callStack))
            {
                message = message + "               /  " + callStack;
            }
            Debug.WriteLine(message);
        }

        private static string GetCallStack(int index)
        {
            StackTrace stackTrace = new();
            if (index >= stackTrace.FrameCount)
            {
                return "";
            }
            StackFrame frame = stackTrace.GetFrame(index);
            if (frame == null)
            {
                return "";
            }
            System.Reflection.MethodBase method = frame.GetMethod();
            return method == null ? "" : method.DeclaringType + "." + method.Name;
        }

        public static void calle_log(string msg, string id = "")
        {
            string callStack = GetCallStack(2);
            string callStack2 = GetCallStack(3);
            string text = callStack + callStack2 + "_" + id;
            if (!notImplemented.Contains(text))
            {
                notImplemented.Add(text);
                if (!string.IsNullOrEmpty(callStack))
                {
                    msg = msg + ": \n" + callStack;
                }
                if (!string.IsNullOrEmpty(callStack2))
                {
                    msg = msg + "\n(caled by - " + callStack2 + ")";
                }
                WriteLog(LogType.debug, "\n\n" + msg + "\n", 0, null);
            }
        }

        public static void MISSING(string message)
        {
            WriteLog(LogType.MISSING, message, 2, null);
        }

        public static void TODO(string todo = "")
        {
            calle_log("TODO:  " + todo + "   ", todo);
        }

        public static void NotImplemented(string whatIsNotImplemented = "")
        {
            if (whatIsNotImplemented != "")
            {
                whatIsNotImplemented = ": " + whatIsNotImplemented;
            }
            calle_log("Method is not implemented " + whatIsNotImplemented, whatIsNotImplemented);
        }

        public static void TestingRequired(string id = "")
        {
            calle_log("Implementation required runtime testing. " + id, id);
        }

        public static void debug(string message, Exception e = null)
        {
            WriteLog(LogType.debug, message, 2, e);
        }

        public static void info(string message, Exception e = null)
        {
            WriteLog(LogType.info, message, 2, e);
        }

        public static void warn(string message, Exception e = null)
        {
            WriteLog(LogType.warn, message, 2, e);
        }

        public static void error(string message, Exception e = null)
        {
            WriteLog(LogType.error, message, 2, e);
        }

        public static void fatal(string message, Exception e = null)
        {
            WriteLog(LogType.fatal, message, 2, e);
        }

        public static void debugFmt(string message, params object[] args)
        {
            WriteLog(LogType.debug, string.Format(message, args), 2, null);
        }

        public static void infoFmt(string message, params object[] args)
        {
            WriteLog(LogType.info, string.Format(message, args), 2, null);
        }

        public static void warnFmt(string message, params object[] args)
        {
            WriteLog(LogType.warn, string.Format(message, args), 2, null);
        }

        public static void errorFmt(string message, Exception e = null, params object[] args)
        {
            WriteLog(LogType.error, string.Format(message, args), 2, e);
        }

        public static void fatalFmt(string message, Exception e = null, params object[] args)
        {
            WriteLog(LogType.fatal, string.Format(message, args), 2, e);
        }

        private static readonly List<string> notImplemented = new();

        private enum LogType
        {
            debug,
            info,
            warn,
            error,
            fatal,
            MISSING
        }
    }
}
