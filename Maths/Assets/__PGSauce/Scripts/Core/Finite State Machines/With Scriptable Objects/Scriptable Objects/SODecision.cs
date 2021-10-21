using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
    public abstract class SODecision<T> : IDecision where T : StateControllerBase<T>
    {
        public abstract bool Decide(T controller);
    }
}
