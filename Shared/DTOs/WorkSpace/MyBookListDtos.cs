using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 我的作品列表查询参数（对应 MyBookListQuery）
    /// </summary>
    public class MyBookListQueryDto
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 我的作品列表单项（对应 BookListItem）
    /// </summary>
    public class BookListItemDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("pic")]
        public string Pic { get; set; } = string.Empty;

        /// <summary>
        /// 前端展示的“更新时间”字符串（now）
        /// </summary>
        [JsonPropertyName("now")]
        public string Now { get; set; } = string.Empty;

        /// <summary>
        /// 当前最新章节号
        /// </summary>
        [JsonPropertyName("chapter")]
        public int Chapter { get; set; }

        [JsonPropertyName("words")]
        public int Words { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }

    /// <summary>
    /// 我的作品列表返回结构（对应 BookListResponse）
    /// </summary>
    public class BookListResponseDto
    {
        [JsonPropertyName("books")]
        public List<BookListItemDto> Books { get; set; } = new();
    }
}
