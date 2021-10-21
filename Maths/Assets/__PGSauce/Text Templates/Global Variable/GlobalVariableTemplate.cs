using UnityEngine;
using System.Collections.Generic;
using PGSauce.Core;

namespace PGSauce.Core
{
	public class GlobalVariableTemplate : TextTemplateGeneratorBase
	{
	    public GlobalVariableTemplate(IGlobalVariableTemplate templateInterface)
        {
            TagGenerators = new Dictionary<string, TagGenerator>();
            
			TagGenerators.Add("#SUBNAMESPACE#", templateInterface.SUBNAMESPACE);
			TagGenerators.Add("#FORMATTEDTYPE#", templateInterface.FORMATTEDTYPE);
			TagGenerators.Add("#TYPE#", templateInterface.TYPE);

        }
	}
}
