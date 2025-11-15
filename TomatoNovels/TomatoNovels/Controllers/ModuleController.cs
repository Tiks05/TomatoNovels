using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Services;
using TomatoNovels.Shared.DTOs.Module;

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
        public async Task<ActionResult<ApiResponse<List<BannerItemDto>>>> BannerList([FromQuery] int limit = 5)
        {
            var result = await _moduleService.GetBannerListAsync(limit);
            return Success(result);
        }
    }
}
