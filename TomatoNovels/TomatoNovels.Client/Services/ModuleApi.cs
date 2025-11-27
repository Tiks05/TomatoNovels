using TomatoNovels.Client.ApiRequest;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Module.Request;
using TomatoNovels.Shared.DTOs.Module.Response;

namespace TomatoNovels.Client.Services
{
    public class ModuleApi
    {
        private readonly ApiRequest.ApiRequest _http;

        public ModuleApi(ApiRequest.ApiRequest http)
        {
            _http = http;
        }

        /// <summary>
        /// 获取 Banner 列表
        /// GET /api/module/banner-list
        /// </summary>
        public Task<ApiResponse<List<BannerListResponseDto>>> GetBannerListAsync(int limit)
        {
            return _http.GetAsync<List<BannerListResponseDto>>(
                $"module/banner-list?limit={limit}"
            );
        }
    }
}
