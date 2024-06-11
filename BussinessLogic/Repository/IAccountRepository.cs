using DataAccess.Models;

namespace API.Repository;

public interface IAccountRepository
{
    Task<SystemAccount> GetAccountAsync(int id);
    Task<IEnumerable<SystemAccount>> GetAccountsAsync();
    Task AddAccountAsync(SystemAccount account);
    Task<SystemAccount> GetAccountByEmailAsync(string email);
    Task UpdateAccountAsync(SystemAccount account);
    Task DeleteAccountAsync(int id);
    Task<bool> HasNewsArticlesAsync(int accountId);

}