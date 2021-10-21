using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PGSauce.Core.PGEditor
{
    public class FSMActionGenerationAction : OdinWindowBaseCodeGenerationAction<FSMCodeGeneratorOdinWindow>
    {
        public FSMActionGenerationAction(FSMCodeGeneratorOdinWindow window) : base(window)
        {
        }

        protected override void GenerateAction()
        {
            Window.GenerateNewFSMAction();
        }

        protected override (bool isOK, string errorMessage) VerifyCriticalData()
        {
            var ok = Window.IsNotNullOrEmpty(Window.FSMName) && Window.IsNotNullOrEmpty(Window.ActionName);
            return (ok, "FSM Name and Action Name must be not empty");
        }
    }
}
