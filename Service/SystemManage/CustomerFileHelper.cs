using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Config;
using DataTransferObjects;
using NPOI.SS.UserModel;


namespace Service.SystemManage
{

    public class CustomerFileHelper
    {

        public static int MaxFileSize { get; set; } = 1024*500;


        /// <summary>
        ///     从文件导入资料到数据库,必须和模板上的列一致
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static List<CustomerImportDto> DoImport(ImportedFileDto dto)
        {
            var importDtos = new List<CustomerImportDto>();

            if (string.IsNullOrWhiteSpace(dto.FileName))
                return importDtos;

            var fileFullName = Path.Combine(Settings.ImportCustomerFilesPathFullname, dto.FileName);
            using (var fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var workbook = WorkbookFactory.Create(fs);

                if (workbook != null)
                {
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        var rowCount = sheet.LastRowNum; //总行数  
                        if (rowCount > 0)
                        {
                            var firstRow = sheet.GetRow(0);
                            int cellCount = firstRow.LastCellNum; //列数  

                            for (var r = 1; r < rowCount; r++)
                            {
                                var row = sheet.GetRow(r);
                                var importDto = new CustomerImportDto();
                                var cell = row.GetCell(0); //分类
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.CustomerCategoryName = str;
                                }
                                cell = row.GetCell(1); //姓名
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (string.IsNullOrWhiteSpace(str))
                                        continue;
                                    importDto.RealName = str;
                                }
                                cell = row.GetCell(2); //性别
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.GenderDescription = str;
                                }
                                cell = row.GetCell(3); //昵称
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.NickName = str;
                                }

                                cell = row.GetCell(4); //出生年月
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.BirthdayDescription = str;
                                }

                                cell = row.GetCell(5); //手机
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.MobilePhoneNumber = str;
                                }

                                cell = row.GetCell(6); //QQ
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.Qq = str;
                                }

                                cell = row.GetCell(7); //微信
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.Wechat = str;
                                }

                                cell = row.GetCell(8); //联系地址
                                if (cell != null)
                                {
                                    var str = cell.ToString();
                                    if (!string.IsNullOrWhiteSpace(str))
                                        importDto.Address = str;
                                }
                                importDtos.Add(importDto);
                            }
                        }
                    }
                }
            }
            return importDtos;
        }


        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="excleFile"></param>
        /// <returns></returns>
        public static FileUploadResultDto Upload(HttpPostedFileBase excleFile)
        {
            var fileName = excleFile.FileName;
            var extension = Path.GetExtension(fileName);

            var allowFileTypes = Settings.AcceptImportExcelFileTypes.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            if (allowFileTypes.All(type => !type.EndsWith(extension)))
                return new FileUploadResultDto {Success = false, OriginalFileName = fileName, Message = $"上传文件类型必须为：{Settings.AcceptImportExcelFileTypes}"};

            var fileSize = excleFile.ContentLength;
            if (fileSize > MaxFileSize)
                return new FileUploadResultDto {Success = false, OriginalFileName = fileName, Message = $"上传文件超过了{MaxFileSize/1024}M！"};


            if (!Directory.Exists(Settings.ImportCustomerFilesPathFullname))
                Directory.CreateDirectory(Settings.ImportCustomerFilesPathFullname);

            var newFileName = string.Format("customer_{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfffffff"), extension); //生成文件名

            excleFile.SaveAs(Path.Combine(Settings.ImportCustomerFilesPathFullname, newFileName));

            return new FileUploadResultDto {Success = true, OriginalFileName = fileName, FileName = newFileName, Message = "上传文件成功"};
        }

    }

}