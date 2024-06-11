using BussinessLogic;
using DataAccess.Models;

namespace API.Repository;

public class NewsArticleRepository : INewsArticleRepository
{
    private readonly NewsArticleDAO _newsArticleDAO;

    public NewsArticleRepository(NewsArticleDAO newsArticleDAO)
    {
        _newsArticleDAO = newsArticleDAO;
    }

    public Task<NewsArticle> GetNewsArticleAsync(string id) => _newsArticleDAO.GetNewsArticleAsync(id);

    public Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync() => _newsArticleDAO.GetNewsArticlesAsync();

    public Task AddNewsArticleAsync(NewsArticle newsArticle) => _newsArticleDAO.AddNewsArticleAsync(newsArticle);

    public Task UpdateNewsArticleAsync(NewsArticle newsArticle) => _newsArticleDAO.UpdateNewsArticleAsync(newsArticle);

    public Task DeleteNewsArticleAsync(string id) => _newsArticleDAO.DeleteNewsArticleAsync(id);

    public Task<bool> IsNewsArticleActiveAsync(string id) => _newsArticleDAO.IsNewsArticleActiveAsync(id);

    public Task<List<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate) =>
        _newsArticleDAO.GetArticlesByDateRangeAsync(startDate, endDate);
}
