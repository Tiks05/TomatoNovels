using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TomatoNovels.Shared.DTOs.Workspace;

namespace TomatoNovels.Services
{
    /// <summary>
    /// 作家工作台业务接口（对应 workspace_service.py）
    /// </summary>
    public interface IWorkspaceService
    {
        #region 作者申请 / 作家信息

        /// <summary>
        /// 保存作者申请信息（头像上传 + 用户信息更新）
        /// </summary>
        Task<AuthorApplyResultDto> SaveAuthorApplicationAsync(
            int userId,
            string name,
            string intro,
            IFormFile? avatarFile,
            string fallbackAvatar
        );

        /// <summary>
        /// 获取作家统计信息（粉丝数、总字数）
        /// </summary>
        Task<AuthorStatsDto> GetAuthorStatsAsync(int writerId);

        #endregion

        #region 作家首页：公告 / 资讯 / 榜单

        /// <summary>
        /// 获取公告列表
        /// </summary>
        Task<List<NoticeItemDto>> GetNoticeListAsync(int limit);

        /// <summary>
        /// 获取作家新闻列表
        /// </summary>
        Task<List<NewsListItemDto>> GetNewsListAsync(int limit);

        /// <summary>
        /// 获取作品榜单（按读者类型 + 分类）
        /// </summary>
        Task<BookRankResponseDto> GetBookRankDataAsync(string readerType, string category);

        #endregion

        #region 作品创建 / 列表 / 详情 / 删除 / 修改

        /// <summary>
        /// 创建作品
        /// </summary>
        Task SaveBookAsync(BookCreateFormDto form, IFormFile? coverFile);

        /// <summary>
        /// 获取我的作品列表
        /// </summary>
        Task<List<BookListItemDto>> GetBookListByUserAsync(int userId);

        /// <summary>
        /// 获取作家端作品详情
        /// </summary>
        Task<BookDetailDto> GetBookDetailAsync(int bookId);

        /// <summary>
        /// 根据 ID 删除作品（含分卷、章节）
        /// </summary>
        Task DeleteBookByIdAsync(int bookId);

        /// <summary>
        /// 更新作品信息
        /// </summary>
        Task UpdateBookInfoAsync(BookUpdateFormDto form, IFormFile? coverFile);

        #endregion

        #region 章节：最近章节信息 + 创建

        /// <summary>
        /// 获取某书最近一次章节信息（卷号 / 章号）
        /// </summary>
        Task<ChapterInfoDto> GetLastChapterInfoAsync(int bookId);

        /// <summary>
        /// 创建章节
        /// </summary>
        Task CreateChapterAsync(ChapterCreateDto dto);

        #endregion

        #region 章节列表 / 删除 / 修改 / 详情

        /// <summary>
        /// 获取章节列表（带分卷）
        /// </summary>
        Task<ChapterListResponseDto> GetChapterListByBookIdAsync(
            int bookId,
            string title,
            string volumeId,
            string status
        );

        /// <summary>
        /// 根据章节 ID 删除章节
        /// </summary>
        Task<bool> DeleteChapterByIdAsync(int chapterId);

        /// <summary>
        /// 更新章节
        /// </summary>
        Task UpdateChapterAsync(ChapterUpdateDto dto);

        /// <summary>
        /// 获取章节详情（校验是否属于该书）
        /// </summary>
        Task<ChapterDetailDto> GetChapterDetailByIdAsync(int bookId, int chapterId);

        #endregion

        #region 分卷：删除 / 更新标题 / 创建

        /// <summary>
        /// 删除分卷及其所有章节
        /// </summary>
        Task<bool> DeleteVolumeWithChaptersAsync(int bookId, int volumeId);

        /// <summary>
        /// 更新分卷标题
        /// </summary>
        Task<bool> UpdateVolumeTitleAsync(int volumeId, int bookId, string newTitle);

        /// <summary>
        /// 创建分卷，返回新分卷的 ID
        /// </summary>
        Task<int> CreateVolumeAsync(int bookId, string title, int sort);

        #endregion

        #region last-chapter / latest-chapter

        /// <summary>
        /// 获取某书最后一卷最后一章的信息
        /// </summary>
        Task<LastChapterResponseDto?> GetLastChapterByBookIdAsync(int bookId);

        /// <summary>
        /// 获取指定分卷 + 整本书的最新进度
        /// </summary>
        Task<LastChapterResponseDto?> GetLastChapterByVolumeIdAsync(int bookId, int volumeId);

        /// <summary>
        /// 获取整本书的最新章节信息
        /// </summary>
        Task<LatestChapterResponseDto?> GetLatestChapterByBookIdAsync(int bookId);

        #endregion
    }
}
