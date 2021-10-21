using System.Collections.Generic;
using System.Linq;
using PGSauce.Core.FiniteStateMachinesBase;
using PGSauce.Core.PGFiniteStateMachine.ScriptableObjects;
using UnityEngine;

namespace PGSauce.Core.PGFiniteStateMachine
{
    public abstract class StateMachineController<T> : StateControllerBase<T> where T : StateMachineController<T>
    {
        [SerializeField] private SOState<T> initialState;
        private HashSet<SOState<T>> _allStates;
        private HashSet<SOState<T>> _visitedStates;
        private Dictionary<SOState<T>, State<T>> _dict;
        public override StateBase<T> InitialState => _dict[initialState];

        protected override void InitFsm()
        {
            base.InitFsm();
            _allStates = new HashSet<SOState<T>>();
            _visitedStates = new HashSet<SOState<T>>();
            _dict = new Dictionary<SOState<T>, State<T>>();
            FindAllStates(initialState);
            foreach (var soState in _allStates)
            {
                foreach (var transition in soState.Transitions)
                {
                    var t = transition.state;

                    if (!t.IsNullState)
                    {
                        // Create transition from state to t
                        var stateTransition = new Transition<T>(_dict[t], transition.decision, transition.reverseValue);
                        _dict[soState].Transitions.Add(stateTransition);
                    }
                }
            }
        }

        private void FindAllStates(SOState<T> soState)
        {
            if (soState.IsNullState) { return; }
            if (_visitedStates.Contains(soState)) { return; }

            _allStates.Add(soState);
            _visitedStates.Add(soState);
            if (!_dict.ContainsKey(soState))
            {
                AddNewStateToDict(soState);
            }

            foreach (var transition in soState.Transitions)
            {
                var t = transition.state;
                if (!t.IsNullState)
                {
                    if (!_dict.ContainsKey(t))
                    {
                        AddNewStateToDict(t);
                    }
                    _allStates.Add(t);
                }

                FindAllStates(t);
            }
        }

        private void AddNewStateToDict(SOState<T> soState)
        {
            _dict[soState] = new State<T>(soState) {debugIndex = soState.debugIndex};
        }
    }
}