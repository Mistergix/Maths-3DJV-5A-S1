using PGSauce.Core;

namespace PGSauce.Core
{
	public interface IGlobalVariableTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string FORMATTEDTYPE();
		string TYPE();

	}
}
