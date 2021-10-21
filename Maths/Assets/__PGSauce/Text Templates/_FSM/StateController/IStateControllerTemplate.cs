using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public interface IStateControllerTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string STATECONTROLLERNAME();

	}
}
