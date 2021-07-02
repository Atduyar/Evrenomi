using System.Collections.Generic;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BlogEmojiManager:IBlogEmojiService
    {
        private IBlogEmojiDal _blogEmojiDal;

        public BlogEmojiManager(IBlogEmojiDal blogEmojiDal)
        {
            _blogEmojiDal = blogEmojiDal;
        }

        public IDataResult<BlogEmoji> GetById(int id)
        {
            return new SuccessDataResult<BlogEmoji>(_blogEmojiDal.Get(be => be.Id == id));
        }

        public IDataResult<List<BlogEmoji>> GetByBlogId(int blogId)
        {
            return new SuccessDataResult<List<BlogEmoji>>(_blogEmojiDal.GetList(be => be.BlogId == blogId));
        }

        public IDataResult<List<BlogEmoji>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<BlogEmoji>>(_blogEmojiDal.GetList(be => be.UserId == userId));
        }

        public IDataResult<List<BlogEmoji>> GetByEmojiId(int emojiId)
        {
            return new SuccessDataResult<List<BlogEmoji>>(_blogEmojiDal.GetList(be => be.EmojiId == emojiId));
        }

        public IDataResult<BlogEmoji> GetByEmojiView(BlogEmojiView blogEmojiView)
        {
            return new SuccessDataResult<BlogEmoji>(_blogEmojiDal.Get(be => be.BlogId == blogEmojiView.Id && be.UserId == blogEmojiView.UserId && be.EmojiId == blogEmojiView.EmojiId));
        }

        public IDataResult<List<BlogEmoji>> GetList()
        {
            return new SuccessDataResult<List<BlogEmoji>>(_blogEmojiDal.GetList());
        }

        public IResult Add(BlogEmoji blogEmoji)
        {
            _blogEmojiDal.Add(blogEmoji);
            return new SuccessResult(Messages.BlogEmojiAdded);
        }

        public IResult Update(BlogEmoji blogEmoji)
        {
            _blogEmojiDal.Update(blogEmoji);
            return new SuccessResult(Messages.BlogEmojiUpdated);
        }

        public IResult Delete(BlogEmoji blogEmoji)
        {
            _blogEmojiDal.Delete(blogEmoji);
            return new SuccessResult(Messages.BlogEmojiDeleted);
        }
    }
}