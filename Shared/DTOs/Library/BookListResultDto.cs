using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Library
{
    /// <summary>
    /// 书库列表返回结构（对应 BookListOutSchema）
    /// </summary>
    public class BookListResultDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("records")]
        public List<BookOutDto> Records { get; set; } = new();
    }
}
