using PGSauce.Core;

namespace PGSauce.Core.PGFiniteStateMachine.ScriptableObjects
{
	public interface IDecisionTemplate : ITextTemplateBase
	{
		string SUBNAMESPACE();
		string NAME();
		string DECISIONNAME();
		string STATECONTROLLERNAME();
		string DEFAULTVALUE();

	}
}
