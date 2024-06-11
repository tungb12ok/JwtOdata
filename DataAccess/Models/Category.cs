using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public partial class Category
    {
        public Category()
        {
            NewsArticles = new HashSet<NewsArticle>();
        }

        public short CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDesciption { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
