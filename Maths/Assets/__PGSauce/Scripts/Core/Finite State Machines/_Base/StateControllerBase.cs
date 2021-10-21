using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace PGSauce.Core.FiniteStateMachinesBase
{
    public abstract class StateControllerBase<T> : MonoBehaviour where T : StateControllerBase<T>
    {
        [SerializeField] private bool showDebug;
        
        public abstract StateBase<T> InitialState { get; }
        public bool ShowDebug => showDebug;
        [ShowInInspector, LabelText("Current State")]
        public string CurrentStateName => Fsm != null ? Fsm.CurrentState.Name() : "";
        public StateMachineBase<T> Fsm { get; private set; }

        protected void Awake()
        {
            InitFsm();
            Fsm = new StateMachineBase<T>(this as T, InitialState);
            CustomInit();
        }
        
        protected void Update()
        {
            BeforeFsmUpdate();
            Fsm.HandleInput();
            Fsm.LogicUpdate();
            Fsm.CheckTransitions();
            AfterFsmUpdate();
        }
        
        protected void FixedUpdate()
        {
            Fsm.PhysicsUpdate();
        }

        protected virtual void CustomInit()
        {
        }

        protected virtual void InitFsm()
        {
        }
        
        protected virtual void BeforeFsmUpdate()
        {
        }

        protected virtual void AfterFsmUpdate()
        {
        }

        private void OnDrawGizmos()
        {
            if(!showDebug) {return;}
#if UNITY_EDITOR
            Handles.Label(transform.position + Vector3.up * 3, CurrentStateName);  
#endif
            
        }
    }
}