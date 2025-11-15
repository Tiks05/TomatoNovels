using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.ApiResponse
{
    /// <summary>
    /// 标准 API 返回结果（泛型版）
    /// </summary>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 成功时返回的数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 失败时的错误信息
        /// </summary>
        public ApiResponseError? Error { get; set; }

        #region 工厂方法（推荐用这些）

        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Error = null
            };
        }

        public static ApiResponse<T> Fail(string code, string message, object? details = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Error = new ApiResponseError
                {
                    Code = code,
                    Message = message,
                    Details = details
                }
            };
        }

        #endregion
    }

    /// <summary>
    /// 非泛型版本（某些地方方便用）
    /// </summary>
    public class ApiResponse : ApiResponse<object?>
    {
        public static ApiResponse Ok(object? data = null)
        {
            return new ApiResponse
            {
                Success = true,
                Data = data,
                Error = null
            };
        }

        public new static ApiResponse Fail(string code, string message, object? details = null)
        {
            return new ApiResponse
            {
                Success = false,
                Data = null,
                Error = new ApiResponseError
                {
                    Code = code,
                    Message = message,
                    Details = details
                }
            };
        }
    }

    /// <summary>
    /// 错误信息结构
    /// </summary>
    public class ApiResponseError
    {
        /// <summary>
        /// 机器可读的错误码，例如：Auth.LoginFailed / User.NotFound
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 给前端/用户看的错误信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 可选详细信息（调试、字段错误列表等）
        /// </summary>
        public object? Details { get; set; }
    }
}
