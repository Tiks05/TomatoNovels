using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Shared.ApiResponse;

namespace TomatoNovels.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// 成功响应（Success = true，带数据）
        /// </summary>
        protected ActionResult<ApiResponse<T>> Success<T>(T data)
        {
            return Ok(ApiResponse<T>.Ok(data));
        }

        /// <summary>
        /// 成功响应（无 data）
        /// </summary>
        protected ActionResult<ApiResponse> Success()
        {
            return Ok(ApiResponse.Ok());
        }

        /// <summary>
        /// 失败响应（泛型版，用于与方法签名对应）
        /// 例如：return Fail<LoginDto>(...)
        /// </summary>
        protected ActionResult<ApiResponse<T>> Fail<T>(
            string code,
            string message,
            object? details = null,
            int statusCode = 400)
        {
            return StatusCode(
                statusCode,
                ApiResponse<T>.Fail(code, message, details)
            );
        }

        /// <summary>
        /// 失败响应（非泛型版，用于不需要返回 T 的情况）
        /// </summary>
        protected ActionResult<ApiResponse> Fail(
            string code,
            string message,
            object? details = null,
            int statusCode = 400)
        {
            return StatusCode(
                statusCode,
                ApiResponse.Fail(code, message, details)
            );
        }
    }
}
