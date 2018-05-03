using Common;
using Common.Net;
using Common.Operator;
using Common.Security;
using Config;
using Data;
using DataTransferObjects;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.SystemManage
{
    public class UserService
    {

        private readonly string _modelDescription = typeof(User).GetDescription();

        public DataDbContext DataDbContext { get; set; }

        ~UserService()
        {
            if (DataDbContext != null)
                DataDbContext.Dispose();
        }

        public List<User> Search(UserSearchDto dto)
        {
            var dataSource = DataDbContext.Set<User>().AsQueryable();

            if (!string.IsNullOrEmpty(dto.Keywords))
            {
                dataSource = dataSource.Where(m =>
                    (m.RealName != null && m.RealName.Contains(dto.Keywords)) ||
                    (m.NickName != null && m.NickName.Contains(dto.Keywords)) ||
                    (m.MobilePhoneNumber != null && m.MobilePhoneNumber.Contains(dto.Keywords)));
            }

            dataSource = dataSource.OrderByDescending(m => m.LastModifyTime);
            if (dto.IsGetTotalCount)
                dto.TotalCount = dataSource.Count();

            return dataSource.Skip(dto.StartIndex).Take(dto.PageSize).ToList();
        }

        public User CheckLogin(string username, string password)
        {
            var user = DataDbContext.Set<User>().FirstOrDefault(t => t.AccountName == username);
            if (user != null)
            {
                if (user.UserState == Models.Enum.UserStateEnum.Enable)
                {
                    string dbPassword = Encrypt(password);

                    if (dbPassword == user.Password)
                    {
                        user.LogOnCount += 1;
                        user.LastModifyTime = DateTime.Now;
                        DataDbContext.SaveChanges();
                        return user;
                    }
                    else
                    {
                        user.ErrorTimes += 1;
                        user.LastErrorDateTime = DateTime.Now;
                        DataDbContext.SaveChanges();
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户不可用,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }

        public User Login(UserLoginDto dto)
        {
            //if (!SecurityCodeService.IsValid(dto.Token, dto.SecurityCode))
            //    throw new Exception("错误：图形验证码错误！");

            if (string.IsNullOrEmpty(dto.AccountName))
                throw new Exception("错误：请输入您的账号！");
            if (string.IsNullOrEmpty(dto.Password))
                throw new Exception("错误：请输入您的密码！");

            try
            {
                var user = CheckLogin(dto.AccountName, dto.Password);
                if (user != null)
                {
                    OnlineUser onlineUser = new OnlineUser();
                    onlineUser.UserId = user.Id;
                    onlineUser.AccountName = user.AccountName;
                    onlineUser.UserName = user.RealName;
                    onlineUser.NickName = user.NickName;
                    onlineUser.LoginIPAddress = Net.Ip;
                    onlineUser.LoginIPAddressName = Net.GetLocation(onlineUser.LoginIPAddress);
                    onlineUser.LoginTime = DateTime.Now;
                    onlineUser.LoginToken = DesEncrypt.Encrypt(Guid.NewGuid().ToString());
                    if (user.AccountName == "admin")
                    {
                        onlineUser.IsSystem = true;
                    }
                    else
                    {
                        onlineUser.IsSystem = false;
                    }
                    OnlineUserProvider.Provider.AddCurrent(onlineUser);
                    return user;
                }
                throw new Exception("登录失败，用户不存在！");
            }
            catch (Exception ex)
            {
                throw new Exception("登录失败，" + ex.Message);
            }
        }

        public void Save(UserEditDto dto)
        {
            if (dto.UpdateId > 0)
                Update(dto);
            else
                Add(dto);
        }

        public void Add(UserEditDto dto)
        {
            ValidateUpdateDto(dto);

            if (DataDbContext.Set<User>().Any(u => u.AccountName == dto.AccountName))
                throw new Exception($"添加用户失败，{dto.AccountName}已存在！");

            var user = dto.MapTo<User>();
            user.Password = Encrypt(dto.Password);
            user.CreatorTime = DateTime.Now;
            user.LastModifyTime = DateTime.Now;

            DataDbContext.Set<User>().Add(user);
            DataDbContext.SaveChanges();
        }


        public void Update(UserEditDto dto)
        {
            var user = DataDbContext.Set<User>().FirstOrDefault(m => m.Id == dto.UpdateId);
            if (user == null)
                throw new Exception($"错误：指定Id {dto.UpdateId} 的用户不存在！");

            ValidateUpdateDto(dto);
            dto.MapTo<User>(user);
            user.LastModifyTime = DateTime.Now;

            DataDbContext.SaveChanges();
        }


        private static void ValidateUpdateDto(UserEditDto dto)
        {
            if (string.IsNullOrEmpty(dto.AccountName))
                throw new Exception("错误：用户帐号不能为空！");
            if (string.IsNullOrEmpty(dto.Password))
                throw new Exception("错误：用户密码不能为空！");

            //dto.NickName = dto.NickName ?? "";
        }

        public void Remove(params long[] ids)
        {
            if (ids == null || ids.Length == 0)
                throw new Exception("错误，删除的序号为空！");
            foreach (var id in ids)
            {
                var data = DataDbContext.Set<User>().FirstOrDefault(b => b.Id == id);
                if (data == null)
                    throw new Exception($"错误，{_modelDescription}不存在！(Id:{id})");

                DataDbContext.Set<User>().Remove(data);
            }
            DataDbContext.SaveChanges();
        }


        public static string Encrypt(string pwd)
        {
            if (string.IsNullOrWhiteSpace(pwd))
                return string.Empty;

            return Md5.md5(DesEncrypt.Encrypt(Md5.md5(pwd, 32), Settings.UserSecretkey).ToLower(), 32).ToLower();
        }

        public User GetDataById(long id)
        {
            return DataDbContext.Set<User>().FirstOrDefault(b => b.Id == id);
        }
    }
}
