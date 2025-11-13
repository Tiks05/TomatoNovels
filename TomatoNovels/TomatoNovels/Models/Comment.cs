using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("comment")]
[Index("BookId", Name = "book_id")]
[Index("ParentId", Name = "parent_id")]
[Index("ReplyToUserId", Name = "reply_to_user_id")]
[Index("UserId", Name = "user_id")]
public partial class Comment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 评论者ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 小说ID
    /// </summary>
    [Column("book_id")]
    public int BookId { get; set; }

    /// <summary>
    /// 父评论ID（用于楼中楼回复）
    /// </summary>
    [Column("parent_id")]
    public int? ParentId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 评论时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 被@用户ID
    /// </summary>
    [Column("reply_to_user_id")]
    public int? ReplyToUserId { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    [Column("likes")]
    public int? Likes { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Comments")]
    public virtual Book Book { get; set; } = null!;

    [InverseProperty("Parent")]
    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Comment? Parent { get; set; }

    [ForeignKey("ReplyToUserId")]
    [InverseProperty("CommentReplyToUsers")]
    public virtual User? ReplyToUser { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CommentUsers")]
    public virtual User User { get; set; } = null!;
}
