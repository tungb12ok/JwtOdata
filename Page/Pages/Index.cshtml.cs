using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;
using System.Text.Json;
using DataAccess.Models;

namespace Page.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
        }

        public IList<NewsArticle> NewsArticles { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var response = await client.GetAsync("https://localhost:7110/api/NewsArticle");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                NewsArticles = JsonSerializer.Deserialize<IList<NewsArticle>>(jsonString, options);
            }
            catch (HttpRequestException ex)
            {
                // Handle exception
                System.Diagnostics.Debug.WriteLine($"Request error: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                // Handle exception
                System.Diagnostics.Debug.WriteLine($"The content type is not supported: {ex.Message}");
            }
            catch (JsonException ex)
            {
                // Handle exception
                System.Diagnostics.Debug.WriteLine($"Invalid JSON: {ex.Message}");
            }
        }
    }
}