using Microsoft.AspNetCore.Http;
using TomatoNovels.Shared.DTOs.Layout;

namespace TomatoNovels.Services
{
    public interface ILayoutService
    {
        /// <summary>
        /// 更新用户资料（含头像上传）
        /// Python: update_user_profile(user_id, name, intro, avatar_file, fallback_avatar)
        /// </summary>
        Task<UserProfileUpdateResultDto> UpdateUserProfileAsync(
            int userId,
            string name,
            string introduction,
            IFormFile? avatarFile,
            string fallbackAvatar);

        /// <summary>
        /// 搜索书籍
        /// Python: search_books(args)
        /// 返回结构与 SearchBookResponseDto 对齐
        /// </summary>
        Task<SearchBookResponseDto> SearchBooksAsync(SearchBookRequestDto request);
    }
}
