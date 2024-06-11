
using DataAccess.Models;

namespace API.Repository;

public interface INewsArticleRepository
{
    Task<NewsArticle> GetNewsArticleAsync(string id);
    Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync();
    Task AddNewsArticleAsync(NewsArticle newsArticle);
    Task UpdateNewsArticleAsync(NewsArticle newsArticle);
    Task DeleteNewsArticleAsync(string id);
    Task<bool> IsNewsArticleActiveAsync(string id);
    Task<List<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);

}