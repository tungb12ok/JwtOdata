using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace BussinessLogic
{
    public class NewsArticleDAO
    {
        private readonly FUNewsManagementDBContext _context;

        public NewsArticleDAO(FUNewsManagementDBContext context)
        {
            _context = context;
        }

        public async Task<NewsArticle> GetNewsArticleAsync(string id)
        {
            return await _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .FirstOrDefaultAsync(na => na.NewsArticleId == id);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesAsync()
        {
            return await _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .ToListAsync();
        }

        public async Task AddNewsArticleAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Add(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsArticleAsync(NewsArticle newsArticle)
        {
            _context.Entry(newsArticle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsArticleAsync(string id)
        {
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            _context.NewsArticles.Remove(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsNewsArticleActiveAsync(string id)
        {
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            return newsArticle != null && newsArticle.NewsStatus == true;
        }
        public async Task<List<NewsArticle>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles
                .Include(x  => x.Tags)
                 .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
                .ToListAsync();
        }
    }
}

