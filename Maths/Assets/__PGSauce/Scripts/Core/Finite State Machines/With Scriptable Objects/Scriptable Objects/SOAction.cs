using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
    public abstract class SOAction<T> : IAction where T : StateControllerBase<T>
    {
        public abstract void Act(T controller);
    }
}
