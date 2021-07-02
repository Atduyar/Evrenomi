using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBlogEmojiCountViewDal: EfEntityRepositoryBase<BlogEmojiCountView, VikingContext>, IBlogEmojiCountViewDal
    {
    }
}