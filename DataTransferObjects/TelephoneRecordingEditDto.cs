using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class TelephoneRecordingEditDto
    {
        public long UpdateId { get; set; }
        public long CustomerId { get; set; }

        public long VisitorId { get; set; }

        public string AudioFileName { get; set; }

        public string Description { get; set; }

    }
}
