using UnityEngine;
using System.Collections.Generic;
using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public class DecisionTemplate : TextTemplateGeneratorBase
	{
	    public DecisionTemplate(IDecisionTemplate templateInterface)
        {
            TagGenerators = new Dictionary<string, TagGenerator>();
            
			TagGenerators.Add("#SUBNAMESPACE#", templateInterface.SUBNAMESPACE);
			TagGenerators.Add("#NAME#", templateInterface.NAME);
			TagGenerators.Add("#DECISIONNAME#", templateInterface.DECISIONNAME);
			TagGenerators.Add("#STATECONTROLLERNAME#", templateInterface.STATECONTROLLERNAME);
			TagGenerators.Add("#DEFAULTVALUE#", templateInterface.DEFAULTVALUE);

        }
	}
}
