using System;
using System.Linq.Expressions;
using Core.DataAccess.Concrete;
using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserNotificationsDal : EfEntityRepositoryBase<UserNotification, VikingContext>, IUserNotificationDal
    {
    }
}