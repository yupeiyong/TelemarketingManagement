using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Models.DataBase;
using Models.Enum;


namespace Models
{

    [Description("用户")]
    public class User : BaseEntity
    {

        public string NickName { get; set; }

        public string Sex => Gender == null ? string.Empty : Gender.Value.GetEnumDescription();

        public Gender? Gender { get; set; }


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
        ///     用户状态
        /// </summary>
        public UserStateEnum UserState { get; set; }

        public string CreatorTimeStr => this.CreatorTime.HasValue ? CreatorTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;

        public string LastModifyTimeStr => this.LastModifyTime.HasValue ? LastModifyTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;

    }

}