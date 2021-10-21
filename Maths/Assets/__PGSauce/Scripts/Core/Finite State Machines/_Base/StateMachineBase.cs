using PGSauce.Core.PGDebugging;

namespace PGSauce.Core.FiniteStateMachinesBase
{
    public class StateMachineBase<T> where T : StateControllerBase<T>
    {
        private readonly T _controller;
        private int _currentStateIndex;
        private bool ShowDebug => _controller.ShowDebug;
        
        public StateBase<T> CurrentState
        {
            get;
            set;
        }

        public T Controller => _controller;

        public StateMachineBase(T stateControllerBase, StateBase<T> initialState)
        {
            _controller = stateControllerBase;
            CurrentState = initialState;
            Enter();
        }
        
        public void ChangeState(StateBase<T> newState)
        {
            if(! newState.IsNullState)
            {
                Exit();
                CurrentState = newState;
                _currentStateIndex = CurrentState.debugIndex;
                Enter();
            }
        }
        
        public void Enter()
        {
            PGDebug.SetCondition(ShowDebug).Message($"{Controller.name} Entered state {CurrentState}").Log();
            CurrentState.Enter(Controller);
        }

        public void CheckTransitions()
        {
            CurrentState.CheckTransitions(Controller);
        }

        public void HandleInput()
        {
            CurrentState.HandleInput(Controller);
        }

        public void LogicUpdate()
        {
            CurrentState.LogicUpdate(Controller);
        }

        public void PhysicsUpdate()
        {
            CurrentState.PhysicsUpdate(Controller);
        }

        public void Exit()
        {
            PGDebug.SetCondition(ShowDebug).Message($"{Controller.name} Exited state {CurrentState}").Log();
            CurrentState.Exit(Controller);
        }
    }
}