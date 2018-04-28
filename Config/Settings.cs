using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public class Settings
    {
        public static string ConnectionString { get; set; } = "TelemarketingManagementConnection";

        public static string ProjectName { get; set; } = "营销管理系统";

        public static string Version { get; set; } = "1.0.0.0";


    }
}
