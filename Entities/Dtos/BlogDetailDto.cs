using System;
using System.Collections.Generic;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class BlogDetailDto
    {
        public int BlogId { get; set; }
        //public int AuthorId { get; set; }
        public UserSummaryDto AuthorSummary { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogTitle { get; set; }
        public string BlogTitlePhotoUrl { get; set; }
        public string BlogSummary { get; set; }
        public List<BlogElements> BlogContent { get; set; }
        public List<Tag> BlogTags { get; set; }
    }
}