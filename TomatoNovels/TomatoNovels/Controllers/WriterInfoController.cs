using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.WriterInfo;
using TomatoNovels.Services;

namespace TomatoNovels.Controllers
{
    /// <summary>
    /// 作者详情相关接口（头像 / 签名 / 作品列表）
    /// 对应原 Flask 的 writerinfo_bp
    /// </summary>
    [Route("api/writerinfo")]
    public class WriterInfoController : ApiControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;

        public WriterInfoController(IWriterInfoService writerInfoService)
        {
            _writerInfoService = writerInfoService;
        }

        /// <summary>
        /// 获取作者头部信息
        /// GET /writerinfo/header/{writerId}
        ///
        /// 对应：
        ///   get_writer_header(writer_id)
        ///   Result.success(WriterHeaderData(writer=data).dict())
        /// </summary>
        [HttpGet("header/{writerId:int}")]
        public async Task<ActionResult<ApiResponse<WriterHeaderDataDto>>> GetWriterHeader(
            [FromRoute] int writerId)
        {
            // service 返回的是 WriterDto
            var writer = await _writerInfoService.GetWriterHeaderAsync(writerId);

            var result = new WriterHeaderDataDto
            {
                Writer = writer
            };

            return Success(result);
        }

        /// <summary>
        /// 获取作者全部作品列表
        /// GET /writerinfo/works/{writerId}
        ///
        /// 对应：
        ///   get_writer_works(writer_id)
        ///   Result.success(WriterWorksData(works=works).dict())
        /// </summary>
        [HttpGet("works/{writerId:int}")]
        public async Task<ActionResult<ApiResponse<WriterWorksDataDto>>> GetWriterWorks(
            [FromRoute] int writerId)
        {
            // service 返回的是 List<WorkDto>
            var works = await _writerInfoService.GetWriterWorksAsync(writerId);

            var result = new WriterWorksDataDto
            {
                Works = works
            };

            return Success(result);
        }
    }
}
