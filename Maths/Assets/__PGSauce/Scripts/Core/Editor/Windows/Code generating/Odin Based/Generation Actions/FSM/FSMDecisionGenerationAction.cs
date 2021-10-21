using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PGSauce.Core.PGEditor
{
    public class FSMDecisionGenerationAction : OdinWindowBaseCodeGenerationAction<FSMCodeGeneratorOdinWindow>
    {
        public FSMDecisionGenerationAction(FSMCodeGeneratorOdinWindow window) : base(window)
        {
        }

        protected override void GenerateAction()
        {
            Window.GenerateNewFSMDecision();
        }

        protected override (bool isOK, string errorMessage) VerifyCriticalData()
        {
            var ok = Window.IsNotNullOrEmpty(Window.FSMName) && Window.IsNotNullOrEmpty(Window.DecisionName);
            return (ok, "FSM Name and Decision Name must be not empty");
        }
    }
}
