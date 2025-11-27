using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Services;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Module.Request;
using TomatoNovels.Shared.DTOs.Module.Response;

namespace TomatoNovels.Controllers
{
    [Route("api/module")]
    public class ModuleController : ApiControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpGet("banner-list")]
        public async Task<ActionResult<ApiResponse<List<BannerListResponseDto>>>> BannerList(
            [FromQuery] BannerListRequestDto req)
        {
            var list = await _moduleService.GetBannerListAsync(req.Limit);
            return Success(list);
        }
    }
}
