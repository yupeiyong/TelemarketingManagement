using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerSearchDto : BaseSearchDto
    {
        public DateTime? StartCreatorTime { get; set; }

        public DateTime? EndCreatorTime { get; set; }

        public long CustomerCategoryId { get; set; }

        public Gender? Gender { get; set; }
    }
}
