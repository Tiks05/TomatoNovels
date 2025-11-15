using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Home
{
    public class BookOut
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = "";

        [JsonPropertyName("cover_url")]
        public string CoverUrl { get; set; } = "";

        [JsonPropertyName("author_nickname")]
        public string AuthorNickname { get; set; } = "";

        [JsonPropertyName("path")]
        public string Path { get; set; } = "";
    }
}
