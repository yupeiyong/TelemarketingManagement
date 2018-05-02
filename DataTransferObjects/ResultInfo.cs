using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class ResultInfo<TModel>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public int Total { get; set; }

        public List<TModel> Data { get; set; }
    }
}
