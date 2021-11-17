using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IBlogTagService
    {
        IDataResult<BlogTag> GetById(int id);
        IDataResult<List<BlogTag>> GetByBlogId(int blogId);
        IDataResult<List<BlogTag>> GetByTagId(int tagId);
        IDataResult<BlogTag> GetByBlogIdAndTagId(int blogId, int tagId);
        IDataResult<List<BlogTag>> GetList();
        IResult Add(BlogTag BlogTag);
        IResult Update(BlogTag BlogTag);
        IResult Delete(BlogTag BlogTag);
    }
}