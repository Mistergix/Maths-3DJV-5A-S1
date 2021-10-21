using UnityEngine;
using System.Collections.Generic;
using PGSauce.Core;

namespace PGSauce.Core.PGEvents
{
	public class PGEventGenericTemplate : TextTemplateGeneratorBase
	{
	    public PGEventGenericTemplate(IPGEventGenericTemplate templateInterface)
        {
            TagGenerators = new Dictionary<string, TagGenerator>();
            
			TagGenerators.Add("#SUBNAMESPACE#", templateInterface.SUBNAMESPACE);
			TagGenerators.Add("#NUMBERARG#", templateInterface.NUMBERARG);
			TagGenerators.Add("#GENERICTYPES#", templateInterface.GENERICTYPES);
			TagGenerators.Add("#GENERICARGUMENTS#", templateInterface.GENERICARGUMENTS);
			TagGenerators.Add("#GENERICVALUES#", templateInterface.GENERICVALUES);

        }
	}
}
