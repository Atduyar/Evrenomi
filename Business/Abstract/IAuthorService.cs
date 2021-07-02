using System.Collections.Generic;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IAuthorService
    {
        IDataResult<List<Author>> GetList();
        IDataResult<Author> GetById(int authorId);
        IDataResult<Author> GetByName(string name);
        IDataResult<Author> GetByUserId(int userId);
        IDataResult<List<AuthorSummaryDto>> GetListSummary();
        IResult Add(Author author);
        IResult AuthorExists(AuthorForRegister authorForRegister, int userId);
    }
}