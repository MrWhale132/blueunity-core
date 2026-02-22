using UnityEngine;

namespace Assets._Project.Scripts
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _singleton;

        public static T Singleton {
            get
            {
                if (_singleton == null)
                {
                    _singleton = Resources.Load<T>(typeof(T).Name);

                    if (_singleton == null)
                        Debug.LogError($"[RuntimeScriptableSingleton] Couldn't find {typeof(T).Name} in Resources.");
                }

                return _singleton;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{GetType().Name}");
            if (guids.Length > 1)
            {
                Debug.LogError($"Multiple {GetType().Name} assets found!");
            }
        }
#endif

    }
}
