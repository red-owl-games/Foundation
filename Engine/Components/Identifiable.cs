using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class Identifiable : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static readonly Dictionary<Identifiable, BetterGuid> Comp2Id = new Dictionary<Identifiable, BetterGuid>();
        private static readonly Dictionary<BetterGuid, Identifiable> Id2Comp = new Dictionary<BetterGuid, Identifiable>();
        
        [SerializeField]
        private BetterGuid id = null;

        private BetterGuid backupId = null;
        
        public BetterGuid Id => id;
        
        public void OnAfterDeserialize()
        {
            if (id == null || id != backupId)
                Register(this);
        }
        
        public void OnBeforeSerialize()
        {
            if (id == null || id != backupId)
                Register(this);
        }
        
        private void OnDestroy()
        {
            Unregister(this);
            id = null;
        }

        private static void Register(Identifiable identifiable)
        {
            BetterGuid UID;
            if (Comp2Id.TryGetValue(identifiable, out UID))
            {
                // found object instance, update ID
                identifiable.id = UID;
                identifiable.backupId = identifiable.id;
                if (!Id2Comp.ContainsKey(UID))
                    Id2Comp.Add(UID, identifiable);
                return;
            }
    
            if (string.IsNullOrEmpty(identifiable.id))
            {
                // No ID yet, generate a new one.
                identifiable.id = Guid.NewGuid();
                identifiable.backupId = identifiable.id;
                Id2Comp.Add(identifiable.id, identifiable);
                Comp2Id.Add(identifiable, identifiable.id);
                return;
            }

            if (!Id2Comp.TryGetValue(identifiable.id, out var tmp))
            {
                // ID not known to the DB, so just register it
                Id2Comp.Add(identifiable.id, identifiable);
                Comp2Id.Add(identifiable, identifiable.id);
                return;
            }
            if (tmp == identifiable)
            {
                // DB inconsistency
                Comp2Id.Add(identifiable, identifiable.id);
                return;
            }
            if (tmp == null)
            {
                // object in DB got destroyed, replace with new
                Id2Comp[identifiable.id] = identifiable;
                Comp2Id.Add(identifiable, identifiable.id);
                return;
            }
            // we got a duplicate, generate new ID
            identifiable.id = Guid.NewGuid();
            identifiable.backupId = identifiable.id;
            Id2Comp.Add(identifiable.id, identifiable);
            Comp2Id.Add(identifiable, identifiable.id);
        }
        
        private static void Unregister(Identifiable identifiable)
        {
            Comp2Id.Remove(identifiable);
            Id2Comp.Remove(identifiable.id);
        }
    }
    
    public static class IdentifiableExtensions
    {
        public static string Identifier(this Component self, string suffix = "")
        {
            string id = self.TryGetComponent<Identifiable>(out var comp)
                ? comp.Id
                : self.GetInstanceID().ToString();
            return $"{id}.{suffix}";
        }
    }
}