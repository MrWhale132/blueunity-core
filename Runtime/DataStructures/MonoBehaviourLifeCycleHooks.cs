using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace Assets._Project.Scripts.UtilScripts
{
    public enum LifeCycleHookType
    {
        //Awake,
        //Start,
        Update,
        //FixedUpdate,
        //LateUpdate,
        //OnEnable,
        //OnDisable,
        //OnDestroy
    }

    [DefaultExecutionOrder(-10000)]
    public class MonoBehaviourLifeCycleHooks:MonoBehaviour
    {
        public static MonoBehaviourLifeCycleHooks _singleton;
        public static MonoBehaviourLifeCycleHooks Singleton {
            get
            {
                if (_singleton == null)
                {
                    var go = new GameObject("MonoBehaviourLifeCycleHooks_Singleton");
                    _singleton = go.AddComponent<MonoBehaviourLifeCycleHooks>();
                    DontDestroyOnLoad(_singleton.gameObject);

                    //init
                    _singleton._hooks = new Dictionary<LifeCycleHookType, Action>();
                    foreach (LifeCycleHookType hookType in Enum.GetValues(typeof(LifeCycleHookType)))
                    {
                        _singleton._hooks[hookType] =_singleton._NoOp;
                    }
                }

                return _singleton;
            }
        }

        public Dictionary<LifeCycleHookType, Action> _hooks;


        public void Awake()
        {
            //if (Singleton == null)
            //{
            //    Singleton = this;
            //    DontDestroyOnLoad(gameObject);

            //    //init
            //    _hooks = new Dictionary<LifeCycleHookType, Action>();
            //    foreach (LifeCycleHookType hookType in Enum.GetValues(typeof(LifeCycleHookType)))
            //    {
            //        _hooks[hookType] = _NoOp;
            //    }
            //}
            //else
            //{
            //    Debug.LogError("Another instance of MonoBehaviourLifeCycleHooks exists. Destroying this instance.");
            //    Destroy(gameObject);
            //}
        }

        public void _NoOp() { }


        public void Hook(LifeCycleHookType hooktype, Action action)
        {
            _hooks[hooktype] += action;
        }
        public void Unhook(LifeCycleHookType hookType, Action action)
        {
            _hooks[hookType] -= action;
        }
        public void Update()
        {
            
            _hooks[LifeCycleHookType.Update].Invoke();
        }
    }
}
