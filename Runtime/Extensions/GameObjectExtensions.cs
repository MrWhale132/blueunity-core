
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.UtilScripts.Extensions
{
    public static class GameObjectExtensions
    {
        public static bool IsProbablyPrefabAsset(this GameObject gameObject)
        {
            return gameObject.scene.buildIndex == -1 && gameObject.scene.name != "DontDestroyOnLoad";
            //return gameObject != null && !(gameObject.scene.IsValid()/* && gameObject.scene.isLoaded*/);//cant use that in awake, scene is still unloaded
        }

        public static string HierarchyPath(this Component component)
        {
            return component.gameObject.HierarchyPath();
        }

        public static string HierarchyPath(this GameObject gameObject)
        {
            var sb = new StringBuilder();

            var t = gameObject.transform;

            while (t != null)
            {
                sb.Insert(0, '.');
                sb.Insert(0, t.gameObject.name);
                t = t.parent;
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T:Component
        {
            var comp = gameObject.GetComponent<T>();

            if(comp == null)
                comp = gameObject.AddComponent<T>();

            return comp;
        }
    }
}
