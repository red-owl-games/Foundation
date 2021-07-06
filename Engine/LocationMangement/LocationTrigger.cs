using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class LocationTrigger : MonoBehaviour
    {
        public enum LocationOptions
        {
            Next,
            Previous,
            Specific
        }
        // TODO: add ability to increase trigger requirements for local-coop - IE 2 colliders with X tag

        public LocationOptions options;
        [ShowIf("IsSpecific")]
        public LocationRef location;
        
        [ShowIf("HasTrigger")]
        public int triggerCount = 1;
        [ShowIf("HasTrigger"), TagSelector]
        public string tagFilter = "Untagged";
        [ShowIf("HasTrigger")]
        public LayerMask layerFilter;

        private int _triggerCount;
        private bool _useTagFilter;
        private bool _useLayerFilter;

        private bool IsSpecific()
        {
            return options == LocationOptions.Specific;
        }

        private bool HasTrigger()
        {
            var selfCollider = GetComponent<Collider>();
            if (selfCollider != null && selfCollider.isTrigger) return true;
            foreach (var childCollider in GetComponentsInChildren<Collider>())
            {
                if (childCollider != null && childCollider.isTrigger) return true;
            }

            return false;
        }

        private void Awake()
        {
            _useTagFilter = string.IsNullOrEmpty(tagFilter);
            _useLayerFilter = layerFilter > 0;
        }

        [Button(ButtonSizes.Large), DisableInEditorMode]
        public void RaiseEvent()
        {
            switch (options)
            {
                case LocationOptions.Next:
                    LocationService.Events.LoadNextLocation.Raise();
                    break;
                case LocationOptions.Previous:
                    LocationService.Events.LoadPreviousLocation.Raise();
                    break;
                case LocationOptions.Specific:
                    LocationService.Events.LoadLocation.Raise(location);
                    break;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (MatchesTag(other) && MatchesLayer(other)) _triggerCount += 1;
            if (_triggerCount >= triggerCount) RaiseEvent();
        }

        private void OnTriggerExit(Collider other)
        {
            if (MatchesTag(other) && MatchesLayer(other)) _triggerCount -= 1;
        }

        private bool MatchesTag(Collider other)
        {
            if (_useTagFilter == false) return true;
            return other.gameObject.CompareTag(tagFilter);
        }

        private bool MatchesLayer(Collider other)
        {
            if (_useLayerFilter == false) return true;
            int match = 1 << other.gameObject.layer;
            return (layerFilter & match) == match;
        }
    }
}