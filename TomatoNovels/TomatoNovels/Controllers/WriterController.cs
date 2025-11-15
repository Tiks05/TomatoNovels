using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Writer;
using TomatoNovels.Services;

namespace TomatoNovels.Controllers
{
    /// <summary>
    /// 作家专区相关接口（公告 / 活动 / 教室）
    /// 对应原 Flask 的 writer_bp
    /// </summary>
    [Route("api/writer")]
    public class WriterController : ApiControllerBase
    {
        private readonly IWriterService _writerService;

        public WriterController(IWriterService writerService)
        {
            _writerService = writerService;
        }

        /// <summary>
        /// 获取公告 / 活动 列表
        /// GET /writer/news?type=notice|active&limit=5
        ///
        /// Flask 逻辑：
        ///   news_type = request.args.get('type', 'normal')
        ///   if news_type != 'notice': news_type = 'active'
        /// </summary>
        [HttpGet("news")]
        public async Task<ActionResult<ApiResponse<object>>> GetNewsList(
            [FromQuery(Name = "type")] string type = "normal",
            [FromQuery(Name = "limit")] int limit = 5)
        {
            // 和原来保持一致：只有 type=notice 时返回 notice，其它都归为 active
            if (type != "notice")
            {
                type = "active";
            }

            // 这里返回值类型用 object，
            // 实际内部可以是 List<NoticeDto> 或 List<ActiveDto>，
            // 这样 JSON 结构可以和之前保持一致
            var data = await _writerService.GetNewsListByTypeAsync(type, limit);

            return Success<object>(data);
        }

        /// <summary>
        /// 获取创作课堂列表
        /// GET /writer/classroom?category_type=xxx
        /// </summary>
        [HttpGet("classroom")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClassroomOutDto>>>> GetClassroomList(
            [FromQuery(Name = "category_type")] string? categoryType = "")
        {
            categoryType = categoryType?.Trim() ?? string.Empty;

            var data = await _writerService.GetClassroomByCategoryAsync(categoryType);

            // 这里是强类型 List<ClassroomOutDto>
            return Success<IEnumerable<ClassroomOutDto>>(data);
        }
    }
}
