using System;
using Core.Entities.Abstract;
using Entities.Dtos;

namespace Entities.Concrete
{
    public class Blog:IEntity
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogTitle { get; set; }
        public string BlogTitlePhotoUrl { get; set; }
        public string BlogSummary { get; set; }
        public string BlogContent { get; set; }
        public int BlogStatus { get; set; }
    }
}