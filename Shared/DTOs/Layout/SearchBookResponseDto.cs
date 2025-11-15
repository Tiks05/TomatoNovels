using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Layout
{
    public class SearchBookResponseDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("records")]
        public List<SearchBookItemDto> Records { get; set; } = new();
    }
}
