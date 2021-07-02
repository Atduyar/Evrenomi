using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class BlogEmojiCountView:IEntity
    {
        public int Id { get; set; }
        public int BlogCount { get; set; }
    }
}