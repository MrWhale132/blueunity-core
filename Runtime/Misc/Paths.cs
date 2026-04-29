using System.IO;
using UnityEngine;

namespace Assets._Project.Scripts.UtilScripts
{
    [CreateAssetMenu(fileName = "Paths", menuName = "Scriptable Objects/Infra/Paths")]
    [DefaultExecutionOrder(-1_000_000)]
    public class Paths : ScriptableObject
    {
        public string _worldSavePath;
        public string WorldSavePath { get => Path.Combine(Application.persistentDataPath, _worldSavePath); }
    }
}
