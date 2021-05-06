using DDD.Core.Model;
using System;
using System.Collections.Generic;

namespace DDD.Core.Events
{
    public sealed class StoredEvent : IEquatable<StoredEvent>
    {
        #region .ctor
        public StoredEvent(string typeName, DateTime occurredOn, string eventBody, long eventId = -1L)
        {
            TypeName = typeName;
            OccurredOn = occurredOn;
            EventBody = eventBody;
            EventId = eventId;
        }
        #endregion

        public string TypeName { get; }
        public DateTime OccurredOn { get; }
        public string EventBody { get; }
        public long EventId { get; }

        public TEvent ToDomainEvent<TEvent>() where TEvent : IDomainEvent
        {
            var eventType = Type.GetType(TypeName);
            return (TEvent)EventSerializer.Instance.Deserialize(EventBody, eventType);
        }
        public IDomainEvent ToDomainEvent() => ToDomainEvent<IDomainEvent>();
        public bool Equals(StoredEvent other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            if (other.GetType() != GetType()) return false;
            return EqualityComparer<long>.Default.Equals(EventId, other.EventId);
        }
        public override bool Equals(object obj) => Equals(obj as StoredEvent);
        public override int GetHashCode() => HashCode.Combine(EventId);
    }
}
