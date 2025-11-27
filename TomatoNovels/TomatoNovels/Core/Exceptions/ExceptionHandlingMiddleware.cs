using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TomatoNovels.Shared.ApiResponse;

namespace TomatoNovels.Core.Exceptions
{
    /// <summary>
    /// 全局异常处理中间件，对应 Flask 的 register_error_handlers(app)
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // 默认：未知异常 → 500
            var statusCode = (int)HttpStatusCode.InternalServerError;
            ApiResponse<object?> apiResult;

            switch (ex)
            {
                // 业务异常，对应 Flask 的 APIException
                case ApiException apiEx:
                    _logger.LogWarning("[业务异常] {Message}", apiEx.Message);
                    statusCode = StatusCodes.Status200OK; // 全部 200
                    apiResult = ApiResponse.Fail(
                        code: "Common.BusinessError",
                        message: apiEx.Message
                    );
                    break;

                // 参数校验异常：例如 FluentValidation 后面可以对接到这里
                case ValidationException validationEx:
                    _logger.LogWarning("[参数校验失败] {Message}", validationEx.Message);
                    statusCode = StatusCodes.Status400BadRequest;
                    apiResult = ApiResponse.Fail(
                        code: "Common.ValidationFailed",
                        message: "参数校验失败",
                        details: validationEx.Errors
                    );
                    break;

                // EF Core 数据库异常，对应 SQLAlchemyError
                case DbUpdateException dbEx:
                    _logger.LogError(dbEx, "[数据库异常]");
                    statusCode = StatusCodes.Status500InternalServerError;
                    apiResult = ApiResponse.Fail(
                        code: "Database.Error",
                        message: "数据库操作异常，请联系管理员"
                    );
                    break;

                // 其它未处理异常，对应 Flask 的兜底 Exception
                default:
                    _logger.LogError(ex, "[系统异常]");
                    statusCode = StatusCodes.Status500InternalServerError;
                    apiResult = ApiResponse.Fail(
                        code: "Common.InternalError",
                        message: "系统异常，请稍后再试"
                    );
                    break;
            }

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(apiResult, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }

    /// <summary>
    /// 示例 ValidationException（占位，如果你之后上 FluentValidation 可以替换掉）
    /// </summary>
    public class ValidationException : Exception
    {
        public object? Errors { get; }

        public ValidationException(string message, object? errors = null) : base(message)
        {
            Errors = errors;
        }
    }
}
