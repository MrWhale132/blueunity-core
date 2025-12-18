using UnityEngine;


namespace Assets._Project.Scripts.UtilScripts
{
    public class DebugUtil
    {
        public static void LogFatal(string message)
        {
            Debug.LogError("Fatal error: "+message);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
