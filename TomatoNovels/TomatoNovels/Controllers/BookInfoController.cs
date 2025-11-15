using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Controllers.Exceptions;
using TomatoNovels.Services;
using TomatoNovels.Shared.DTOs.BookInfo;

namespace TomatoNovels.Controllers
{
    [Route("api/bookinfo")]
    public class BookInfoController : ApiControllerBase
    {
        private readonly IBookInfoService _service;

        public BookInfoController(IBookInfoService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取书籍头部信息（标题、作者、状态等）
        /// 对应 Flask：/header/<book_id>
        /// </summary>
        [HttpGet("header/{bookId:int}")]
        public async Task<ActionResult<ApiResponse<BookHeaderDto>>> GetHeader(int bookId)
        {
            var result = await _service.GetBookHeaderAsync(bookId);

            // DTO 会自动校验类型（相当于 Pydantic）
            return Success(result);
        }

        /// <summary>
        /// 获取整本书的基础信息
        /// Flask：/content/<book_id>
        /// </summary>
        [HttpGet("content/{bookId:int}")]
        public async Task<ActionResult<ApiResponse<BookContentDto>>> GetContent(int bookId)
        {
            var result = await _service.GetBookContentAsync(bookId);
            return Success(result);
        }

        /// <summary>
        /// 获取某一章内容
        /// Flask：/chapter?bookId=&volumeId=&chapterId=
        /// </summary>
        [HttpGet("chapter")]
        public async Task<ActionResult<ApiResponse<ChapterReadResponseDto>>> GetChapter(
            [FromQuery] int bookId,
            [FromQuery] int volumeId,
            [FromQuery] int chapterId)
        {
            var data = await _service.GetChapterContentAsync(bookId, volumeId, chapterId);
            return Success(data);
        }
    }
}
