using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;
using Sirenix.OdinInspector;

namespace PGSauce.Core.PGFiniteStateMachineScripted
{
    public abstract class StateMachineController<T> : StateControllerBase<T> where T : StateMachineController<T>
    {
        protected sealed override void InitFsm()
        {
            base.InitFsm();
            InitializeStates();
            CreateTransitions();
        }
        
        public void AddNewTransition(State<T> from, State<T> to, Transition<T>.Decision decision)
        {
            var stateTransition = new Transition<T>(to, decision);
            from.Transitions.Add(stateTransition);
        }

        protected abstract void InitializeStates();

        protected abstract void CreateTransitions();
    }
}
