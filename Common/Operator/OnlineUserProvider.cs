

using Common.Json;
using Common.Security;
using Common.Web;
using System;

namespace Common.Operator
{
    public class OnlineUserProvider
    {
        public static OnlineUserProvider Provider
        {
            get { return new OnlineUserProvider(); }
        }
        private string OnlineUserKey = $"Online_User_{DateTime.Now.Date.ToString("yyyy-MM-dd")}";
        private string LoginProvider = Common.Configs.Configs.GetValue("LoginProvider");

        public OnlineUser GetCurrent()
        {
            OnlineUser operatorModel = new OnlineUser();
            if (LoginProvider == "Cookie")
            {
                operatorModel = DesEncrypt.Decrypt(WebHelper.GetCookie(OnlineUserKey).ToString()).ToObject<OnlineUser>();
            }
            else
            {
                operatorModel = DesEncrypt.Decrypt(WebHelper.GetSession(OnlineUserKey).ToString()).ToObject<OnlineUser>();
            }
            return operatorModel;
        }
        public void AddCurrent(OnlineUser operatorModel)
        {
            if (LoginProvider == "Cookie")
            {
                WebHelper.WriteCookie(OnlineUserKey, DesEncrypt.Encrypt(operatorModel.ToJson()), 60);
            }
            else
            {
                WebHelper.WriteSession(OnlineUserKey, DesEncrypt.Encrypt(operatorModel.ToJson()));
            }
            WebHelper.WriteCookie("Online_User_mac", Md5.md5(Net.Net.GetMacByNetworkInterface().ToJson(), 32));
        }
        public void RemoveCurrent()
        {
            if (LoginProvider == "Cookie")
            {
                WebHelper.RemoveCookie(OnlineUserKey.Trim());
            }
            else
            {
                WebHelper.RemoveSession(OnlineUserKey.Trim());
            }
        }
    }
}
