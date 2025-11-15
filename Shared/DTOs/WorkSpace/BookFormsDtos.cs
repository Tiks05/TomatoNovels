using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Workspace
{
    /// <summary>
    /// 创建作品表单（对应 BookCreateForm）
    /// </summary>
    public class BookCreateFormDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("reader_type")]
        public string ReaderType { get; set; } = string.Empty;

        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;

        [JsonPropertyName("hero1")]
        public string Hero1 { get; set; } = string.Empty;

        [JsonPropertyName("hero2")]
        public string Hero2 { get; set; } = string.Empty;

        [JsonPropertyName("introduction")]
        public string Introduction { get; set; } = string.Empty;
    }

    /// <summary>
    /// 更新作品表单（对应 BookUpdateForm）
    /// </summary>
    public class BookUpdateFormDto
    {
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("reader_type")]
        public string ReaderType { get; set; } = string.Empty;

        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;

        [JsonPropertyName("hero")]
        public string Hero { get; set; } = string.Empty;

        [JsonPropertyName("introduction")]
        public string Introduction { get; set; } = string.Empty;
    }
}
