using PGSauce.Core;

namespace PGSauce.Core.PGEvents
{
	public interface IPGEventListenerGenericTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string NUMBERARG();
		string GENERICTYPES();
		string GENERICARGUMENTS();
		string GENERICVALUES();

	}
}
