using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerCategoryEditDto
    {
        public long UpdateId { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///     自定义顺序
        /// </summary>
        public int CustomOrder { get; set; }
    }
}
