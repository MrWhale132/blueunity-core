using System;
using System.Collections.Generic;
using Theblueway.Core.Common;
using UnityEngine;

namespace Theblueway.Core.DataStructures
{
    [Serializable]
    public struct SerilaizeableKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public SerilaizeableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    [Serializable]
    public class DebugDictionary<TKey, TValue>
    {
        public bool _triggerUpdate;
        public bool _keepUpdated;

        public List<SerilaizeableKeyValuePair<TKey, TValue>> KeyValuePairs;

        public Dictionary<TKey, TValue> Dictionary { get; set; }

        /// <summary>
        /// for saving and loading only
        /// </summary>
        public DebugDictionary()
        {
            
        }

        public DebugDictionary(Dictionary<TKey, TValue> dictionary)
        {
            Dictionary = dictionary;
            KeyValuePairs = new List<SerilaizeableKeyValuePair<TKey, TValue>>();
            MonoBehaviourLifeCycleHooks.Singleton.Hook(LifeCycleHookType.Update, Update);
        }

        ~DebugDictionary()
        {
            //Debug.Log("DebugDictionary destructor called");
            KeyValuePairs = null;
            Dictionary = null;
            MonoBehaviourLifeCycleHooks.Singleton.Unhook(LifeCycleHookType.Update, Update);
        }


        public void Recreate()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();


            if (Dictionary != null)
            {
                Debug.LogError("DebugDictionary: ReCreate should not be called on an existing dictionary." +
                    "This method is intended to be called when unity drops the Dictionary during serilaziation.");

                Dictionary.Clear();
            }
            else
                Dictionary = new Dictionary<TKey, TValue>();


            foreach (var kvp in KeyValuePairs)
            {
                Dictionary.Add(kvp.Key, kvp.Value);
            }

            timer.Stop();
            Debug.Log($"TRACE: DebugDictionary.Recreate took: {timer.ElapsedMilliseconds / 1000f} seconds");
        }



        public void Update()
        {
            UpdateIfNeeded();
        }

        public void TriggerUpdate()
        {
            _triggerUpdate = true;
            UpdateIfNeeded();
        }



        public void UpdateIfNeeded()
        {
            if (!_triggerUpdate && !_keepUpdated)
                return;
            _triggerUpdate = false;

            KeyValuePairs.Clear();
            foreach (var kvp in Dictionary)
            {
                KeyValuePairs.Add(new SerilaizeableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
            }
        }



        public void Clear()
        {
            KeyValuePairs.Clear();
            Dictionary?.Clear();
        }
    }
}
