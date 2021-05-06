using DDD.Core.Model;

namespace DDD.Core.Events
{
    public interface IEventStore
    {
        long CountStoredEvents();
        StoredEvent[] GetAllStoredEventsSince(long storedEventId);
        StoredEvent[] GetAllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        StoredEvent Append(IDomainEvent domainEvent);
        void Close();
    }
}
