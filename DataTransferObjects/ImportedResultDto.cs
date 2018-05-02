using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ImportedResultDto: BaseResponseDto
    {

        /// <summary>
        /// 导入条数
        /// </summary>
        public int Count { get; set; }
    }
}
