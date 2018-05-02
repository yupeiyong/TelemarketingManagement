﻿using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;


namespace JJsites.WebPc.Lib.net.App_Code
{

    /// <summary>
    ///     Config 的摘要说明
    /// </summary>
    public static class Config
    {

        private static bool noCache = true;
        private static JObject _Items;

        public static JObject Items
        {
            get
            {
                if (noCache || _Items == null)
                {
                    _Items = BuildItems();
                }
                return _Items;
            }
        }


        private static JObject BuildItems()
        {
            var json = File.ReadAllText(HttpContext.Current.Server.MapPath("config.json"));
            return JObject.Parse(json);
        }


        public static T GetValue<T>(string key)
        {
            return Items[key].Value<T>();
        }


        public static string[] GetStringList(string key)
        {
            return Items[key].Select(x => x.Value<string>()).ToArray();
        }


        public static string GetString(string key)
        {
            return GetValue<string>(key);
        }


        public static int GetInt(string key)
        {
            return GetValue<int>(key);
        }

    }

}