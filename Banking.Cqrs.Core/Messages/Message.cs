namespace Banking.Cqrs.Core.Messages
{
    public abstract class Message
    {
        public Message(string id)
        {
            Id = id;
        }

        public string Id { get; set; } = string.Empty;
    }
}
