using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class BlogViewDto
    {
        public int BlogId { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public DateTime BlogDateT { get; set; }
        public string BlogDate { get; set; }
        public string BlogTitle { get; set; }
        public string BlogTitlePhotoUrl { get; set; }
        public string BlogSummary { get; set; }
        public string BlogContent { get; set; }
        public string BlogTags { get; set; }



        public int Views { get; set; }
    }
}
