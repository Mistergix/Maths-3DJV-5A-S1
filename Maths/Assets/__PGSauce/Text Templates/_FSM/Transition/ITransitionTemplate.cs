using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public interface ITransitionTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string NAME();
		string STATECONTROLLERNAME();

	}
}
