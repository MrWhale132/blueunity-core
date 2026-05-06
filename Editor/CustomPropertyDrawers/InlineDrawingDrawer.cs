using Theblueway.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Theblueway.Core.Editor.CustomPropertyDrawers
{
    [CustomPropertyDrawer(typeof(InlineDrawingAttribute))]
    public class InlineDrawingDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0f;

            var copy = property.Copy();
            var end = copy.GetEndProperty();

            copy.NextVisible(true); // enter children

            while (!SerializedProperty.EqualContents(copy, end))
            {
                height += EditorGUI.GetPropertyHeight(copy, true);
                height += EditorGUIUtility.standardVerticalSpacing;
                copy.NextVisible(false);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var copy = property.Copy();
            var end = copy.GetEndProperty();

            float y = position.y;

            copy.NextVisible(true); // enter children

            while (!SerializedProperty.EqualContents(copy, end))
            {
                float h = EditorGUI.GetPropertyHeight(copy, true);

                var r = new Rect(position.x, y, position.width, h);
                EditorGUI.PropertyField(r, copy, true);

                y += h + EditorGUIUtility.standardVerticalSpacing;
                copy.NextVisible(false);
            }

            EditorGUI.EndProperty();
        }


        public void Old(Rect position, SerializedProperty property, GUIContent label)
        {
            var copy = property.Copy();
            var end = copy.GetEndProperty();

            float y = position.y;

            copy.NextVisible(true); // enter children

            while (!SerializedProperty.EqualContents(copy, end))
            {
                float h = EditorGUI.GetPropertyHeight(copy, true);

                var r = new Rect(position.x, y, position.width, h);
                EditorGUI.PropertyField(r, copy, true);

                y += h + EditorGUIUtility.standardVerticalSpacing;
                copy.NextVisible(false);
            }
        }
    }
}
