using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class TelephoneRecordingSearchDto:BaseSearchDto
    {

        public DateTime? StartCreatorTime { get; set; }

        public DateTime? EndCreatorTime { get; set; }

        public long CustomerId { get; set; }

        public long VisitorId { get; set; }


    }
}
