using System.Collections.Generic;
using Unity.Mathematics;

namespace RedOwl.Engine
{
    public interface IInputService
    {
        IInputState this[int index] { get; set; }
        void SetStates(params IInputState[] states);
    }
    
    public class InputService : IInputService, IServiceInit
    {
        private Dictionary<int, IInputState> _states;

        public IInputState this[int index]
        {
            get
            {
                IInputState output;
                if (!_states.TryGetValue(index, out output))
                {
                    output = new NullInputState();
                    _states[index] = output;
                }
                return output;
            }
            set => _states[index] = value;
        }

        public void SetStates(params IInputState[] states)
        {
            _states.Clear();
            for (int i = 0; i < states.Length; i++)
            {
                _states[i + 1] = states[i];
            }
        }
        
        public void Init()
        {
            _states = new Dictionary<int, IInputState>();
            SetStates(
                new NullInputState(),
                new NullInputState(),
                new NullInputState(),
                new NullInputState()
            );
        }
    }
    
    public partial class Game
    {
        public static IInputService InputService => Find<IInputService>();

        public static void BindInputService() => Bind<IInputService>(new InputService());
    }
}
