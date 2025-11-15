using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TomatoNovels.Utils
{
    public static class ImageUtils
    {
        /// <summary>
        /// 保存上传的图片文件，返回 (相对路径, 前端可访问 URL)
        /// </summary>
        /// <param name="file">上传的图片文件</param>
        /// <param name="subFolder">子目录（例如："user/avatars"）</param>
        /// <param name="uploadRoot">上传根路径（默认 static/uploads）</param>
        /// <param name="http">用于获取 host 和 scheme</param>
        public static async Task<(string RelativePath, string Url)> SaveUploadedImageAsync(
            IFormFile file,
            string subFolder,
            IHttpContextAccessor http,
            string uploadRoot = "uploads"
        )
        {
            if (file == null || file.Length == 0)
                throw new Exception("上传文件无效");

            // 获取扩展名
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid().ToString("N")}{ext}";

            // 例如： /static/uploads/user/avatars/xxxxxx.png
            var relativePath = $"/{uploadRoot}/{subFolder}/{fileName}";

            // 确定保存路径： wwwroot/static/uploads/...
            var savePath = Path.Combine("wwwroot", relativePath.TrimStart('/'));

            // 如果目录不存在，创建
            var dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            // 保存文件
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 构造绝对访问 URL，和 Flask 一模一样
            var request = http.HttpContext!.Request;
            var scheme = request.Scheme;               // http or https
            var host = request.Host.Value;             // localhost:5000
            var url = $"{scheme}://{host}{relativePath}";

            return (relativePath, url);
        }
    }
}
