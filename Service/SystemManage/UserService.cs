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

        public DataDbContext DataDbContext { get; set; }


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
                if (user.UserState != Models.Enum.UserStateEnum.Enable)
                {
                    string dbPassword = Encrypt(password);

                    if (dbPassword == user.Password)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        user.LogOnCount += 1;
                        user.LastModifyTime = DateTime.Now;
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
                //if (Session["nfine_session_verifycode"].IsEmpty() || Md5.md5(code.ToLower(), 16) != Session["nfine_session_verifycode"].ToString())
                //{
                //    throw new Exception("验证码错误，请重新输入");
                //}

                var user = CheckLogin(dto.AccountName, dto.Password);
                if (user != null)
                {
                    OnlineUser onlineUser = new OnlineUser();
                    onlineUser.UserId = user.Id;
                    onlineUser.AccountName = user.AccountName;
                    onlineUser.UserName = user.RealName;
                    //onlineUser.CompanyId = user.F_OrganizeId;
                    //onlineUser.DepartmentId = user.F_DepartmentId;
                    //onlineUser.RoleId = user.F_RoleId;
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
                //logBaseEntity.F_Account = username;
                //logBaseEntity.F_NickName = username;
                //logBaseEntity.F_Result = false;
                //logBaseEntity.F_Description = "登录失败，" + ex.Message;
                //new LogService().WriteDbLog(logBaseEntity);
                //return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
                throw new Exception("登录失败，" + ex.Message);
            }
        }



        public void Add(UserUpdateDto dto)
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


        public void Update(UserUpdateDto dto)
        {
            var user = DataDbContext.Set<User>().FirstOrDefault(m => m.Id == dto.UpdateId);
            if (user == null)
                throw new Exception($"错误：指定Id {dto.UpdateId} 的用户不存在！");

            ValidateUpdateDto(dto);
            dto.MapTo<User>(user);
            user.LastModifyTime = DateTime.Now;

            DataDbContext.SaveChanges();
        }


        private static void ValidateUpdateDto(UserUpdateDto dto)
        {
            if (string.IsNullOrEmpty(dto.AccountName))
                throw new Exception("错误：用户帐号不能为空！");
            if (string.IsNullOrEmpty(dto.Password))
                throw new Exception("错误：用户密码不能为空！");

            //dto.NickName = dto.NickName ?? "";
        }


        public void Remove(long id)
        {
            //var user = roleEnum.HasValue ?
            //    DbContext.Set<User>().FirstOrDefault(m => m.Id == id && (m.UserRoles.HasValue && ((m.UserRoles.Value & (long)roleEnum.Value) == (long)roleEnum.Value))) :
            //    DbContext.Set<User>().FirstOrDefault(m => m.Id == id);
            //if (user == null)
            //    throw new Exception(string.Format($"错误：指定Id {id} 的用户不存在！"));

            //DbContext.Remove(user).SaveChanges();
        }


        public static string Encrypt(string pwd)
        {
            if (string.IsNullOrWhiteSpace(pwd))
                return string.Empty;

            return Md5.md5(DesEncrypt.Encrypt(Md5.md5(pwd, 32), Settings.UserSecretkey).ToLower(), 32).ToLower();
        }
    }
}
