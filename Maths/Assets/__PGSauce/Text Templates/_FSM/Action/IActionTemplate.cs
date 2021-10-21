using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public interface IActionTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string NAME();
		string ACTIONNAME();
		string STATECONTROLLERNAME();

	}
}
