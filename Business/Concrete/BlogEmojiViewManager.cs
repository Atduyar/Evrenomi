using System;
using System.Collections.Generic;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BlogEmojiViewManager: IBlogEmojiViewService
    {
        private IBlogEmojiViewDal _blogEmojiViewDal;

        public BlogEmojiViewManager(IBlogEmojiViewDal blogEmojiViewDal)
        {
            _blogEmojiViewDal = blogEmojiViewDal;
        }

        public IDataResult<List<BlogEmojiView>> GetByBlogId(int blogId)
        {
            return new SuccessDataResult<List<BlogEmojiView>>(_blogEmojiViewDal.GetList(i => i.Id == blogId));
        }

        public IDataResult<List<BlogEmojiView>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<BlogEmojiView>>(_blogEmojiViewDal.GetList(i => i.UserId == userId));
        }

        public IDataResult<List<BlogEmojiView>> GetByEmojiId(int emojiId)
        {
            return new SuccessDataResult<List<BlogEmojiView>>(_blogEmojiViewDal.GetList(i => i.EmojiId == emojiId));
        }

        public IDataResult<List<BlogEmojiView>> GetList()
        {
            return new SuccessDataResult<List<BlogEmojiView>>(_blogEmojiViewDal.GetList());
        }

        public IDataResult<BlogEmojiView> UserViewed(int blogId, int userId, int emojiId)
        {
            try
            {
                var u = _blogEmojiViewDal.Get(i => i.UserId == userId && i.Id == blogId && i.EmojiId == emojiId);

                return new SuccessDataResult<BlogEmojiView>(u);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<BlogEmojiView>(e.Message);
            }
            //if (u == null)
            //{
            //    return new ErrorResult();
            //}
            //return new SuccessResult();
        }
    }
}