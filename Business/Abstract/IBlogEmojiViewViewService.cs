using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBlogEmojiViewViewService
    {
        IDataResult<List<BlogEmojiViewView>> GetByBlogId(int blogId);
        IDataResult<List<BlogEmojiViewView>> GetByUserId(int userId);
        IDataResult<List<BlogEmojiViewView>> GetList();
        IResult UserViewed(int blogId, int userId);
    }
}