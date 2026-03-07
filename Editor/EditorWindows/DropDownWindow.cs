
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Theblueway.Core.Editor.EditorWindows
{
    public class DropDownWindow : EditorWindow
    {
        public Service _service;


        public static void Open<T>(
            IEnumerable<T> options,
            T currentValue,
            Action<T> onSelected,
            Func<T, string> tostring = default)
        {
            var win = CreateInstance<DropDownWindow>();
            var service = new Service<T>();
            win._service = service;
            service.window = win;

            service.allOptions = new List<T>(options);
            service.cachedAllOptionIndexes = new();
            for (int i = 0; i < service.allOptions.Count; i++) service.cachedAllOptionIndexes.Add(i);
            service.filteredOptions = service.cachedAllOptionIndexes.ToList(); //immutability
            service.currentValue = currentValue;
            service.onSelected = onSelected;
            service.toString = tostring != default ? tostring : element => element.ToString();

            service.cachedStringRepresentation = service.allOptions.Select(option => service.toString(option)).ToList();

            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            win.ShowAsDropDown(new Rect(mousePos, Vector2.zero), new Vector2(1000, 300));
        }


        private void OnGUI()
        {
            _service.OnGUI();
        }

        public abstract class Service { public abstract void OnGUI(); }
        public class Service<T>:Service
        {
            public DropDownWindow window;

            public List<string> cachedStringRepresentation;
            public List<T> allOptions;
            public List<int> filteredOptions;
            public List<int> cachedAllOptionIndexes;
            public string search = "";
            public string prevSearch = "";
            public Vector2 scroll;
            public Action<T> onSelected;
            public T currentValue;
            public Func<T, string> toString;


            public override void OnGUI()
            {
                EditorGUI.BeginChangeCheck();

                search = EditorGUILayout.TextField(search, EditorStyles.toolbarSearchField);

                if (EditorGUI.EndChangeCheck())
                {
                    if (search != prevSearch)
                    {
                        if (string.IsNullOrEmpty(search))
                            filteredOptions = cachedAllOptionIndexes.ToList();
                        else
                        {
                            filteredOptions.Clear();

                            int i = 0;
                            foreach (var option in allOptions)
                            {
                                string stringValue = cachedStringRepresentation[i];

                                if (stringValue.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    filteredOptions.Add(i);
                                }
                                i++;
                            }
                        }
                        //filteredOptions = string.IsNullOrEmpty(search)
                        //    ? allOptions
                        //    : allOptions
                        //        .Where(o => o.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                }

                scroll = EditorGUILayout.BeginScrollView(scroll);

                foreach (int index in filteredOptions)
                {
                    var option = allOptions[index];
                    bool isCurrent = option.Equals(currentValue);
                    GUIStyle style = isCurrent ? EditorStyles.boldLabel : EditorStyles.label;

                    if (GUILayout.Button(toString(option), style))
                    {
                        onSelected?.Invoke(option);
                        window.Close();
                    }
                }

                prevSearch = search;

                EditorGUILayout.EndScrollView();
            }
        }
    }
}
