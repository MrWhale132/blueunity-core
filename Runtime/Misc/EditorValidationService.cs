using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Theblueway.Core.Runtime.Packages.com.blueutils.core.Runtime.Misc
{
    public interface IEditorValidatable
    {
        void EditorValidate();
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class EditorValidationService
    {
        static readonly HashSet<IEditorValidatable> _dirty =
            new HashSet<IEditorValidatable>();

        static bool _scheduled;

        static EditorValidationService()
        {
#if UNITY_EDITOR
            // Safety: clear stale refs on domain reload
            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                _dirty.Clear();
                _scheduled = false;
            };
#endif
        }

        public static void RequestValidation(IEditorValidatable obj)
        {
#if !UNITY_EDITOR
            return;
#endif
            if (obj == null)
                return;

            _dirty.Add(obj);

            if (_scheduled)
                return;

            _scheduled = true;

#if UNITY_EDITOR
            EditorApplication.delayCall += Run;
#endif
        }

        static void Run()
        {
            _scheduled = false;

            // Snapshot so new requests during validation
            // get processed next frame
            _dirty.RemoveWhere(x =>
            {
                if (x == null) return true;
                if (x is Object uobj) return uobj == null;
                return false;
            });
            var list = new List<IEditorValidatable>(_dirty);
            _dirty.Clear();

            foreach (var v in list)
            {
                try
                {
                    v.EditorValidate();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }

}
