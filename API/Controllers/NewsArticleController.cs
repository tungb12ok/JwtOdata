using API.Repository;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsArticleController(INewsArticleRepository newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetActiveNewsArticles()
        {
            var articles = await _newsArticleRepository.GetNewsArticlesAsync();
            var activeArticles = articles.Where(a => a.NewsStatus == true).ToList();
            return Ok(activeArticles);
        }
        [Authorize(Roles = "0,1")]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetNewsArticles(string id)
        {
            var article = await _newsArticleRepository.GetNewsArticleAsync(id);
            return Ok(article);
        }
        [Authorize(Roles = "0,1")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetAllNewsArticles()
        {
            var articles = await _newsArticleRepository.GetNewsArticlesAsync();
            return Ok(articles);
        }

        [Authorize(Roles = "0,1")]
        [HttpPost]
        public async Task<ActionResult> CreateNewsArticle(NewsArticle newsArticle)
        {
            await _newsArticleRepository.AddNewsArticleAsync(newsArticle);
            return CreatedAtAction("New Article", new { id = newsArticle.NewsArticleId }, newsArticle);
        }

        [Authorize(Roles = "0,1")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNewsArticle(string id, NewsArticle newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return BadRequest();
            }

            await _newsArticleRepository.UpdateNewsArticleAsync(newsArticle);
            return NoContent();
        }

        [Authorize(Roles = "0,1")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsArticle(string id)
        {
            if (await _newsArticleRepository.IsNewsArticleActiveAsync(id))
            {
                return BadRequest("Cannot delete active news article.");
            }

            await _newsArticleRepository.DeleteNewsArticleAsync(id);
            return NoContent();
        }
        [Authorize(Roles = "0,1")]
        [HttpGet("report")]
        public async Task<IActionResult> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {

            try
            {
                if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                {
                    return BadRequest("Invalid date range.");
                }

                var articles = await _newsArticleRepository.GetArticlesByDateRangeAsync(startDate, endDate);

                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
