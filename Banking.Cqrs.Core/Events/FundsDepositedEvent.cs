namespace Banking.Cqrs.Core.Events
{
    public class FundsDepositedEvent : BaseEvent
    {
        public FundsDepositedEvent(string id) : base(id)
        {
        }

        public double Amount { get; set; }
    }
}
