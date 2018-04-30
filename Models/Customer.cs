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

        public string Sex => this.Gender == null ? string.Empty : this.Gender.Value.GetEnumDescription();

        public Gender? Gender { get; set; }

        public virtual CustomerCategory CustomerCategory { get; set; }
        /// <summary>
        ///     手机电话号码：用于绑定、找回密码等
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

        public string Address { get; set; }
        //[Index]
        //[MaxLength(100)]
        //public string Password { get; set; }


        //[Index]
        //[MaxLength(200)]
        //public string AccountName { get; set; }


    }

}