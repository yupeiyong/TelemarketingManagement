using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerSearchDto : BaseSearchDto
    {
        public DateTime? StartLastModifyTime { get; set; }

        public DateTime? EndLastModifyTime { get; set; }
    }
}
