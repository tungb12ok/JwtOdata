using API.Repository;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic;

public class AccountDAO
{
    private readonly FUNewsManagementDBContext _context;

    public AccountDAO(FUNewsManagementDBContext context)
    {
        _context = context;
    }

    public async Task<SystemAccount> GetAccountAsync(int id)
    {
        return await _context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountId == id);
    }

    public async Task<SystemAccount> GetAccountByEmailAsync(string email)
    {
        return await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
    }

    public async Task<IEnumerable<SystemAccount>> GetAccountsAsync()
    {
        return await _context.SystemAccounts.ToListAsync();
    }

    public async Task AddAccountAsync(SystemAccount account)
    {
        var lastId = await _context.SystemAccounts.MaxAsync(a => (int?)a.AccountId) ?? 0;

        account.AccountId = (short)(lastId + 1);

        _context.SystemAccounts.Add(account);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateAccountAsync(SystemAccount account)
    {
        _context.Entry(account).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(int id)
    {
        var account = await _context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountId == id);
        _context.SystemAccounts.Remove(account);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasNewsArticlesAsync(int accountId)
    {
        return await _context.NewsArticles.AnyAsync(na => na.CreatedById == accountId);
    }
}