
using System;
using System.Collections;

namespace Theblueway.Core.Common
{
    public static class CoroutineHelper
    {
        public static IEnumerator StartSafe(IEnumerator target, Action<Exception> onException)
        {
            while (true)
            {
                object current;
                try
                {
                    // MoveNext() executes the code until the next 'yield'
                    if (!target.MoveNext()) break;
                    current = target.Current;
                }
                catch (Exception ex)
                {
                    onException?.Invoke(ex);
                    yield break;
                }

                yield return current; // Pass the yielded value back to Unity
            }
        }
    }


    public static class MonoBehaviourExtensions
    {
        public static void StartSafeCoroutine(this UnityEngine.MonoBehaviour monoBehaviour, IEnumerator target, Action<Exception> onException)
        {
            monoBehaviour.StartCoroutine(CoroutineHelper.StartSafe(target, onException));
        }
    }
}
