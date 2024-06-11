using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BussinessLogic;

public class CategoryDAO
{
    private readonly FUNewsManagementDBContext _context;

    public CategoryDAO(FUNewsManagementDBContext context)
    {
        _context = context;
    }

    public async Task<Category> GetCategoryAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasNewsArticlesAsync(int categoryId)
    {
        return await _context.NewsArticles.AnyAsync(na => na.CategoryId == categoryId);
    }
}