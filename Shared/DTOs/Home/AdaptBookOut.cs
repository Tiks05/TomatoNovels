using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class AdaptBookOut
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("pic")]
        public string Pic { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }
}
