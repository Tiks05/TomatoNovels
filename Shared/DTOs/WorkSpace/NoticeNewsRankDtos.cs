using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 公告列表项（对应 NoticeItemSchema）
    /// </summary>
    public class NoticeItemDto
    {
        [JsonPropertyName("notice_url")]
        public string NoticeUrl { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }

    /// <summary>
    /// 作家新闻列表项（对应 NewsListItemSchema）
    /// </summary>
    public class NewsListItemDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }

    /// <summary>
    /// 榜单单条记录（对应 SortItem）
    /// </summary>
    public class SortItemDto
    {
        [JsonPropertyName("num")]
        public int Num { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("pic")]
        public string Pic { get; set; } = string.Empty;

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = string.Empty;
    }

    /// <summary>
    /// 榜单返回结构（对应 BookRankResponse）
    /// </summary>
    public class BookRankResponseDto
    {
        [JsonPropertyName("plot_type")]
        public string PlotType { get; set; } = string.Empty;

        [JsonPropertyName("child")]
        public List<SortItemDto> Child { get; set; } = new();
    }
}
