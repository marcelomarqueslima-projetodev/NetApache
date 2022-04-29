using Banking.Account.Query.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Contracts.Persistence
{
    public interface IBankAccountRepository : IAsyncReepository<BankAccount>
    {
        Task<BankAccount> FindByAccountIdentifier(string identifier);
        Task<IEnumerable<BankAccount>> FindByAccountHolder(string accountHolder);
        Task<IEnumerable<BankAccount>> FindBalanceGreaterThan(double balance);
        Task<IEnumerable<BankAccount>> FindBalanceLessThan(double balance);

        Task DeleteByIdentifier(string identifier);
        Task DepositBankAccountByIdentifier(BankAccount bankAccount);
        Task WithdrawBankAccountByyIdentifier(BankAccount bankAccount);
    }
}
