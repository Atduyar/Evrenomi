using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BlogEmojiView:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EmojiId { get; set; }
    }
}