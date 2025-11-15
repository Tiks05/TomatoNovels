using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Layout
{
    public class SearchBookRequestDto
    {
        /// <summary>
        /// 关键词（书名或作者）
        /// </summary>
        [JsonPropertyName("keyword")]
        public string? Keyword { get; set; }

        /// <summary>
        /// 排序方式：0=相关，1=最热，2=最新
        /// </summary>
        [JsonPropertyName("type")]
        public int? Type { get; set; } = 0;

        /// <summary>
        /// 更新时间筛选索引：0=全部
        /// </summary>
        [JsonPropertyName("timeindex")]
        public int? TimeIndex { get; set; } = 0;

        /// <summary>
        /// 字数范围索引：0=全部
        /// </summary>
        [JsonPropertyName("numindex")]
        public int? NumIndex { get; set; } = 0;

        /// <summary>
        /// 状态筛选索引：0=全部
        /// </summary>
        [JsonPropertyName("stateindex")]
        public int? StateIndex { get; set; } = 0;

        /// <summary>
        /// 当前页码（必须 > 0）
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }
}
