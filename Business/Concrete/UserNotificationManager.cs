using System;
using System.Collections.Generic;
using System.Net;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class UserNotificationManager:IUserNotificationService
    {
        private IUserNotificationDal _userNotificationDal;

        public UserNotificationManager(IUserNotificationDal userNotificationDal)
        {
            _userNotificationDal = userNotificationDal;
        }

        public IDataResult<UserNotification> GetById(int id)
        {
            return new SuccessDataResult<UserNotification>(_userNotificationDal.Get(p => p.Id == id));
        }

        public IDataResult<List<UserNotification>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<UserNotification>>(_userNotificationDal.GetList(p => p.UserId == userId));
        }

        public IDataResult<List<UserNotification>> GetBySender(string sender)
        {
            return new SuccessDataResult<List<UserNotification>>(_userNotificationDal.GetList(p => p.Sender == sender));
        }

        public IDataResult<List<UserNotification>> GetList()
        {
            return new SuccessDataResult<List<UserNotification>>(_userNotificationDal.GetList());
        }

        public IResult Add(UserNotification userNotification)
        {
            return new SuccessDataResult<int>(_userNotificationDal.Add(userNotification), Messages.UserNotificationAdded);
        }

        public IResult Update(UserNotification userNotification)
        {
            _userNotificationDal.Update(userNotification);
            return new SuccessResult(Messages.UserNotificationUpdated);
        }

        public IResult SetReaded(int id)
        {
            var o = GetById(id);
            if (Equals(!o.Success))
            {
                return o;
            }

            o.Data.Readed = true;
            _userNotificationDal.Update(o.Data);
            return new SuccessResult(Messages.UserNotificationUpdated);
        }

        public IResult Delete(UserNotification userNotification)
        {
            _userNotificationDal.Delete(userNotification);
            return new SuccessResult(Messages.UserNotificationDeleted);
        }

        public IDataResult<string> SendNotifications(string oneSignalId, string header, string text, string data)
        {
            string result = ""; 
            Random rnd = new Random();
            string randomN = rnd.Next(1, 100000).ToString();

            try
            {
                using (var client = new WebClient())
                {
                    var request_json = "{\"app_id\":\"0011b0ec-2fce-4012-989a-56306b697f46\",\"include_player_ids\":[\"" + oneSignalId + "\"],\"channel_for_external_user_ids\":\"push\",\"data\":" + data + ",\"headings\":{\"en\":\"" + header + "\"},\"contents\":{\"en\":\"" + text + "\"},\"small_icon\":\"ic_stat_onesignal_default\",\"android_group\":\"" + randomN + "\",\"template_id\":\"a4f3c3fe-8127-4eeb-a655-f5019bc7c10b\"}";
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers.Add("Authorization", "Basic " + "OGU5OGNkZDQtZjFlOS00MDM4LWE2NWEtNWI3ZWE0NGFjYmQz");
                    result = client.UploadString("https://onesignal.com/api/v1/notifications", "POST", request_json);
                }
                return new SuccessDataResult<string>(result);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<string>(e.Message + " (One signal)\n"+data);
            }
        }

    }
}

