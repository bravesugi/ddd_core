using DDD.Core.Model;
using Newtonsoft.Json;
using System;

namespace DDD.Core.Events
{
    public sealed class EventSerializer
    {
        [ThreadStatic]
        private static readonly Lazy<EventSerializer> _instance = new Lazy<EventSerializer>(() => new EventSerializer(), true);
        public static EventSerializer Instance => _instance.Value;

        private readonly bool _isPretty;

        public EventSerializer(bool isPretty = false) => _isPretty = isPretty;

        public T Deserialize<T>(string serialization) => JsonConvert.DeserializeObject<T>(serialization);
        public object Deserialize(string serialization, Type type) => JsonConvert.DeserializeObject(serialization, type);
        public string Serialize(IDomainEvent domainEvent) => JsonConvert.SerializeObject(domainEvent, _isPretty ? Formatting.Indented : Formatting.None);
    }
}
