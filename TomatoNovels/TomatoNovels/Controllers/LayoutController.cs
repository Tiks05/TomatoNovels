using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Layout;
using TomatoNovels.Services;

namespace TomatoNovels.Controllers
{
    [Route("api/layout")]
    public class LayoutController : ApiControllerBase
    {
        private readonly ILayoutService _layoutService;

        public LayoutController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        /// <summary>
        /// 更新用户资料（含头像上传）
        /// POST form-data
        /// 字段：id, avatar, name, introduction, avatar(file)
        /// </summary>
        [HttpPost("profile/update")]
        public async Task<ActionResult<ApiResponse<UserProfileUpdateResultDto>>> UpdateProfile(
            [FromForm] UserProfileUpdateFormDto form,
            IFormFile? avatar)
        {
            var result = await _layoutService.UpdateUserProfileAsync(
                userId: form.Id,
                name: form.Name,
                introduction: form.Introduction,
                avatarFile: avatar,
                fallbackAvatar: form.Avatar
            );

            return Success(result);
        }

        /// <summary>
        /// 搜索书籍
        /// GET
        /// </summary>
        [HttpGet("search-books")]
        public async Task<ActionResult<ApiResponse<SearchBookResponseDto>>> SearchBooks(
            [FromQuery] SearchBookRequestDto request)
        {
            var result = await _layoutService.SearchBooksAsync(request);
            return Success(result);
        }
    }
}
