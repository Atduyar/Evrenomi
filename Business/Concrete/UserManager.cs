using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;

namespace Business.Concrete
{
    public class UserManager:IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<List<OperationClaim>> GerClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult();
        }

        public IDataResult<List<User>> GetList(Status.Per per)
        {
            return new SuccessDataResult<List<User>>(_userDal.GetList(u => (((Status.GetUserFilter(per) | u.Status) & (2147483647 - (Status.GetUserFilter(per) & u.Status))) & Status.GetUserMask(per)) == 0).ToList());
        }

        public IDataResult<List<UserSummaryDto>> GetListSummary(Status.Per per)
        {
            var user = new SuccessDataResult<List<User>>(_userDal.GetList(u => (((Status.GetUserFilter(per) | u.Status) & (2147483647 - (Status.GetUserFilter(per) & u.Status))) & Status.GetUserMask(per)) == 0).ToList()).Data;
            var result = from u in user
                select new UserSummaryDto
                {
                    Id = u.Id,
                    Nickname = u.Nickname,
                    AvatarUrl = u.AvatarUrl
                };
            return new SuccessDataResult<List<UserSummaryDto>>(result.ToList());
        }

        public IDataResult<User> GetById(int id,Status.Per per)
        {
            User user;
            if (per == Status.Per.System)
                user = _userDal.Get(u => u.Id == id );
            else
                user = _userDal.Get(u => u.Id == id && (((Status.GetUserFilter(per) | u.Status) & (2147483647 - (Status.GetUserFilter(per) & u.Status))) & Status.GetUserMask(per)) == 0);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);
        }

        public IDataResult<User> GetByEmail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Email == email));
        }

        public IDataResult<User> GetByNickname(string nickname)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Nickname == nickname));
        }

        public IDataResult<User> GetByNickname(string nickname, Status.Per per)
        {
            User user;
            if (per == Status.Per.System)
                user = _userDal.Get(u => u.Nickname == nickname);
            else
                user = _userDal.Get(u => u.Nickname == nickname && (((Status.GetUserFilter(per) | u.Status) & (2147483647 - (Status.GetUserFilter(per) & u.Status))) & Status.GetUserMask(per)) == 0);
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>(Messages.UserNotFound);
        }

        public IDataResult<User> GetByEmailOrNickname(string emailOrNickname)
        {
            var userMail = GetByEmail(emailOrNickname);
            var userNickname = GetByNickname(emailOrNickname);

            var user = (userMail.Data == null) ? userNickname : userMail; // user i alır.
            return user;
        }

        public IResult Update(int id, UserDetailsDto userUpdate, Status.Per per)
        {
            var user = GetById(id, Status.Per.System).Data;

            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);//kullanıcı bulunamadı --- id yanlıs olabilir
            }

            if (userUpdate.Nickname != user.Nickname)
            {
                if (GetByNickname(userUpdate.Nickname).Data == null)
                {
                    user.Nickname = userUpdate.Nickname;
                }
                else
                {
                    return new ErrorResult(Messages.NicknameUnchangeable);//kullanıcı adı zaten var
                }
            }

            if (userUpdate.Email != user.Email)
            {
                if (GetByEmail(userUpdate.Email).Data == null)
                {
                    user.Email = userUpdate.Email;
                }
                else
                {
                    return new ErrorResult(Messages.EmailUnchangeable);//Eposta adı zaten var
                }
            }

            if (((Status.GetUseUpdateFilter(per) ^ user.Status) & Status.GetUserUpdateMask(per)) != 0)//adminsen etkisisz
            {
                return new ErrorResult(Messages.UserUpdateBan);//update ban
            }

            User result = new User
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                FirstName = userUpdate.FirstName,
                LastName = userUpdate.LastName,
                Description = userUpdate.Description,
                AvatarUrl = user.AvatarUrl,
                PasswordSalt = user.PasswordSalt,
                PasswordHash = user.PasswordHash,
                Status = user.Status,
                OneSignalId = user.OneSignalId
            };

            _userDal.Update(result);
            return new SuccessResult();
        }

        public IResult UpdateOneSignalId(int id, string userOneSignalId)
        {
            var user = GetById(id, Status.Per.System).Data;

            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);//kullanıcı bulunamadı --- id yanlıs olabilir
            }
            User result = new User
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Description = user.Description,
                AvatarUrl = user.AvatarUrl,
                PasswordSalt = user.PasswordSalt,
                PasswordHash = user.PasswordHash,
                Status = user.Status,
                OneSignalId = userOneSignalId
            };

            _userDal.Update(result);
            return new SuccessResult();
        }

        public IResult UpdateStatus(int id, int status, Status.Per per)
        {
            var user = GetById(id, Status.Per.System).Data;

            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);//kullanıcı bulunamadı --- id yanlıs olabilir
            }

            if (per == Status.Per.System || per == Status.Per.Admin)
            {
                user.Status = status;
            }
            else
            {
                return new ErrorResult();//su anlık yok. kullanıcılar belirli ayarlar ı ayarlayabilecek sadece
            }

            _userDal.Update(user);
            return new SuccessResult();
        }

        public IResult UpdatePp(int id, string url)
        {
            var user = GetById(id,Status.Per.System).Data;
            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);//kullanıcı bulunamadı --- id yanlıs olabilir
            }
            User result = new User
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Description = user.Description,
                AvatarUrl = url,
                PasswordSalt = user.PasswordSalt,
                PasswordHash = user.PasswordHash,
                Status = user.Status,
                OneSignalId = user.OneSignalId
            };

            _userDal.Update(result);
            return new SuccessResult();
        }

    }
}