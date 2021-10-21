using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
    public abstract class IState : ScriptableObject
    {
        [SerializeField] private bool isNullState;
        [SerializeField] [HideIf("IsNullState")]
        private string stateName;

        public int debugIndex;

        public string StateName => IsNullState ? "NULL" : (stateName == "" ? name : stateName);
        public bool IsNullState => isNullState;

        public abstract List<ITransition> GetTransitions();
    }
}
