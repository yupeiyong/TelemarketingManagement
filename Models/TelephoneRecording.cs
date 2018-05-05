using Models.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Description("电话录音")]
    public class TelephoneRecording : BaseEntity
    {
        public long CustomerId { get; set; }

        public string CustomerRealName { get; set; }

        public long VisitorId { get; set; }

        public string VisitorNickName { get; set; }

        public string AudioFileName { get; set; }


        public string Description { get; set; }

        public string CreatorTimeStr => this.CreatorTime.HasValue ? CreatorTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;
        public string LastModifyTimeStr => this.LastModifyTime.HasValue ? LastModifyTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty;
    }
}
