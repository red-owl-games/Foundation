using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class AvatarInteraction : AvatarAbility
    {
        public override int Priority { get; } = -200;
        
        // TODO: Should we have a ButtonProperty similar to the AnimatorProperty?
        public AvatarInputButtons button = AvatarInputButtons.ButtonSouth;

        public BetterStackTypes stackType = BetterStackTypes.Fifo;
        
        private ButtonStates _button;

        private BetterStack<IInteractable> _stack;

        #region Unity

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _stack.Push(interactable);
                _stack.Peek()?.Select();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Deselect();
                if (_stack.Remove(interactable)) _stack.Peek()?.Select();
            }
        }

        #endregion

        public override void OnStart()
        {
            _stack = new BetterStack<IInteractable>(stackType);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            _button = input.Get(button);
            if (_button != ButtonStates.Pressed) return;
            // TODO: Is this how we want consumeable input to work? Feels like we need another ButtonStates.Consumed
            if (UseInteraction()) input.Set(button, ButtonStates.Cancelled);
        }

        public bool UseInteraction()
        {
            if (_stack.Count <= 0) return false;
            _stack.Peek()?.Interact();
            return true;
        }
    }
}