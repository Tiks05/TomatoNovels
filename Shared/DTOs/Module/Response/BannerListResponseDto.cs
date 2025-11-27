using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Module.Response
{
    public class BannerListResponseDto
    {
        [JsonPropertyName("banner_url")]
        public string BannerUrl { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;
    }
}
