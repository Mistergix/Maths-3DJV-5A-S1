using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;

namespace PGSauce.Core.PGFiniteStateMachineScripted
{
    public abstract class State<T> : StateBase<T> where T : StateControllerBase<T>
    {
        protected State()
        {
            Transitions = new List<TransitionBase<T>>();
        }
        
        public override string Name()
        {
            return ToString();
        }

        public override bool IsNullState => false;
    }
}
