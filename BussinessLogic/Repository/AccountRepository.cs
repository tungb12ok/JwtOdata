using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogic;
using DataAccess.Models;

namespace API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO _accountDAO;

        public AccountRepository(AccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }

        public Task<SystemAccount> GetAccountAsync(int id) => _accountDAO.GetAccountAsync(id);

        public Task<IEnumerable<SystemAccount>> GetAccountsAsync() => _accountDAO.GetAccountsAsync();

        public Task AddAccountAsync(SystemAccount account) => _accountDAO.AddAccountAsync(account);

        public Task<SystemAccount> GetAccountByEmailAsync(string email) => _accountDAO.GetAccountByEmailAsync(email);

        public Task UpdateAccountAsync(SystemAccount account) => _accountDAO.UpdateAccountAsync(account);

        public Task DeleteAccountAsync(int id) => _accountDAO.DeleteAccountAsync(id);

        public Task<bool> HasNewsArticlesAsync(int accountId) => _accountDAO.HasNewsArticlesAsync(accountId);
    }
}