using System;

namespace DDD.Core.Model
{
    public abstract class Identity : IEquatable<Identity>, IIdentity
    {
        #region Operator
        public static bool operator ==(Identity left, Identity right) => Equals(left, right);
        public static bool operator !=(Identity left, Identity right) => !(left == right);
        #endregion

        #region .ctor
        public Identity() : this(Guid.NewGuid().ToString()) { }
        public Identity(string id) => Id = id;
        #endregion

        public string Id { get; protected set; }

        public bool Equals(Identity other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            if (other.GetType() != GetType()) return false;
            return string.Equals(Id, other.Id, StringComparison.Ordinal);
        }
        public override bool Equals(object other) => Equals(other as Identity);
        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
