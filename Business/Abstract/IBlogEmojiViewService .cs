using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBlogEmojiViewService
    {
        IDataResult<List<BlogEmojiView>> GetByBlogId(int blogId);
        IDataResult<List<BlogEmojiView>> GetByUserId(int userId);
        IDataResult<List<BlogEmojiView>> GetByEmojiId(int emojiId);
        IDataResult<List<BlogEmojiView>> GetList();
        IDataResult<BlogEmojiView> UserViewed(int blogId, int userId, int emojiId);
    }
}