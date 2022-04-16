namespace Banking.Cqrs.Core.Events
{
    public class AccountOpenedEvent : BaseEvent
    {
        public AccountOpenedEvent(string id, string accountHolder, string accountType, DateTime createdDate, 
                                  double openigBalance) : base(id)
        {
            AccountHolder = accountHolder;
            AccountType = accountType;
            CreatedDate = createdDate;
            OpenigBalance = openigBalance;
        }

        public string AccountHolder { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public double OpenigBalance { get; set; }
    }
}
