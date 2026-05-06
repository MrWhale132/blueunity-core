namespace Theblueway.Core.Editor
{
    using System.Reflection;
    using Theblueway.Core.Attributes;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + 4f;
        }


        public override void OnGUI(Rect position)
        {
            var attr = (InspectorButtonAttribute)attribute;

            string label = attr.Label ?? ObjectNames.NicifyVariableName(attr.MethodName);

            if (!GUI.Button(position, label))
                return;
            
            var targets = Selection.objects;
            
            foreach (var obj in targets)
            {
                if (obj == null) continue;

                //this does not work because Selection.objects are not the instances of components that are edited but the gameobject they are on
                var method = obj.GetType().GetMethod(
                    attr.MethodName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (method == null || method.GetParameters().Length != 0)
                    continue;

                Debug.Log("not null method");
                Undo.RecordObject(obj, label);
                method.Invoke(obj, null);
                EditorUtility.SetDirty(obj);
            }
        }
    }
}
