using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ImportedFileDto 
    {

        public long Id { get; set; }

        public string OriginalFileName { get; set; }

        public string FileName { get; set; }

    }
}
