
using Assets._Project.Scripts;
using System;
using Theblueway.Core.Runtime.InspectorAttributes;
using UnityEngine;

namespace Theblueway.Core.Runtime.Debugging.Logging
{
    [CreateAssetMenu(fileName = "LoggingConfigSO", menuName = "Theblueway/Logging/LoggingConfigSO")]
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
