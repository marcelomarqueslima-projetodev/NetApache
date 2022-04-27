using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.WithdrawnFund
{
    public class WithdrawnFundCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}
