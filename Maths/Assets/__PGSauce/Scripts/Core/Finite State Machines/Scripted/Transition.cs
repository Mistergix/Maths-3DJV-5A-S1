using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PGSauce.Core.FiniteStateMachinesBase;

namespace PGSauce.Core.PGFiniteStateMachineScripted
{
    public sealed class Transition<T> : TransitionBase<T> where T : StateControllerBase<T>
    {
        public delegate bool Decision();

        private readonly Decision _decision;

        public Transition(State<T> to, Decision decision)
        {
            To = to;
            _decision = decision;
        }

        public override bool Decide(T controller)
        {
            return _decision();
        }

        public override StateBase<T> To { get; set; }
    }
}
