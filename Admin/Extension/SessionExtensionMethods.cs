using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Mvc.Extension
{
    public static class SessionExtensionMethods
    {
        public static void SetObject(this ISession session,string key,object value)
        {
            string str = JsonConvert.SerializeObject(value);
            session.SetString(key,str);
        }

        public static T GetObject<T>(this ISession session,string key) where T:class
        {
            string str = session.GetString(key);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                T obj = JsonConvert.DeserializeObject<T>(str);
                return obj;
            }
        }
    }
}
