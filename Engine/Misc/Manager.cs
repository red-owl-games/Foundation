using RedOwl.Engine;

namespace RedOwl.Engine
{
    internal interface IBindable
    {
        void DoBind();
    }

    public interface IInjectable
    {
        void DoInject();
    }

    [Singleton]
    public abstract class Manager<T> : Asset<T>, IBindable, IInjectable where T : Manager<T>
    {
        public void DoBind() => Game.Bind((T) this);
        public void DoInject() => Game.Inject(this);
    }
}