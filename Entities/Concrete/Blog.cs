using System;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Blog:IEntity
    {
        public int BlogId { get; set; }
        public int AuthorId { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogTitle { get; set; }
        public string BlogTitlePhotoUrl { get; set; }
        public string BlogSideTitle { get; set; }
        public string BlogContent { get; set; }
        public int BlogStatus { get; set; }
        public string BlogTags { get; set; }
    }
}