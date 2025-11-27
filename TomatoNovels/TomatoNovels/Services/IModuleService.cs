using System.Collections.Generic;
using System.Threading.Tasks;
using TomatoNovels.Shared.DTOs.Module.Response;

namespace TomatoNovels.Services
{
    /// <summary>
    /// 模块内容服务接口（Banner / 公告 / 活动等）
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// 获取首页 Banner 列表
        /// </summary>
        /// <param name="limit">最多返回多少条</param>
        Task<List<BannerListResponseDto>> GetBannerListAsync(int limit);
    }
}
