using PGSauce.Core.FiniteStateMachinesBase;
using PGSauce.Core.PGFiniteStateMachine.ScriptableObjects;

namespace PGSauce.Core.PGFiniteStateMachine
{
    public sealed class Transition<T> : TransitionBase<T> where T : StateControllerBase<T>
    {
        private SODecision<T> _decision;
        private bool _reverseValue;
        
        public Transition(StateBase<T> stateBibi, SODecision<T> transitionDecision, bool reverseValue)
        {
            To = stateBibi;
            _decision = transitionDecision;
            _reverseValue = reverseValue;
        }

        public override bool Decide(T controller)
        {
            var ok = _decision.Decide(controller);
            if (_reverseValue)
            {
                ok = !ok;
            }
            return ok;
        }

        public override StateBase<T> To { get; set; }
    }
}