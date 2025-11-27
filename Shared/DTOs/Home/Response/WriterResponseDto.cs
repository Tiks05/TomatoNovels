using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home.Response
{
    public class WriterResponseDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = "";

        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("pic")]
        public string Pic { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }
}
