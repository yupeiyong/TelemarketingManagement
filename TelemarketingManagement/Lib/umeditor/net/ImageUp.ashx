<%@ WebHandler Language="C#" Class="JJsites.WebPc.Lib.umeditor.net.ImageUp" %>

using System.Collections.Generic;
using System.Text;
using System.Web;
using JJsites.Configs;


namespace JJsites.WebPc.Lib.umeditor.net
{

    public class ImageUp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;

            //上传配置
            var pathbase = AppSettings.UploadedFilesRootPathFullname; //保存路径
            var size = 30; //文件大小限制,单位mb
            string[] filetype = {".gif", ".png", ".jpg", ".jpeg"}; //文件允许格式

            var callback = context.Request["callback"];

            //string editorId = context.Request["editorid"];

            //上传图片
            var up = new Uploader();
            var info = up.upFile(context, pathbase, filetype, size, AppSettings.UploadedSubFolderMaxCount); //获取上传状态
            var imageAbsoluteUrl = FilePoolHelper.GetRequestUrl(info["name"], 800, 800);

            info.Add("url", imageAbsoluteUrl);
            var json = BuildJson(info);
            context.Response.ContentType = "text/html";
            context.Response.Write(callback != null ? $"<script>{callback}(JSON.parse(\"{json}\"));</script>" : json);
        }


        public bool IsReusable
        {
            get { return false; }
        }


        private string BuildJson(Dictionary<string, string> info)
        {
            var fields = new List<string>();
            string[] keys = {"originalName", "name", "url", "size", "state", "type"};
            for (var i = 0; i < keys.Length; i++)
            {
                fields.Add($"\"{keys[i]}\": \"{info[keys[i]]}\"");
            }
            return "{" + string.Join(",", fields) + "}";
        }

    }

}
