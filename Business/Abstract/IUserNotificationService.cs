using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserNotificationService
    {
        IDataResult<UserNotification> GetById(int id);
        IDataResult<List<UserNotification>> GetByUserId(int userId);
        IDataResult<List<UserNotification>> GetBySender(string sender);
        IDataResult<List<UserNotification>> GetList();
        IResult Add(UserNotification userNotification);
        IResult Update(UserNotification userNotification);
        IResult SetReaded(int id);
        IResult Delete(UserNotification userNotification);
        IDataResult<string> SendNotifications(string oneSignalId, string header, string text, string data);
    }
}