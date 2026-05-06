using System.IO;
using UnityEngine;

namespace Theblueway.Core.Common
{
    [CreateAssetMenu(fileName = "Paths", menuName = "Scriptable Objects/Theblueway/Infra/Paths")]
    [DefaultExecutionOrder(-1_000_000)]
    public class Paths : ScriptableObject
    {
        public string _worldSavePath;
        public string WorldSavePath { get => Path.Combine(Application.persistentDataPath, _worldSavePath); }
    }
}
