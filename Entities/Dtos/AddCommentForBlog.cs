using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public class AddCommentForBlog
    {
        public int BlogId { get; set; }
        [Required]
        [MaxLength(1000)]
        [MinLength(20)]
        public string Text { get; set; }
        public int? ParentBlogCommentId { get; set; } = null;
    }
}