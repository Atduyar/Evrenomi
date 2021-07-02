using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BlogTag:IEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int BlogId { get; set; }
    }
}