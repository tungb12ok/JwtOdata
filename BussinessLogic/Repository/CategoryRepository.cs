using System.Collections.Generic;
using System.Threading.Tasks;
using BussinessLogic;
using DataAccess.Models;

namespace API.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO _categoryDAO;

        public CategoryRepository(CategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public Task<Category> GetCategoryAsync(int id) => _categoryDAO.GetCategoryAsync(id);

        public Task<IEnumerable<Category>> GetCategoriesAsync() => _categoryDAO.GetCategoriesAsync();

        public Task AddCategoryAsync(Category category) => _categoryDAO.AddCategoryAsync(category);

        public Task UpdateCategoryAsync(Category category) => _categoryDAO.UpdateCategoryAsync(category);

        public Task DeleteCategoryAsync(int id) => _categoryDAO.DeleteCategoryAsync(id);

        public Task<bool> HasNewsArticlesAsync(int categoryId) => _categoryDAO.HasNewsArticlesAsync(categoryId);
    }
}