using System.Collections.Generic;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BlogTagManager:IBlogTagService
    {
        private IBlogTagDal _blogTagDal;

        public BlogTagManager(IBlogTagDal blogTagDal)
        {
            _blogTagDal = blogTagDal;
        }

        public IDataResult<BlogTag> GetById(int id)
        {
            return new SuccessDataResult<BlogTag>(_blogTagDal.Get(bt => bt.Id == id));
        }

        public IDataResult<List<BlogTag>> GetByBlogId(int blogId)
        {
            return new SuccessDataResult<List<BlogTag>>(_blogTagDal.GetList(bt => bt.BlogId == blogId));
        }

        public IDataResult<List<BlogTag>> GetByTagId(int tagId)
        {
            return new SuccessDataResult<List<BlogTag>>(_blogTagDal.GetList(bt => bt.TagId == tagId));
        }

        public IDataResult<List<BlogTag>> GetList()
        {
            return new SuccessDataResult<List<BlogTag>>(_blogTagDal.GetList());
        }

        public IResult Add(BlogTag blogTag)
        {
            _blogTagDal.Add(blogTag);
            return new SuccessResult(Messages.BlogTagAdded);
        }

        public IResult Update(BlogTag blogTag)
        {
            _blogTagDal.Update(blogTag);
            return new SuccessResult(Messages.BlogTagUpdated);
        }

        public IResult Delete(BlogTag blogTag)
        {
            _blogTagDal.Delete(blogTag);
            return new SuccessResult(Messages.BlogTagDeleted);
        }
    }
}