using System.Collections.Generic;

namespace PGSauce.Core.FiniteStateMachinesBase
{
    public abstract class StateBase<T> where T : StateControllerBase<T>
    {
        public abstract string Name();
        public abstract bool IsNullState { get; }
        public List<TransitionBase<T>> Transitions { get; protected set; }

        public override string ToString()
        {
            return Name();
        }

        public int debugIndex;
        
        public void CheckTransitions(T controller)
        {
            foreach (var t in Transitions)
            {
                var decisionSucceeded = t.Decide(controller);

                if (!decisionSucceeded) continue;
                controller.Fsm.ChangeState(t.To);
                return;
            }
        }

        public virtual void Enter(T controller)
        {
        }

        public virtual void Exit(T controller)
        {
        }

        public virtual void PhysicsUpdate(T controller)
        {
        }

        public virtual void HandleInput(T controller)
        {
        }

        public virtual void LogicUpdate(T controller)
        {
        }
    }
}