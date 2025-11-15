using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Library
{
    /// <summary>
    /// 书库筛选请求参数（对应 BookListQuerySchema）
    /// </summary>
    public class BookListQueryDto
    {
        [JsonPropertyName("reader_type")]
        public string? ReaderType { get; set; }

        // 'theme_type' | 'role_type' | 'plot_type'
        [JsonPropertyName("category_group")]
        public string? CategoryGroup { get; set; }

        [JsonPropertyName("category_type")]
        public string? CategoryType { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("word_count_range")]
        public string? WordCountRange { get; set; }

        // 'hot' | 'new' | 'words'
        [JsonPropertyName("sort")]
        public string? Sort { get; set; }

        [Range(1, int.MaxValue)]
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue)]
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 10;
    }
}
