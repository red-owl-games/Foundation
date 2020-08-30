using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Core
{
    public class WaitForNextEvent : CustomYieldInstruction
    {
        private bool _keepWaiting;
        public override bool keepWaiting => _keepWaiting;

        private readonly TelegramReference _evt;

        public WaitForNextEvent(TelegramReference evt)
        {
            _evt = evt;
            _evt.Subscribe(HandleEvent);
            _keepWaiting = true;
        }

        private void HandleEvent()
        {
            _evt.Unsubscribe(HandleEvent);
            _keepWaiting = false;
            
        }
    }

    public interface ITelegram
    {
        void Raise();
        void Subscribe(Action listener);
        void Unsubscribe(Action listener); 
    }

    [HideMonoScript]
    [CreateAssetMenu(menuName = "Red Owl/Telegram")]
    public class TelegramReference : ScriptableObject, ITelegram
    {
        [SerializeReference, ValueDropdown("PossibleValues"), OnValueChanged("UpdatePayload"), InlineProperty, HideLabel]
        private string key;

        [Title("Payload")]
        [ShowInInspector, InlineProperty, HideLabel, HideReferenceObjectPicker]
        private ISignal payload;

        private ValueDropdownList<string> PossibleValues()
        {
            var names = new List<string>(Telegraph.Signals.Names);
            var output = new ValueDropdownList<string>();
            foreach (string t in names)
            {
                output.Add(t.Replace(".", "/").Replace("+", "/"), t);
            }

            return output;
        }

        private void UpdatePayload()
        {
            payload = Telegraph.GetDefault(key);
        }

        private void OnValidate()
        {
            if (payload == null) payload = Telegraph.GetDefault(key);
        }

        [Title("")]
        [Button(ButtonSizes.Large)]
        public void Raise()
        {
            Log.Always($"Raising {key}");
            Telegraph.Send(key, payload);
        }

        public void Subscribe(Action listener) => Telegraph.Subscribe(key, listener);
        public void Unsubscribe(Action listener) => Telegraph.Unsubscribe(key, listener);
        
        public WaitForNextEvent OnNext() => new WaitForNextEvent(this);
    }
}