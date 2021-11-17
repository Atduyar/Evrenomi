using System;
using System.Collections.Generic;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class BlogSummaryDto
    {
        public int BlogId { get; set; }
        public string AuthorName { get; set; }
        public string BlogTitle { get; set; }
        public string BlogTitlePhotoUrl { get; set; }
        public string BlogSummary { get; set; }
        public string BlogDate { get; set; }
        public List<Tag> BlogTags { get; set; }
        public int Views { get; set; }
        public bool Readed { get; set; }
    }
}