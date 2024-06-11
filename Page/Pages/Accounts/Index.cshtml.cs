using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Page.ViewModel;
using System.Text.Json;
using DataAccess.Models;

namespace Page.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [BindProperty]
        public SystemAccount Account { get; set; }
        public IEnumerable<SystemAccountViewModel> Accounts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the access token from the cookie
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7110/api/Account");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            try
            {
                Accounts = JsonConvert.DeserializeObject<IEnumerable<SystemAccountViewModel>>(jsonString);
            }
            catch (Exception ex)
            {
                return Redirect("/Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("https://localhost:7110/api/Account", Account);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Unable to create account.");
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync($"https://localhost:7110/api/Account/{id}", Account);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }

            ModelState.AddModelError(string.Empty, "Unable to update account.");
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["AuthToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"https://localhost:7110/api/Account/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            
            ModelState.AddModelError(string.Empty, "Unable to delete account.");
            return Redirect("/Accounts/Index");
        }
    }
}
