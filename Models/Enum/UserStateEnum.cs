using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enum
{
    /// <summary>
    /// 系统用户状态
    /// </summary>
    public enum UserStateEnum
    {
        /// <summary>
        /// 待启用
        /// </summary>
        [Description("待启用")]
        Disable=0,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable =1,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Frozen =2,
    }
}
