using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
    [System.Serializable]
    public class Transition<T> : ITransition where T : StateControllerBase<T>
    {
        public SODecision<T> decision;
        public SOState<T> state;
        public bool reverseValue;

        public override IDecision GetDecision()
        {
            return decision;
        }

        public override IState GetState()
        {
            return state;
        }

        public override bool ReverseValue()
        {
            return reverseValue;
        }
    }
}
