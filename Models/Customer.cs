using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
using Models.DataBase;
using Models.Enum;


namespace Models
{

    /// <summary>
    ///     客户
    /// </summary>
    [Description("客户")]
    public class Customer : BaseEntity
    {
        public string NickName { get; set; }

        public string HeadImage { get; set; }

        public string Sex => Gender.GetEnumDescription();

        public Gender Gender { get; set; }

        /// <summary>
        ///     档案号
        /// </summary>
        public string ArchivesNum { get; set; }

        /// <summary>
        ///     手机电话号码：用于绑定、找回密码等
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        ///     手机号码是否验证？
        /// </summary>
        public bool? IsMobileVerified { get; set; }

        /// <summary>
        ///     QQ
        /// </summary>
        public string Qq { get; set; }

        /// <summary>
        ///     微信
        /// </summary>
        public string Wechat { get; set; }

        public DateTime? LastSentMessageTime { get; set; }

        public int SentMessageCount { get; set; }

        /// <summary>
        ///     补充的联系人
        /// </summary>
        public string AdditionalContacts { get; set; }

        /// <summary>
        ///     补充的联系电话
        /// </summary>
        public string AdditionalPhones { get; set; }

        /// <summary>
        ///     补充资料
        /// </summary>
        public string AdditionalInformation { get; set; }

        /// <summary>
        ///     员工工号
        /// </summary>
        public string WorkNumber { get; set; }


        public string RealName { get; set; }

        public string RealNamePinyin { get; set; }


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

        public string RedirectLocationAfterWechatLogin { get; set; }


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
        public DateTime? OnLastError { get; set; }


        [Index]
        [MaxLength(200)]
        public string RedirectLocationAfterLogin { get; set; }

        [Index]
        public long? UserRoles { get; set; }

        [Index]
        public int MaxOnlineClientCount { get; set; }

    }

}