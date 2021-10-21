using PGSauce.Core;

namespace PGSauce.Core.PGEvents
{
	public interface IPGEventGenericTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string NUMBERARG();
		string GENERICTYPES();
		string GENERICARGUMENTS();
		string GENERICVALUES();

	}
}
