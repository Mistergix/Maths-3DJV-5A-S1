using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
    public abstract class ITransition
    {
        public abstract IDecision GetDecision();
        public abstract IState GetState();
        public abstract bool ReverseValue();
    }
}
