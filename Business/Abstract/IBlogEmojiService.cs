using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBlogEmojiService
    {
        IDataResult<BlogEmoji> GetById(int id);
        IDataResult<List<BlogEmoji>> GetByBlogId(int blogId);
        IDataResult<List<BlogEmoji>> GetByUserId(int userId);
        IDataResult<List<BlogEmoji>> GetByEmojiId(int emojiId);
        IDataResult<List<BlogEmoji>> GetList();
        IDataResult<BlogEmoji> GetByEmojiView(BlogEmojiView blogEmojiView);
        IResult Add(BlogEmoji blogEmoji);
        IResult Update(BlogEmoji blogEmoji);
        IResult Delete(BlogEmoji blogEmoji);
    }
}