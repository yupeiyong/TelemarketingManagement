using Models;
using Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CustomerEditViewModel
    {
        public Customer Customer { get; set; }

        public List<Gender> Genders { get; set; }
    }
}
