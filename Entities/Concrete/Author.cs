using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Author:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public string AuthorDescription { get; set; }
        public int AuthorStatus { get; set; }
    }
}