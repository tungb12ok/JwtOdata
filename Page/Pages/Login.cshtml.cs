using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Page.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            var client = _httpClientFactory.CreateClient();
            var loginData = new { email, password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7110/api/Account/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(responseData))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        var token = tokenElement.GetString();
                        if (!string.IsNullOrEmpty(token))
                        {
                            HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true });
                            return RedirectToPage("/Index");
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid token received.");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
