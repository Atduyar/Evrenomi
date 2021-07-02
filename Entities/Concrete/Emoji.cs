using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Emoji: IEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}