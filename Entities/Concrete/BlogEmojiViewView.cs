using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BlogEmojiViewView: IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}