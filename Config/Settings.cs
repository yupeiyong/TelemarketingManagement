using System;
using System.Collections.Generic;
using System.IO;
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


        public static string UserSecretkey { get; set; } = "TelemarketingManagement_User_Secret_key";


        public static string AppDataPathFullname { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");


        public static string ImportCustomerFilesPathFullname => Path.Combine(AppDataPathFullname, ".ImportCustomerFiles");

        /// <summary>
        ///     允许上传Excel文件的类型
        /// </summary>
        public static string AcceptImportExcelFileTypes = ".xlsx,.xls";
    }
}
