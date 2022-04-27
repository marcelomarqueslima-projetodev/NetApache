using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Events;

namespace Banking.Account.Command.Application.Aggregate
{
    public class AccountAggregate : AggregateRoot
    {
        public bool Active { get; set; }
        public double Balance { get; set; }

        public AccountAggregate()
        {

        }

        public AccountAggregate(OpenAccountCommand command)
        {
            var accountOpenedEvent = new AccountOpenedEvent(
                    command.Id,
                    command.AccountHolder,
                    command.AccountType,
                    DateTime.Now,
                    command.OpeningBalance
                );
            RaiseEvent(accountOpenedEvent);
        }

        public void Apply(AccountOpenedEvent @event)
        {
            Id = @event.Id;
            Active = true;
            Balance = @event.OpenigBalance;
        }

        public void DepositFunds(double amount)
        {
            if (!Active)
            {
                throw new Exception("Os valores não pode ser depositados em uma conta cancelada.");
            }
            if(amount <= 0)
            {
                throw new Exception("Os valor do deposito deverá ser maior que zero.");
            }

            var fundsDepositEvent = new FundsDepositedEvent(Id)
            {
                Id = Id,
                Amount = amount
            };
            RaiseEvent(fundsDepositEvent);
        }

        public void Apply(FundsDepositedEvent @event)
        {
            Id= @event.Id;
            Balance += @event.Amount;
        }

        public void WithdrawFunds(double amount)
        {
            if (!Active)
            {
                throw new Exception("Conta bancária encerrada.");
            }

            var fundsWithdrawnEvent = new FundsWithdrawnEvent(Id)
            {
                Id = Id,
                Amount = amount
            };
            RaiseEvent(fundsWithdrawnEvent);
        }

        public void Apply(FundsWithdrawnEvent @event)
        {
            Id = @event.Id;
            Balance -= @event.Amount;
        }

        public void CloseAccount()
        {
            if (!Active)
            {
                throw new Exception("A conta  está encerrada.");
            }

            var accountCloseEvent = new AccountClosedEvent(Id);
            RaiseEvent(accountCloseEvent);
        }

        public void Apply(AccountClosedEvent @event)
        {
            Id = (@event.Id);
            Active = false;
        }
    }
}
