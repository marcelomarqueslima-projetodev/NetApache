using MediatR;
using Banking.Account.Query.Domain;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountByHolder
{
    public class FindAccountByHolderQuery : IRequest<IEnumerable<BankAccount>>
    {
        public string AccountHolder { get; set; } = string.Empty;
    }
}
