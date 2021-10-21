using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;
using PGSauce.Core.PGFiniteStateMachine.ScriptableObjects;

namespace PGSauce.Core.PGFiniteStateMachine
{
    public class State<T> : StateBase<T> where T : StateControllerBase<T>
    {
        private SOState<T> _soState;

        public State(SOState<T> soState)
        {
            _soState = soState;
            Transitions = new List<TransitionBase<T>>();
        }

        public sealed override void Enter(T controller)
        {
            base.Enter(controller);
            _soState.OnEnter(controller);
        }

        public sealed override void Exit(T controller)
        {
            base.Exit(controller);
            _soState.OnExit(controller);
        }

        public sealed override void HandleInput(T controller)
        {
            base.HandleInput(controller);
            _soState.HandleInput(controller);
        }

        public sealed override void LogicUpdate(T controller)
        {
            base.LogicUpdate(controller);
            _soState.LogicUpdate(controller);
        }

        public sealed override void PhysicsUpdate(T controller)
        {
            base.PhysicsUpdate(controller);
            _soState.PhysicsUpdate(controller);
        }

        public override string Name()
        {
            return _soState.StateName;
        }

        public override bool IsNullState => _soState.IsNullState;
    }
}