using PGSauce.Core;

namespace PGSauce.Core.PGEvents
{
	public interface IPGEventTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string FORMATTEDSPACEDTYPES();
		string NUMBERARG();
		string FORMATTEDTYPES();
		string TYPES();

	}
}
