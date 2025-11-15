using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Controllers.Exceptions;
using TomatoNovels.Shared.DTOs.Workspace;
using TomatoNovels.Services;

namespace TomatoNovels.Controllers
{
    /// <summary>
    /// 作家工作台相关接口（对应 Flask 的 workspace_bp）
    /// url 前缀建议在 Program.cs 中配置为 /workspace
    /// </summary>
    [Route("api/workspace")]
    public class WorkspaceController : ApiControllerBase
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        #region 作者申请 / 作家信息

        /// <summary>
        /// 作家申请（头像+表单）
        /// POST /workspace/apply
        /// </summary>
        [HttpPost("apply")]
        public async Task<ActionResult<ApiResponse<AuthorApplyResultDto>>> AuthorApply(
            [FromForm] AuthorApplyFormDto form,
            IFormFile? avatar // 对应 request.files.get("avatar")
        )
        {
            var result = await _workspaceService.SaveAuthorApplicationAsync(
                userId: form.Id,
                name: form.Name,
                intro: form.Introduction,
                avatarFile: avatar,
                fallbackAvatar: form.Avatar
            );

            return Success(result);
        }

        /// <summary>
        /// 作家统计信息
        /// GET /workspace/writer/stats/{userId}
        /// </summary>
        [HttpGet("writer/stats/{userId:int}")]
        public async Task<ActionResult<ApiResponse<AuthorStatsDto>>> GetWriterStats(int userId)
        {
            var data = await _workspaceService.GetAuthorStatsAsync(userId);
            return Success(data);
        }

        #endregion

        #region 作家首页：公告 / 资讯 / 榜单

        /// <summary>
        /// 作家公告列表
        /// GET /workspace/writer/notice-list?limit=3
        /// </summary>
        [HttpGet("writer/notice-list")]
        public async Task<ActionResult<ApiResponse<List<NoticeItemDto>>>> GetNoticeList(
            [FromQuery] int limit = 3)
        {
            var data = await _workspaceService.GetNoticeListAsync(limit);
            return Success(data);
        }

        /// <summary>
        /// 作家中心资讯列表
        /// GET /workspace/writer/news-list?limit=4
        /// </summary>
        [HttpGet("writer/news-list")]
        public async Task<ActionResult<ApiResponse<List<NewsListItemDto>>>> GetNewsList(
            [FromQuery] int limit = 4)
        {
            var data = await _workspaceService.GetNewsListAsync(limit);
            return Success(data);
        }

        /// <summary>
        /// 作家榜单
        /// GET /workspace/writer/book-rank?type=男生&category=西方奇幻
        /// </summary>
        [HttpGet("writer/book-rank")]
        public async Task<ActionResult<ApiResponse<BookRankResponseDto>>> GetBookRank(
            [FromQuery(Name = "type")] string readerType = "",
            [FromQuery(Name = "category")] string category = ""
        )
        {
            var data = await _workspaceService.GetBookRankDataAsync(readerType, category);
            return Success(data);
        }

        #endregion

        #region 作品创建 / 列表 / 详情 / 删除 / 修改

        /// <summary>
        /// 创建作品
        /// POST /workspace/writer/create-book
        /// </summary>
        [HttpPost("writer/create-book")]
        public async Task<ActionResult<ApiResponse<string>>> CreateBook(
            [FromForm] BookCreateFormDto form,
            IFormFile? cover // request.files["cover"]
        )
        {
            await _workspaceService.SaveBookAsync(form, cover);
            return Success("创建成功");
        }

        /// <summary>
        /// 我的作品列表
        /// GET /workspace/writer/my-book-list?user_id=123
        /// </summary>
        [HttpGet("writer/my-book-list")]
        public async Task<ActionResult<ApiResponse<List<BookListItemDto>>>> GetMyBookList(
            [FromQuery] MyBookListQueryDto query
        )
        {
            var data = await _workspaceService.GetBookListByUserAsync(query.UserId);
            return Success(data);
        }

        /// <summary>
        /// 作家端-作品概览详情
        /// GET /workspace/writer/book-overview/{bookId}
        /// </summary>
        [HttpGet("writer/book-overview/{bookId:int}")]
        public async Task<ActionResult<ApiResponse<BookDetailDto>>> GetBookOverview(int bookId)
        {
            var data = await _workspaceService.GetBookDetailAsync(bookId);
            return Success(data);
        }

        /// <summary>
        /// 删除作品
        /// DELETE /workspace/writer/delete-book/{bookId}
        /// </summary>
        [HttpDelete("writer/delete-book/{bookId:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteBook(int bookId)
        {
            await _workspaceService.DeleteBookByIdAsync(bookId);
            return Success();
        }

        /// <summary>
        /// 更新作品信息
        /// POST /workspace/writer/update-book
        /// </summary>
        [HttpPost("writer/update-book")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateBook(
            [FromForm] BookUpdateFormDto form,
            IFormFile? cover
        )
        {
            await _workspaceService.UpdateBookInfoAsync(form, cover);
            return Success("修改成功");
        }

        #endregion

        #region 章节：最近章节信息 + 创建

        /// <summary>
        /// 获取某书最近一次章节信息（卷号/章号）
        /// GET /workspace/writer/get-last-chapterInfo?book_id=1
        /// </summary>
        [HttpGet("writer/get-last-chapterInfo")]
        public async Task<ActionResult<ApiResponse<ChapterInfoDto>>> GetLastChapterInfo(
            [FromQuery(Name = "book_id")] int bookId
        )
        {
            var data = await _workspaceService.GetLastChapterInfoAsync(bookId);
            return Success(data);
        }

        /// <summary>
        /// 创建章节
        /// POST /workspace/writer/create-chapter
        /// </summary>
        [HttpPost("writer/create-chapter")]
        public async Task<ActionResult<ApiResponse>> CreateChapter(
            [FromBody] ChapterCreateDto dto
        )
        {
            await _workspaceService.CreateChapterAsync(dto);
            return Success();
        }

        #endregion

        #region 章节列表 / 删除 / 修改 / 详情

        /// <summary>
        /// 章节列表（带分卷）
        /// GET /workspace/writer/chapter-list?book_id=&title=&volume_id=&status=
        /// </summary>
        [HttpGet("writer/chapter-list")]
        public async Task<ActionResult<ApiResponse<ChapterListResponseDto>>> GetChapterList(
            [FromQuery(Name = "book_id")] int bookId,
            [FromQuery(Name = "title")] string title = "",
            [FromQuery(Name = "volume_id")] string volumeId = "",
            [FromQuery(Name = "status")] string status = ""
        )
        {
            var data = await _workspaceService.GetChapterListByBookIdAsync(
                bookId, title, volumeId, status
            );
            return Success(data);
        }

        /// <summary>
        /// 删除章节
        /// DELETE /workspace/writer/delete-chapter/{chapterId}
        /// </summary>
        [HttpDelete("writer/delete-chapter/{chapterId:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteChapter(int chapterId)
        {
            var success = await _workspaceService.DeleteChapterByIdAsync(chapterId);
            if (!success)
            {
                throw new ApiException("删除失败", 40004);
            }

            return Success();
        }

        /// <summary>
        /// 更新章节
        /// POST /workspace/writer/update-chapter
        /// </summary>
        [HttpPost("writer/update-chapter")]
        public async Task<ActionResult<ApiResponse>> UpdateChapter(
            [FromBody] ChapterUpdateDto dto
        )
        {
            await _workspaceService.UpdateChapterAsync(dto);
            return Success();
        }

        /// <summary>
        /// 章节详情
        /// GET /workspace/writer/chapter-detail?book_id=&chapter_id=
        /// </summary>
        [HttpGet("writer/chapter-detail")]
        public async Task<ActionResult<ApiResponse<ChapterDetailDto>>> GetChapterDetail(
            [FromQuery(Name = "book_id")] int bookId,
            [FromQuery(Name = "chapter_id")] int chapterId
        )
        {
            var data = await _workspaceService.GetChapterDetailByIdAsync(bookId, chapterId);
            return Success(data);
        }

        #endregion

        #region 分卷：删除 / 更新标题 / 创建

        /// <summary>
        /// 删除分卷（及其章节）
        /// DELETE /workspace/writer/delete-volume?book_id=&volume_id=
        /// </summary>
        [HttpDelete("writer/delete-volume")]
        public async Task<ActionResult<ApiResponse>> DeleteVolume(
            [FromQuery(Name = "book_id")] int bookId,
            [FromQuery(Name = "volume_id")] int volumeId
        )
        {
            var success = await _workspaceService.DeleteVolumeWithChaptersAsync(bookId, volumeId);
            if (!success)
            {
                throw new ApiException("删除失败", 40004);
            }

            return Success();
        }

        /// <summary>
        /// 更新分卷标题
        /// POST /workspace/writer/update-volume
        /// body: { id, book_id, title }
        /// </summary>
        [HttpPost("writer/update-volume")]
        public async Task<ActionResult<ApiResponse>> UpdateVolumeTitle([FromBody] VolumeUpdateDto dto)
        {
            var success = await _workspaceService.UpdateVolumeTitleAsync(
                dto.Id,
                dto.BookId,
                dto.Title
            );

            if (!success)
            {
                throw new ApiException("更新失败", 40004);
            }

            return Success();
        }

        /// <summary>
        /// 创建分卷
        /// POST /workspace/writer/create-volume
        /// body: { book_id, title, sort }
        /// </summary>
        [HttpPost("writer/create-volume")]
        public async Task<ActionResult<ApiResponse<int>>> CreateVolume(
            [FromBody] VolumeCreateDto dto
        )
        {
            var newId = await _workspaceService.CreateVolumeAsync(dto.BookId, dto.Title, dto.Sort);
            return Success(newId);
        }

        #endregion

        #region last-chapter / latest-chapter

        /// <summary>
        /// 获取某书的“最后一卷最后一章”信息
        /// GET /workspace/writer/last-chapter?book_id=
        /// </summary>
        [HttpGet("writer/last-chapter")]
        public async Task<ActionResult<ApiResponse<LastChapterResponseDto?>>> GetLastChapterByBook(
            [FromQuery(Name = "book_id")] int bookId
        )
        {
            var data = await _workspaceService.GetLastChapterByBookIdAsync(bookId);
            // Python 里是“无数据返回空 dict”，这里用 null 表示
            return Success(data);
        }

        /// <summary>
        /// 获取某书指定分卷 + 整本书的最新进度
        /// GET /workspace/writer/last-chapter-by-volume?book_id=&volume_id=
        /// </summary>
        [HttpGet("writer/last-chapter-by-volume")]
        public async Task<ActionResult<ApiResponse<LastChapterResponseDto?>>> GetLastChapterByVolume(
            [FromQuery(Name = "book_id")] int bookId,
            [FromQuery(Name = "volume_id")] int volumeId
        )
        {
            var data = await _workspaceService.GetLastChapterByVolumeIdAsync(bookId, volumeId);
            return Success(data);
        }

        /// <summary>
        /// 获取整本书“最新章节”的信息
        /// GET /workspace/writer/latest-chapter?book_id=
        /// </summary>
        [HttpGet("writer/latest-chapter")]
        public async Task<ActionResult<ApiResponse<LatestChapterResponseDto?>>> GetLatestChapterByBook(
            [FromQuery(Name = "book_id")] int bookId
        )
        {
            var data = await _workspaceService.GetLatestChapterByBookIdAsync(bookId);
            return Success(data);
        }

        #endregion
    }

    /// <summary>
    /// 更新分卷标题请求体（Flask 中是自由 dict，这里单独定义）
    /// </summary>
    public class VolumeUpdateDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
    }

    /// <summary>
    /// 创建分卷请求体（Flask 中是自由 dict，这里单独定义）
    /// </summary>
    public class VolumeCreateDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("sort")]
        public int Sort { get; set; }
    }
}
