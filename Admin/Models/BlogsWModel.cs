using System.Collections.Generic;
using Entities.Concrete;
using Entities.Dtos;

namespace Admin.Models
{
    public class BlogsWModel
    {
        public List<Blog> PendingBlogs { get; set; }
        public string Token { get; set; }
    }
}