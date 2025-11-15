using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Comment
{
    /// <summary>
    /// 评论用户信息（user / reply_to_user）
    /// </summary>
    public class CommentUserDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;
    }
}
