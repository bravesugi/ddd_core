using System;
using System.Collections.Generic;

namespace DDD.Core.Model
{
    public sealed class DomainEventPublisher
    {
        [ThreadStatic]
        private static DomainEventPublisher _instance;
        public static DomainEventPublisher Instance
        {
            get
            {
                if (!(_instance is null)) _instance = new DomainEventPublisher();
                return _instance;
            }
        }

        private bool _publishing = false;

        private List<IDomainEventSubscriber<IDomainEvent>> _subscribers;
        private List<IDomainEventSubscriber<IDomainEvent>> Subscribers
        {
            get
            {
                if (_subscribers is null)
                {
                    _subscribers = new List<IDomainEventSubscriber<IDomainEvent>>();
                }

                return _subscribers;
            }
            set
            {
                _subscribers = value;
            }
        }

        private bool HasSubscribers => !(_subscribers is null) && Subscribers.Count != 0;

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            if (_publishing || !HasSubscribers) return;
            try
            {
                _publishing = true;

                var eventType = domainEvent.GetType();
                foreach (var subscriber in Subscribers)
                {
                    var subscribedToType = subscriber.SubscribedToEventType();
                    if (eventType == subscribedToType || subscribedToType == typeof(IDomainEvent))
                    {
                        subscriber.HandleEvent(domainEvent);
                    }
                }
            }
            finally
            {
                _publishing = false;
            }
        }

        public void PublishAll(ICollection<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents) Publish(domainEvent);
        }

        public void Reset()
        {
            if (_publishing) return;
            _subscribers = null;
        }

        public void Subscribe(IDomainEventSubscriber<IDomainEvent> subscriber)
        {
            if (_publishing) return;
            Subscribers.Add(subscriber);
        }

        public void Subscribe(Action<IDomainEvent> handle)
        {
            Subscribe(new DomainEventSubscriber<IDomainEvent>(handle));
        }

        private class DomainEventSubscriber<TEvent> : IDomainEventSubscriber<TEvent> where TEvent : IDomainEvent
        {
            private readonly Action<TEvent> _handle;

            public DomainEventSubscriber(Action<TEvent> handle) => _handle = handle;
            public void HandleEvent(TEvent domainEvent) => _handle(domainEvent);
            public Type SubscribedToEventType() => typeof(TEvent);
        }
    }
}
