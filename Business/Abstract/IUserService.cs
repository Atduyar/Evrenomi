using System.Collections.Generic;
using Core.Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GerClaims(User user);
        void Add(User user);
        User GetByEmail(string email);
        User GetByNickname(string nickname);
        User GetByEmailOrNickname(string emailOrNickname);
    }
}