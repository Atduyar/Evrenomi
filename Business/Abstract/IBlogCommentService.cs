using System.Collections.Generic;
using Core.DataAccess.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IBlogCommentService
    {
        IDataResult<BlogComment> GetById(int blogCommentId);

        IDataResult<List<BlogComment>> GetByBlogId(int blogId);
        IDataResult<List<BlogComment>> GetByUserId(int userId);
        IDataResult<List<BlogComment>> GetByStatus(int status, int mask);
        IDataResult<List<BlogComment>> GetByCommentResponse(int blogCommentId);
        IDataResult<List<BlogComment>> GetList();

        IDataResult<int> Add(BlogComment blogComment, int userId);
        IResult Update(BlogComment blogComment);
        IResult Delete(BlogComment blogComment);

        IDataResult<CommentForBlog> GetByCommentIdForBlog(int blogCommentId);
        IDataResult<int> AddCommentForBlog(AddCommentForBlog addCommentForBlog, int userId);
        IDataResult<List<CommentForBlog>> GetByCommentForBlog(int blogId);
        IDataResult<List<CommentForBlog>> GetByCommentResponseForBlog(int blogCommentId);
    }
}