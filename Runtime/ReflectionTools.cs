
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.Events;

namespace Theblueway.Core.Runtime
{
    public static class ReflectionTools
    {
        public static IEnumerable<(MethodInfo method, object target)> GetRuntimeDelegatesFromUnityEvent(UnityEventBase unityEvent)
        {
            return GetRuntimeDelegatesFromUnityEvent(unityEvent, Array.Empty<Type>());
        }
        public static IEnumerable<(MethodInfo method, object target)> GetRuntimeDelegatesFromUnityEvent<T0>(UnityEventBase unityEvent)
        {
            return GetRuntimeDelegatesFromUnityEvent(unityEvent, new Type[] { typeof(T0) });
        }
        public static IEnumerable<(MethodInfo method, object target)> GetRuntimeDelegatesFromUnityEvent<T0, T1>(UnityEventBase unityEvent)
        {
            return GetRuntimeDelegatesFromUnityEvent(unityEvent, new Type[] { typeof(T0), typeof(T1) });
        }
        public static IEnumerable<(MethodInfo method, object target)> GetRuntimeDelegatesFromUnityEvent<T0, T1, T2>(UnityEventBase unityEvent)
        {
            return GetRuntimeDelegatesFromUnityEvent(unityEvent, new Type[] { typeof(T0), typeof(T1), typeof(T2) });
        }
        public static IEnumerable<(MethodInfo method, object target)> GetRuntimeDelegatesFromUnityEvent<T0, T1, T2, T3>(UnityEventBase unityEvent)
        {
            return GetRuntimeDelegatesFromUnityEvent(unityEvent, new Type[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3) });
        }

        public static IEnumerable<(MethodInfo method, object target)> GetRuntimeDelegatesFromUnityEvent(UnityEventBase unityEvent, Type[] argTypes)
        {
            var delegates = new List<(MethodInfo method, object target)>();


            ///persistent listeners have fixed arguments and a <see cref="PersistentListenerMode"/>
            ///since during runtime you cant add persisten listeners you cant restore them to how they was set in the inspector originally
            ///thus, for now, I will just say that persistent listeners are not supported.
            ///we lose these persistent calls because we add components with <see cref="Component.GetComponent{T}"/>
            ///and not instantiating them via prefabs, which would have them.


            // 1. Persistent (Inspector) listeners
            //for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
            //{
            //    var target = unityEvent.GetPersistentTarget(i);
            //    var methodName = unityEvent.GetPersistentMethodName(i);

            //    //though target can be null for statics, they can not be used in inspector
            //    if (target != null && !string.IsNullOrEmpty(methodName))
            //    {
            //        var methodInfo = UnityEventBase.GetValidMethodInfo(target, methodName, argTypes);

            //        if (methodInfo != null)
            //        {
            //            //Debug.Log("method found: "+methodInfo.Name);
            //            delegates.Add((methodInfo, target));
            //        }
            //    }
            //}

            // 2.
            var callsField = typeof(UnityEventBase).GetField("m_Calls",
                BindingFlags.Instance | BindingFlags.NonPublic);
            var invokableCallList = callsField.GetValue(unityEvent);

            //GetDelegatesFromCallList("m_PersistentCalls");
            GetDelegatesFromCallList("m_RuntimeCalls");

            return delegates;


            void GetDelegatesFromCallList(string fieldName)
            {
                var runtimeCallsField = invokableCallList.GetType().GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
                var runtimeCalls = runtimeCallsField.GetValue(invokableCallList) as System.Collections.IList;

                foreach (var invokable in runtimeCalls)
                {
                    var delegateField = invokable.GetType().GetField("Delegate",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    if (delegateField != null)
                    {
                        if (delegateField.GetValue(invokable) is Delegate del)
                            delegates.Add((del.Method, del.Target));
                    }
                }
            }
        }
    }
}
