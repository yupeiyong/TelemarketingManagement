using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class BaseResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }
    }
}
