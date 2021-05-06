using System.Collections.Generic;
using System.Threading;

namespace DDD.Core.Model
{
    public abstract class EventSourcedRootEntity : Entity
    {
        private readonly List<IDomainEvent> _mutatingEvents = new List<IDomainEvent>();
        private int _unmutatedVersion;

        #region .ctor
        public EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion)
        {
            foreach (var e in eventStream) When(e);
            _unmutatedVersion = streamVersion;
        }
        #endregion

        protected int MutatedVersion => Interlocked.Increment(ref _unmutatedVersion);
        protected int UnmutatedVersion => _unmutatedVersion;

        public IList<IDomainEvent> GetMutatingEvents() => _mutatingEvents.ToArray();
        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            When(e);
        }
        protected abstract void When(IDomainEvent e);
    }
}
