namespace Banking.Cqrs.Core.Events
{
    public class FundsWithdrawnEvent : BaseEvent
    {
        public FundsWithdrawnEvent(string id) : base(id)
        {
        }

        public double Amount { get; set; }
    }
}
