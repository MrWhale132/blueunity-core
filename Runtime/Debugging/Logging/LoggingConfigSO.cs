using System;
using Theblueway.Core.Attributes;
using Theblueway.Core.Common;
using UnityEngine;

namespace Theblueway.Core.Logging
{
    [CreateAssetMenu(fileName = "LoggingConfigSO", menuName = "Scriptable Objects/Theblueway/Logging/LoggingConfigSO")]
    public class LoggingConfigSO:ScriptableSingleton<LoggingConfigSO>
    {
        public static new LoggingConfigSO Singleton {
            get
            {
                if (_singleton == null)
                {
                    _singleton = Resources.Load<LoggingConfigSO>(typeof(LoggingConfigSO).Name);
                }

                return _singleton;
            }
        }

        [InlineDrawing]
        public BlueLoggingConfig Config;
    }


    [Serializable]
    public class BlueLoggingConfig
    {
        public LogLevel LogLevel = LogLevel.Info;
        //public string Format;
    }

}
