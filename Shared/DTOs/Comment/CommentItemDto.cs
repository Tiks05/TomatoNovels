using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Comment
{
    /// <summary>
    /// 顶级评论条目（包含 children 平铺列表）
    /// 对应 Python get_comments_by_book 的 results 中的每一项
    /// </summary>
    public class CommentItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

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

        /// <summary>
        /// 所有后代子评论的平铺列表
        /// Python: "children": flatten_comment_tree(top)
        /// </summary>
        [JsonPropertyName("children")]
        public List<CommentChildDto> Children { get; set; } = new();
    }
}
