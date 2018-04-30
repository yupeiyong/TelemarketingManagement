using Common;
using Models.DataBase;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : BaseEntity
    {
        public string NickName { get; set; }

        public string Sex => Gender.GetEnumDescription();

        public Gender Gender { get; set; }


        /// <summary>
        ///     手机电话号码
        /// </summary>
        public string MobilePhoneNumber { get; set; }


        /// <summary>
        ///     QQ
        /// </summary>
        public string Qq { get; set; }

        /// <summary>
        ///     微信
        /// </summary>
        public string Wechat { get; set; }




        public string RealName { get; set; }


        public int Age
        {
            get
            {
                if (!Birthday.HasValue)
                    return -1;

                var now = DateTime.Now;
                var birthday = Birthday.Value;
                var years = now.Year - birthday.Year;
                if (birthday.Month > now.Month || (birthday.Month == now.Month && birthday.Day > now.Day))
                    years++;
                return years;
            }
        }

        public DateTime? Birthday { get; set; }


        [Index]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool IsEncrypted { get; set; } = false;

        [Index]
        [MaxLength(200)]
        public string AccountName { get; set; }

        [Index]
        public int ErrorTimes { get; set; }

        [Index]
        public DateTime? LastErrorDateTime { get; set; }



        [Index]
        public long? UserRoles { get; set; }

        public int LogOnCount { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStateEnum UserState { get; set; }
    }
}
