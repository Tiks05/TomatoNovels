using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Module.Response;

namespace TomatoNovels.Services.Impl
{
    public class ModuleService : IModuleService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public ModuleService(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        public async Task<List<BannerListResponseDto>> GetBannerListAsync(int limit)
        {
            // === 完全对应 Flask SQLAlchemy ===
            var records = await _db.News
                .Where(n => n.IsBanner == true && n.Type == "active")
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new { n.Id, n.BannerUrl })
                .Take(limit)
                .ToListAsync();

            var http = _http.HttpContext;
            var scheme = http?.Request.Scheme ?? "http";
            var host = http?.Request.Host.Value;

            var list = new List<BannerListResponseDto>();

            foreach (var r in records)
            {
                string bannerUrl = "";
                if (!string.IsNullOrWhiteSpace(r.BannerUrl) && host != null)
                {
                    // Flask: f"{request.host_url.rstrip('/')}{banner_url}"
                    bannerUrl = $"{scheme}://{host}{NormalizePath(r.BannerUrl)}";
                }

                list.Add(new BannerListResponseDto
                {
                    BannerUrl = bannerUrl,
                    Path = $"/classroom/{r.Id}"   // 完全对标 Flask
                });
            }

            return list;
        }

        private string NormalizePath(string relative)
        {
            if (string.IsNullOrWhiteSpace(relative)) return "";
            return relative.StartsWith("/") ? relative : "/" + relative;
        }
    }
}
