﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class CustomerCategorySearchDto:BaseSearchDto
    {
        public DateTime? StartCreatorTime { get; set; }

        public DateTime? EndCreatorTime { get; set; }
    }
}
