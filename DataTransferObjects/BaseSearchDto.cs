using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class BaseSearchDto
    {

        /// <summary>
        ///     开始位置
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        ///     每一页数量
        /// </summary>
        public int PageSize { get; set; } = 20;


        /// <summary>
        ///     总条数
        /// </summary>
        public int TotalCount { get; set; }


        /// <summary>
        ///     是否获取总条数
        /// </summary>
        public bool IsGetTotalCount { get; set; } = true;


        /// <summary>
        ///     总页数
        /// </summary>
        public int PageCount => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

        public string Keywords { get; set; }

        public string sort { get; set; }

        public string order { get; set; }

        public int offset
        {
            get { return StartIndex; }
            set { StartIndex = value; }
        }

        public int limit
        {
            get { return PageSize; }
            set { PageSize = value; }
        }

    }

}
