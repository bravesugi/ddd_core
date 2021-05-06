using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.Model
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        #region Operator
        public static bool operator ==(ValueObject left, ValueObject right) => Equals(left, right);
        public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
        #endregion

        public bool Equals(ValueObject other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            if (other.GetType() != GetType()) return false;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
        public override bool Equals(object obj) => Equals(obj as ValueObject);
        public override int GetHashCode()
        {
            var hc = new HashCode();
            foreach (var v in GetEqualityComponents()) hc.Add(v);
            return hc.ToHashCode();
        }
        protected abstract IEnumerable<object> GetEqualityComponents();
    }
}
