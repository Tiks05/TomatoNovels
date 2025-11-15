using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Comment
{
    /// <summary>
    /// 创建评论请求
    /// 对应 Python: CreateCommentRequest
    /// </summary>
    public class CreateCommentRequestDto
    {
        /// <summary>
        /// 前端传递的用户 ID
        /// </summary>
        [Required]
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 所属书籍 ID
        /// </summary>
        [Required]
        [JsonPropertyName("book_id")]
        public int BookId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Required]
        [MaxLength(2000)]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 父评论 ID（如果是回复则有）
        /// </summary>
        [JsonPropertyName("parent_id")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 被 @ 的用户 ID（替代昵称）
        /// </summary>
        [JsonPropertyName("reply_to_user_id")]
        public int? ReplyToUserId { get; set; }
    }
}
