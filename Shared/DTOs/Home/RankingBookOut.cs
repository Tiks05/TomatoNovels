using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class RankingBookOut
    {
        [JsonPropertyName("num")]
        public string Num { get; set; } = "";

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";

        [JsonPropertyName("pic")]
        public string Pic { get; set; } = "";

        [JsonPropertyName("author")]
        public string Author { get; set; } = "";
    }
}
