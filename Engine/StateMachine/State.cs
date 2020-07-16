using System.Collections;

namespace RedOwl.Core
{
    public interface IState { }

    public interface IStateEnter : IState
    {
        void OnEnter();
    }

    public interface IStateAsyncEnter : IState
    {
        IEnumerator OnEnter();
    }

    public interface IStateExecute : IState
    {
        void OnExecute();
    }

    public interface IStateAsyncExecute : IState
    {
        IEnumerator OnExecute();
    }

    public interface IStateExit : IState
    {
        void OnExit();
    }

    public interface IStateAsyncExit : IState
    {
        IEnumerator OnExit();
    }

    internal interface IStateIdentifiable
    {
        string Id { get; }
    }

    public class State : IState, IStateIdentifiable
    {
        public string Id { get; }

        public State(string name)
        {
            Id = name;
        }
        
        public void OnEnter() { }

        public IEnumerator OnExecute()
        {
            yield return null;
        }

        public void OnExit() { }
    }
}