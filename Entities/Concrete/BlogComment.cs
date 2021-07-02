using System;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BlogComment:IEntity
    {
        public int Id { get; set; }
        public int? ParentBlogCommentId { get; set; }
        public int UserId { get; set; }
        public int BlogId { get; set; }
        public string Text { get; set; }
        public int Status { get; set; }
        public DateTime CommentDate { get; set; }
    }
}