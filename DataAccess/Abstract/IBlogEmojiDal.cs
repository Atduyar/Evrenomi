using Core.DataAccess.Abstract;
using Core.Entities.Abstract;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IBlogEmojiDal: IEntityRepository<BlogEmoji>
    {
    }
}