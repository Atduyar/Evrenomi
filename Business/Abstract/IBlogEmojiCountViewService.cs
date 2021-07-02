using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBlogEmojiCountViewService
    {
        IDataResult<BlogEmojiCountView> GetByBlogId(int blogId);
        IDataResult<List<BlogEmojiCountView>> GetList();
        //IDataResult<List<BlogEmojiView>> GetByUserId(int userId);          //En büyüğünü filan alabilir
    }
}