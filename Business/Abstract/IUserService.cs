using System.Collections.Generic;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GerClaims(User user);
        IResult Add(User user);
        IDataResult<List<User>> GetList(Status.Per per);
        IDataResult<List<UserSummaryDto>> GetListSummary(Status.Per per);
        IDataResult<User> GetById(int id,Status.Per per);
        IDataResult<User> GetByEmail(string email);
        IDataResult<User> GetByNickname(string nickname);
        IDataResult<User> GetByNickname(string nickname, Status.Per per);
        IDataResult<User> GetByEmailOrNickname(string emailOrNickname);
        IResult Update(int id, UserDetailsDto userUpdate, Status.Per per);
        IResult UpdateStatus(int id, int status, Status.Per per);
        IResult UpdatePp(int id, string url);
        IResult UpdateOneSignalId(int id, string userOneSignalId);
    }
}