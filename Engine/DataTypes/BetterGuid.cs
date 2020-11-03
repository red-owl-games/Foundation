using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty]
    public class BetterGuid : IComparable, IComparable<BetterGuid>, IEquatable<BetterGuid>
    {
        [SerializeField, HideLabel, DisplayAsString, InlineButton("GenerateNewId", "New")]
        private string value;

        private BetterGuid()
        {
            GenerateNewId();
        }
        
        private BetterGuid(string value)
        {
            this.value = value;
        }
        
        private void GenerateNewId() => value = Guid.NewGuid().ToString();

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;
            if (!(other is BetterGuid))
                throw new ArgumentException("Must be BetterGuid");
            BetterGuid guid = (BetterGuid)other;
            return guid.value == value ? 0 : 1;
        }

        public int CompareTo(BetterGuid other)
        {
            return other.value == value ? 0 : 1;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BetterGuid) obj);
        }

        public bool Equals(BetterGuid other)
        {
            return !(other is null) && value == other.value;
        }
        
        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return value;
        }
        
        public static bool operator ==(BetterGuid lhs, BetterGuid rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(BetterGuid lhs, BetterGuid rhs)
        {
            return !(lhs == rhs);
        }
        
        public static implicit operator BetterGuid(Guid guid)
        {
            return new BetterGuid(guid.ToString());
        }

        public static implicit operator Guid(BetterGuid serializableGuid)
        {
            return new Guid(serializableGuid.value);
        }

        public static implicit operator string(BetterGuid serializableGuid)
        {
            return serializableGuid.ToString();
        }
    }
}