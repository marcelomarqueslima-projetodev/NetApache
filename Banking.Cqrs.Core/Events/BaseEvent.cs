using Banking.Cqrs.Core.Messages;

namespace Banking.Cqrs.Core.Events
{
    public class BaseEvent : Message
    {
        public BaseEvent(string id) : base(id)
        {
        }

        public int Version { get; set; }
    }
}
