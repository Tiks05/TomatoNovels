using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Services;
using TomatoNovels.Shared.DTOs.Library;

namespace TomatoNovels.Controllers
{
    [Route("api/library")]
    public class LibraryController : ApiControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        /// <summary>
        /// GET /books
        /// 书库筛选接口（对应 Flask 的 get_books）
        /// </summary>
        [HttpGet("books")]
        public async Task<ActionResult<ApiResponse<BookListResultDto>>> GetBooks(
            [FromQuery] BookListQueryDto query
        )
        {
            var result = await _libraryService.GetFilteredBooksAsync(query);

            return Success(result);
        }
    }
}
