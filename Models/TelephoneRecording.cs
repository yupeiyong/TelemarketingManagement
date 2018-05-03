using Models.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Description("电话采访记录")]
    public class TelephoneRecording : BaseEntity
    {
        public long CustomerId { get; set; }

        public string CustomerRealName { get; set; }

        public long VisitorId { get; set; }

        public string VisitorNickName { get; set; }

        public string AudioFileName { get; set; }
    }
}
