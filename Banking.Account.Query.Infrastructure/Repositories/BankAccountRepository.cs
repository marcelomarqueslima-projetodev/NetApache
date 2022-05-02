using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Query.Infrastructure.Repositories
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(MySqlDbContext context) : base(context) { }

        public async Task<BankAccount> FindByAccountIdentifier(string identifier)
        {
            return await _context.BankAccounts!.Where(x => x.Identifier == identifier).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BankAccount>> FindByAccountHolder(string accountHolder)
        {
            return await _context.BankAccounts!.Where(x => x.AccountHolder == accountHolder).ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> FindBalanceGreaterThan(double balance)
        {
            return await _context.BankAccounts!.Where(x => x.Balance > balance).ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> FindBalanceLessThan(double balance)
        {
            return await _context.BankAccounts!.Where(x => x.Balance < balance).ToListAsync();
        }

        public async Task DeleteByIdentifier(string identifier)
        {
            var bankAccount = await _context.BankAccounts!.Where(x => x.Identifier == identifier).FirstOrDefaultAsync();
            if (bankAccount == null)
            {
                throw new Exception($"Não pode deletar a conta  bancária com ID {identifier}");
            }

            _context.BankAccounts!.Remove(bankAccount);
            await _context.SaveChangesAsync();
        }

        public async Task DepositBankAccountByIdentifier(BankAccount bankAccount)
        {
            var account = await _context.BankAccounts!.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"Não e foi possivel identificar a conta bancária com identificação {bankAccount}");
            }
            account.Balance += bankAccount.Balance;
            await UpdateAsync(account);
        }

        public async Task WithdrawBankAccountByyIdentifier(BankAccount bankAccount)
        {
            var account = await _context.BankAccounts!.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync();
            if (account == null)
            {
                throw new Exception($"Não e foi possivel identificar a conta bancária com identificação {bankAccount}");
            }

            if (account.Balance < bankAccount.Balance)
            {
                throw new Exception($"Valor do saque maaior  que o saldo em conta para retirada {bankAccount.Balance}");
            }
            account.Balance -= bankAccount.Balance;
            await UpdateAsync(account);
        }
    }
}
