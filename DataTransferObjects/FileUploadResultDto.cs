using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class FileUploadResultDto
    {

        public string OriginalFileName { get; set; }

        public string FileName { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
