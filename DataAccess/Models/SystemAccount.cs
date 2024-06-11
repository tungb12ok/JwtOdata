﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public partial class SystemAccount
    {
        public SystemAccount()
        {
            NewsArticles = new HashSet<NewsArticle>();
        }

        public short AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        public string? AccountPassword { get; set; }
        [JsonIgnore]
        public virtual ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
