using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BlogEmoji:IEntity
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public int EmojiId { get; set; }
    }
}