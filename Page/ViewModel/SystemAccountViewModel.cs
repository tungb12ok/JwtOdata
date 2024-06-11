using System.Text.Json.Serialization;

namespace Page.ViewModel
{
    public class SystemAccountViewModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public int AccountRole { get; set; }
        public string AccountPassword { get; set; }
        public List<NewsArticleViewModel> NewsArticles { get; set; }
    }

}
