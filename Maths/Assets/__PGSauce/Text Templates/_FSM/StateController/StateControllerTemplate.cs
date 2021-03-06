using UnityEngine;
using System.Collections.Generic;
using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public class StateControllerTemplate : TextTemplateGeneratorBase
	{
	    public StateControllerTemplate(IStateControllerTemplate templateInterface)
        {
            TagGenerators = new Dictionary<string, TagGenerator>();
            
			TagGenerators.Add("#SUBNAMESPACE#", templateInterface.SUBNAMESPACE);
			TagGenerators.Add("#STATECONTROLLERNAME#", templateInterface.STATECONTROLLERNAME);

        }
	}
}
