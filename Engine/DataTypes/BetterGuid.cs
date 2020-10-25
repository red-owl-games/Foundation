using System;
using Sirenix.OdinInspector;

namespace RedOwl.Engine
{
    [Serializable, InlineProperty]
    public class BetterGuid : IComparable, IComparable<BetterGuid>, IEquatable<BetterGuid>
    {
        [HideLabel, DisplayAsString]
        public readonly string value;

        private BetterGuid(string value)
        {
            this.value = value;
        }

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
            return value == other.value;
        }
        
        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return value != null ? new Guid(value).ToString() : string.Empty;
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
    }
}