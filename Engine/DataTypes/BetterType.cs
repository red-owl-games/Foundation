using System;
using UnityEngine;

namespace RedOwl.UIX.Engine
{
	[Serializable]
    public class BetterType : IEquatable<Type>, IEquatable<BetterType>
    {
	    [SerializeField]
	    private string id;
	    
	    private Type _type;	
	    public Type Type => (_type == null) ? _type = Type.GetType(id) : _type;

	    public BetterType(Type type)
	    {
		    _type = type;
		    id = type.AssemblyQualifiedName;
	    }

	    public bool Equals(Type other)
	    {
		    if (other == null) return false;
		    return other == Type;
	    }

	    public bool Equals(BetterType other)
	    {
		    if (ReferenceEquals(null, other)) return false;
		    if (ReferenceEquals(this, other)) return true;
		    return id == other.id;
	    }

	    public override bool Equals(object obj)
	    {
		    return ReferenceEquals(this, obj) || obj is BetterType other && Equals(other);
	    }

	    public override int GetHashCode() => (id != null ? id.GetHashCode() : 0);

	    public static bool operator ==(BetterType left, BetterType right) => Equals(left, right);
	    
	    public static bool operator !=(BetterType left, BetterType right) => !Equals(left, right);
	    
	    public static implicit operator Type(BetterType self) => self.Type;
	    
	    public static implicit operator BetterType(Type value) => new BetterType(value);
    }
}