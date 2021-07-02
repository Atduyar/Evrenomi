using System;
using System.Net;
using Admin.Controllers;
using Admin.Models;
using Admin.Models.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Admin.Concrete
{
public class ConnectionManager
    {
        static public class Con
        {
            static public string BaseUrl = "https://api.atduyar.com/api/";
            static public IDataResult<T> get<T>(string url, string json, string metod, bool auth = true)
            {

                string result = "";
                using (var client = new WebClient())
                {
                    try
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        if (auth)
                            client.Headers.Add("Authorization", "Bearer " + ConT.t);
                        if (json == "")
                            result = client.DownloadString(BaseUrl + url);//get
                        else
                            result = client.UploadString(BaseUrl + url, metod, json);

                        T t;
                        t = JsonConvert.DeserializeObject<T>(result);

                        return new SuccessDataResult<T>(t);
                    }
                    catch (Exception e)
                    {
                        return new ErrorDataResult<T>();
                    }
                }
            }

            static public IDataResult<string> get(string url, bool auth = true)
            {

                string result = "";
                using (var client = new WebClient())
                {
                    try
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        if (auth)
                            client.Headers.Add("Authorization", "Bearer " + ConT.t);
                        
                        result = client.DownloadString(BaseUrl + url);//get
                        return new SuccessDataResult<string>(data: result);
                    }
                    catch (Exception e)
                    {
                        return new ErrorDataResult<string>();
                    }
                }
            }
        }
    }

    static class ConT
    {
        static public string BaseUrl = "https://api.atduyar.com/api/auth/login";
        static public string t { get; set; }
        static public DateTime e { get; set; }

        static public string getT(string json, HttpContext hc)
        {

            string result = "";
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = client.UploadString(BaseUrl, "POST", json);

                    Token t = JsonConvert.DeserializeObject<Token>(result);
                    ConT.t = t.token;
                    ConT.e = t.expiration;
                    hc.Session.SetString("Token", t.token);

                    return ConT.t;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
    }
}