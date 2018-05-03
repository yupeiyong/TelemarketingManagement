using Config;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.SystemManage
{
    public class TelephoneRecordingFileHelper
    {
        public static int MaxFileSize { get; set; } = 1024 * 500;

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="excleFile"></param>
        /// <returns></returns>
        public static FileUploadResultDto Upload(HttpPostedFileBase excleFile)
        {
            var fileName = excleFile.FileName;
            var extension = Path.GetExtension(fileName);

            var allowFileTypes = Settings.AcceptImportExcelFileTypes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (allowFileTypes.All(type => !type.EndsWith(extension)))
                return new FileUploadResultDto { Success = false, OriginalFileName = fileName, Message = $"上传文件类型必须为：{Settings.AcceptImportExcelFileTypes}" };

            var fileSize = excleFile.ContentLength;
            if (fileSize > MaxFileSize)
                return new FileUploadResultDto { Success = false, OriginalFileName = fileName, Message = $"上传文件超过了{MaxFileSize / 1024}M！" };


            if (!Directory.Exists(Settings.ImportCustomerFilesPathFullname))
                Directory.CreateDirectory(Settings.ImportCustomerFilesPathFullname);

            var newFileName = string.Format("customer_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension); //生成文件名

            excleFile.SaveAs(Path.Combine(Settings.ImportCustomerFilesPathFullname, newFileName));

            return new FileUploadResultDto { Success = true, OriginalFileName = fileName, FileName = newFileName, Message = "上传文件成功" };
        }
    }
}
