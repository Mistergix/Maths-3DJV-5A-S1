using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.PGDebugging;
using UnityEngine;

namespace PGSauce.Core.Utilities
{
    public abstract class MonoSingleton<T> : MonoSingletonBase where T : MonoSingletonBase
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<T>();
                PGDebug.Message($"Instance of {typeof(T)} set through Find Object Of Type !").LogWarning();

                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;
            Init();
        }

        public virtual void Init()
        {

        }
    }
}
