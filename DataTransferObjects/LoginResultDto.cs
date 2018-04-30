using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class LoginResultDto : BaseResponseDto
    {
        public string NickName { get; set; }

        public string RedirectUrl { get; set; }
    }
}
