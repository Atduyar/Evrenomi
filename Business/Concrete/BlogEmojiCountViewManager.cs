using System.Collections.Generic;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BlogEmojiCountViewManager: IBlogEmojiCountViewService
    {
        private IBlogEmojiCountViewDal _blogEmojiCountViewDal;

        public BlogEmojiCountViewManager(IBlogEmojiCountViewDal blogEmojiCountViewDal)
        {
            _blogEmojiCountViewDal = blogEmojiCountViewDal;
        }

        public IDataResult<BlogEmojiCountView> GetByBlogId(int blogId)
        {
            return new SuccessDataResult<BlogEmojiCountView>(_blogEmojiCountViewDal.Get(i => i.Id == blogId));
        }

        public IDataResult<List<BlogEmojiCountView>> GetList()
        {
            return new SuccessDataResult<List<BlogEmojiCountView>>(_blogEmojiCountViewDal.GetList());
        }
    }
}