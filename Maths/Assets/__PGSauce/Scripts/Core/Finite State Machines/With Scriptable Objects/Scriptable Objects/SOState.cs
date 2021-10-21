using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.FiniteStateMachinesBase;
using PGSauce.Core.PGDebugging;
using Sirenix.OdinInspector;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
    public abstract class SOState<T> : IState where T : StateControllerBase<T>
    {
        [SerializeField] [HideIf("IsNullState")] private SOAction<T> enterAction;
        [SerializeField] [HideIf("IsNullState")] private SOAction<T> inputAction;
        [SerializeField] [HideIf("IsNullState")] private SOAction<T> logicAction;
        [SerializeField] [HideIf("IsNullState")] private SOAction<T> physicsAction;
        [SerializeField] [HideIf("IsNullState")] private SOAction<T> exitAction;

        [SerializeField] [HideIf("IsNullState")] private Transition<T>[] transitions;

        public Transition<T>[] Transitions => transitions;

        public override List<ITransition> GetTransitions()
        {
            return transitions.Select(t => t as ITransition).ToList();
        }

        public void OnEnter(T controller) {
            if(enterAction == null) { return; }
            enterAction.Act(controller);
        }
        public void HandleInput(T controller) {
            if (inputAction == null) { return; }
            inputAction.Act(controller);
        }
        public void LogicUpdate(T controller) {
            if (logicAction == null) { return; }
            logicAction.Act(controller);
        }
        public void PhysicsUpdate(T controller) {
            if (physicsAction == null) { return; }
            physicsAction.Act(controller);
        }
        public void OnExit(T controller) {
            if (exitAction == null) { return; }
            exitAction.Act(controller);
        }
    }
}
