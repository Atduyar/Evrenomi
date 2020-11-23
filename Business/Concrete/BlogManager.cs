using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BlogManager: IBlogService
    {
        private IBlogDal _blogDal;

        public BlogManager(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }

        public IDataResult<Blog> GetById(int blogId)
        {
            return new SuccessDataResult<Blog>(_blogDal.Get(p => p.BlogId == blogId));
        }

        public IDataResult<List<Blog>> GetList()
        {
            return new SuccessDataResult<List<Blog>>(_blogDal.GetList().ToList());
        }

        public IResult Add(Blog blog)
        {
            _blogDal.Add(blog);
            return new SuccessResult(Messages.BlogAdded);
        }

        public IResult Update(Blog blog)
        {
            _blogDal.Update(blog); 
            return new SuccessResult(Messages.BlogUpdated);
        }

        public IResult Delete(Blog blog)
        {
            _blogDal.Delete(blog);
            return new SuccessResult(Messages.BlogDeleted);
        }
    }
}