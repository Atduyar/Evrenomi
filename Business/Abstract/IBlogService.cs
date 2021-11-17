using System.Collections.Generic;
using Business.Constants;
using Core.DataAccess.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IBlogService
    {
        IDataResult<string> GetHtmlBlog(BlogDetailDto blogDetailDto, int userId);
        IDataResult<string> GetHtmlBlog(BlogDetailDto blogDetailDto);
        IDataResult<Blog> GetById(int blogId, Status.Per per = Status.Per.User);
        IDataResult<List<BlogSummaryDto>> GetByAuthorId(int authorId, PageFilter pageFilter, Status.Per per = Status.Per.User);
        IDataResult<List<Blog>> GetByAuthorId(int authorId, Status.Per per = Status.Per.User);
        IDataResult<List<Blog>> GetByStatus(int mask, int filter);
        IDataResult<List<BlogSummaryDto>> GetByUserReaded(int userId, PageFilter pageFilter, Status.Per per = Status.Per.User);
        IDataResult<List<Blog>> GetList();
        IDataResult<List<BlogSummaryDto>> GetSearchList(List<string> text, Status.Per per = Status.Per.User);
        IDataResult<List<BlogSummaryDto>> GetListSummary();
        IDataResult<List<Tag>> GetTags(int blogId);
        IDataResult<int> Add(Blog blog);
        IResult Delete(Blog blog);
        IResult Update(Blog blog);
        IResult UpdateStatus(int blogId, int status, Status.Per per);
        IDataResult<List<BlogSummaryDto>> GetPage(PageFilter blogPageFilter, int userId, out object metadata, Status.Per per);
        IDataResult<List<BlogSummaryDto>> GetPage(PageFilter blogPageFilter, out object metadata, Status.Per per);
        IDataResult<List<BlogSummaryDto>> GetPage(PageFilter blogPageFilter, int userId, Status.Per per);
        IDataResult<List<BlogSummaryDto>> GetPage(PageFilter blogPageFilter, Status.Per per);
        void GetBlogView(int blogId, int userId);
    }
}