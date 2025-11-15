using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Writer;
using TomatoNovels.Models;

namespace TomatoNovels.Services.Impl
{
    /// <summary>
    /// 作家专区服务实现
    /// 对应原 Flask 的 writer_service.py
    /// </summary>
    public class WriterService : IWriterService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WriterService(
            AppDbContext db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 公共方法

        public async Task<object> GetNewsListByTypeAsync(string newsType, int limit = 5)
        {
            // EF Core 查询 News
            var query = _db.Set<News>()
                .Where(n => n.Type == newsType)
                .OrderByDescending(n => n.UpdatedAt)
                .Take(limit);

            var host = GetHostUrl();

            if (newsType == "picnotice")
            {
                var list = await query
                    .Select(item => new PicNoticeDto
                    {
                        CoverUrl = string.IsNullOrEmpty(item.CoverUrl)
                            ? string.Empty
                            : $"{host}{item.CoverUrl}",
                        Title = item.Title,
                        Path = $"/newsinfo/{item.Id}"
                    })
                    .ToListAsync();

                return list;
            }
            else if (newsType == "notice")
            {
                var list = await query
                    .Select(item => new NoticeDto
                    {
                        Title = item.Title,
                        Path = $"/newsinfo/{item.Id}"
                    })
                    .ToListAsync();

                return list;
            }
            else if (newsType == "active")
            {
                var list = await query
                    .Select(item => new ActiveDto
                    {
                        CoverUrl = string.IsNullOrEmpty(item.CoverUrl)
                            ? string.Empty
                            : $"{host}{item.CoverUrl}",
                        Title = item.Title,
                        Path = $"/newsinfo/{item.Id}",
                        UpdatedAt = item.UpdatedAt.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                return list;
            }

            // 理论上不会走到这里（Controller 已经把 type 规范成 notice/active）
            return Array.Empty<object>();
        }

        public async Task<List<ClassroomOutDto>> GetClassroomByCategoryAsync(string categoryType)
        {
            var host = GetHostUrl();

            var query = _db.Set<Classroom>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoryType))
            {
                query = query.Where(c => c.CategoryType == categoryType);
            }

            query = query
                .OrderByDescending(c => c.CreateAt)
                .Take(10);

            var list = await query
                .Select(item => new ClassroomOutDto
                {
                    Title = item.Title,
                    Intro = item.Intro,
                    CoverUrl = string.IsNullOrEmpty(item.CoverUrl)
                        ? string.Empty
                        : $"{host}{item.CoverUrl}",
                    Path = $"/classroom/{item.Id}",
                    IsIncludeVideo = item.IsIncludeVideo
                })
                .ToListAsync();

            return list;
        }

        #endregion

        #region 私有工具方法

        /// <summary>
        /// 获取当前请求的 HostUrl，例如：
        ///   https://localhost:7014
        /// 末尾不带斜杠
        /// </summary>
        private string GetHostUrl()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return string.Empty;
            }

            var request = httpContext.Request;
            var host = $"{request.Scheme}://{request.Host}".TrimEnd('/');
            return host;
        }

        #endregion
    }
}
