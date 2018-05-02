using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerEditDto
    {
        public long UpdateId { get; set; }

        public long CustomerCategoryId { get; set; }

        public string NickName { get; set; }


        public Gender? Gender { get; set; }


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


        public DateTime? Birthday { get; set; }

        public string Address { get; set; }


    }
}
