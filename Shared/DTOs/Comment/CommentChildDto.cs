using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Comment
{
    /// <summary>
    /// 子评论条目（没有 children 字段）
    /// 对应 Python flatten_comment_tree() 返回的每一项
    /// </summary>
    public class CommentChildDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 格式化时间字符串：yyyy-MM-dd HH:mm
        /// Python: created_at.strftime('%Y-%m-%d %H:%M')
        /// </summary>
        [JsonPropertyName("time")]
        public string Time { get; set; } = string.Empty;

        [JsonPropertyName("likes")]
        public int? Likes { get; set; }

        [JsonPropertyName("parent_id")]
        public int? ParentId { get; set; }

        [JsonPropertyName("is_flat")]
        public bool IsFlat { get; set; }

        [JsonPropertyName("reply_to_user")]
        public CommentUserDto? ReplyToUser { get; set; }

        [JsonPropertyName("user")]
        public CommentUserDto User { get; set; } = new();
    }
}
