
using UnityEngine;
using UnityDebug = UnityEngine.Debug;

namespace Theblueway.Core.Runtime.Debugging.Logging
{
    public class BlueDebug
    {
        public static BlueDebug _singleton;
        public static BlueDebug singleton {
            get {
                EnsureInstance();
                return _singleton;
            }
        }

        public static void EnsureInstance()
        {
            if (_singleton == null)
            {
                _singleton = new BlueDebug();
                if (LoggingConfigSO.Singleton != null)
                    _singleton._config = LoggingConfigSO.Singleton.Config;
                else
                    _singleton._config = CreateDefaultConfig();
            }
        }

        public static BlueLoggingConfig CreateDefaultConfig()
        {
            return new BlueLoggingConfig
            {
                LogLevel = LogLevel.Info,
            };
        }


        public BlueLoggingConfig _config;

        public LogLevel logLevel => _config.LogLevel;





        [HideInCallstack]
        public static void LogError(string message, Object context = null) { Error(message, context); }
        [HideInCallstack]
        public static void Error(string message, Object context = null)
        {
            singleton.Log(LogLevel.Error, message, context);
        }

        [HideInCallstack]
        public static void LogWarning(string message, Object context = null) { Warn(message, context); }
        [HideInCallstack]
        public static void Warn(string message, Object context = null)
        {
            singleton.Log(LogLevel.Warning, message, context);
        }

        [HideInCallstack]
        public static void Log(string message, Object context = null) => Info(message, context);
        [HideInCallstack]
        public static void Info(string message, Object context = null)
        {
            singleton.Log(LogLevel.Info, message, context);
        }

        [HideInCallstack]
        public static void Debug(object obj, Object context = null) => Debug(obj?.ToString(), context);
        [HideInCallstack]
        public static void Debug(string message, Object context = null)
        {
            singleton.Log(LogLevel.Debug, message, context);
        }

        [HideInCallstack]
        public static void Trace(string message, Object context = null)
        {
            singleton.Log(LogLevel.Trace, message, context);
        }




        [HideInCallstack]
        public void Log(LogLevel level, string message, Object context = null)
        {
            if ((int)level < (int)logLevel)
            {
                return;
            }

            LogType logType = MapBlueLogLevelToUnityLogType(level);

            const string messageFormat = "{0} | {1}";

            string formattedMessage = string.Format(messageFormat, level.ToString(), message);

            UnityDebug.unityLogger.Log(logType, message: formattedMessage, context: context);
        }

        [HideInCallstack]
        public static LogType MapBlueLogLevelToUnityLogType(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace or LogLevel.Debug or LogLevel.Info => LogType.Log,
                LogLevel.Warning => LogType.Warning,
                LogLevel.Error or LogLevel.Critical or LogLevel.Fatal => LogType.Error,
                _ => LogType.Log,
            };
        }
    }







    public enum LogLevel
    {
        All = 0,
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warning = 4,
        Error = 5,
        Critical = 6,
        Fatal = 7,
        None = 8,
    }
}
