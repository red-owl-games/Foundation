using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class AvatarInteraction : AvatarAbility
    {
        public override int Priority { get; } = -200;
        
        public AvatarInputButtons selectButton = AvatarInputButtons.TriggerRight;
        public AvatarInputButtons interactButton = AvatarInputButtons.ButtonSouth;
        

        public BetterStackTypes stackType = BetterStackTypes.Fifo;
        
        private ButtonStates _select;
        private ButtonStates _interact;

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

        public override void HandleInput(ref AvatarInput input)
        {
            _interact = input.Get(interactButton);
            if (_interact == ButtonStates.Pressed)
            {
                if (UseInteraction())
                    input.Set(interactButton, ButtonStates.Cancelled);
            }

            _select = input.Get(selectButton);
            if (_select == ButtonStates.Held)
            {
                UseSelect();
            }
            if (_select == ButtonStates.Cancelled)
            {
                UseDeselect();
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