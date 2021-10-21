using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public interface IStateTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string NAME();
		string STATECONTROLLERNAME();

	}
}
