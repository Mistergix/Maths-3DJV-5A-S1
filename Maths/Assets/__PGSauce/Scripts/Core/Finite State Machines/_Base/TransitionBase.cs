namespace PGSauce.Core.FiniteStateMachinesBase
{
    public abstract class TransitionBase<T> where T : StateControllerBase<T>
    {
        public abstract bool Decide(T controller);
        public abstract StateBase<T> To { get; set; }
    }
}