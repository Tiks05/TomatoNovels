namespace TomatoNovels.Core.Exceptions
{
    /// <summary>
    /// 自定义业务异常（对应 Flask 的 APIException）
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// 业务错误码（例如 40001 / 40002）
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 可选的附加数据（对应 Flask 的 data 字段）
        /// </summary>
        public object? DataObject { get; }

        public ApiException(
            string message = "业务异常",
            int code = 40000,
            object? data = null
        ) : base(message)
        {
            Code = code;
            DataObject = data;
        }
    }
}
