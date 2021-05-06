using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.Model
{
    public abstract class Entity : IEquatable<Entity>
    {
        #region Operator
        public static bool operator ==(Entity left, Entity right) => Equals(left, right);
        public static bool operator !=(Entity left, Entity right) => !(left == right);
        #endregion

        public bool Equals(Entity other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            if (other.GetType() != GetType()) return false;
            return GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }
        public override bool Equals(object obj) => Equals(obj as Entity);
        public override int GetHashCode()
        {
            var hc = new HashCode();
            foreach (var e in GetIdentityComponents()) hc.Add(e);
            return hc.ToHashCode();
        }
        protected abstract IEnumerable<object> GetIdentityComponents();
    }
}
