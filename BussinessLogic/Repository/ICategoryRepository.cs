using DataAccess.Models;

namespace API.Repository;

public interface ICategoryRepository
{
    Task<Category> GetCategoryAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task<bool> HasNewsArticlesAsync(int categoryId); // New method
}