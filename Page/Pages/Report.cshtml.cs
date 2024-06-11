using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;
using System.Text.Json;
namespace Page.Pages
{
    public class ReportModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IList<NewsArticle> NewsArticles { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddMonths(-1).Date;

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;


        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var sDate = StartDate.ToString("yyyy-MM-dd");
            var eDate = EndDate.ToString("yyyy-MM-dd");

            try
            {
                var response = await client.GetAsync($"https://localhost:7110/api/NewsArticle/report?startDate={sDate}&endDate={eDate}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Error");
                }

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNameCaseInsensitive = true
                };
        
                var jsonString = await response.Content.ReadAsStringAsync();
                NewsArticles = JsonSerializer.Deserialize<IList<NewsArticle>>(jsonString, options);

                return Page();
            }
            catch (HttpRequestException ex)
            {
                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                return RedirectToPage("/Error");
            }
        }

    }
}