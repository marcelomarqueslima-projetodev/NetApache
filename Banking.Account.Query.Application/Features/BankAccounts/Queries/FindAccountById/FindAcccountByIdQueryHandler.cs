using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountById
{
    public class FindAcccountByIdQueryHandler : IRequestHandler<FindAcccountByIdQuery, BankAccount>
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public FindAcccountByIdQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public  async Task<BankAccount> Handle(FindAcccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await _bankAccountRepository.FindByAccountIdentifier(request.Identifier);
        }
    }
}
