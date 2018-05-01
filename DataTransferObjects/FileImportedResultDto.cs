using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class FileImportedResultDto
    {
        /*
          Title = "上传文件",
                Message = $"提示：成功收到{dtoes.Count}个文件！",
                Files = dtoes.Select(i => i.RawFilename).ToList(),
                OriginalFiles = dtoes.Select(i => i.OriginalFilename).ToList(),
                Success = true
         
         */

        public string Title { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }
    }

}
