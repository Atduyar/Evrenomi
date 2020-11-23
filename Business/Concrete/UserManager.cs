using System.Collections.Generic;
using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class UserManager:IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GerClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public User GetByEmail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        public User GetByNickname(string nickname)
        {
            return _userDal.Get(u => u.Nickname == nickname);
        }
        public User GetByEmailOrNickname(string emailOrNickname)
        {
            var userMail = GetByEmail(emailOrNickname);
            var userNickname = GetByNickname(emailOrNickname);

            var user = (userMail == null) ? userNickname : userMail;// user i alır.
            return user;
        }
    }
}