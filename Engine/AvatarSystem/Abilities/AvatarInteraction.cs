using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface IAvatarInputInteraction : IAvatarInput
    {
        ButtonStates Select { get; }
        ButtonStates Interact { get; set; }
    }
    
    [HideMonoScript]
    public class AvatarInteraction : AvatarAbility<IAvatarInputInteraction>
    {
        public override int Priority { get; } = -200;

        public BetterStackTypes stackType = BetterStackTypes.Fifo;
        
        private ButtonStates _select;

        private BetterStack<IInteractable> _stack;

        #region Unity

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _stack.Push(interactable);
                //_stack.Peek()?.Select();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Deselect();
                _stack.Remove(interactable);
            }
        }

        #endregion

        public override void OnReset()
        {
            _stack = new BetterStack<IInteractable>(stackType);
        }

        protected override void ProcessInput(ref IAvatarInputInteraction input)
        {
            if (input.Interact == ButtonStates.Pressed)
            {
                if (UseInteraction())
                    input.Interact = ButtonStates.Cancelled;
            }

            switch (input.Select)
            {
                case ButtonStates.Held:
                    UseSelect();
                    break;
                case ButtonStates.Cancelled:
                    UseDeselect();
                    break;
            }
        }

        public bool UseInteraction()
        {
            if (_stack.Count <= 0) return false;
            _stack.Peek()?.Interact();
            return true;
        }
        
        public void UseSelect()
        {
            if (_stack.Count <= 0) return;
            _stack.Peek()?.Select();
        }
        
        public void UseDeselect()
        {
            if (_stack.Count <= 0) return;
            _stack.Peek()?.Deselect();
        }
    }
}