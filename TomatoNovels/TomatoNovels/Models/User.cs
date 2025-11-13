using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("user")]
[Index("Phone", Name = "phone", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [Column("phone")]
    [StringLength(11)]
    public string Phone { get; set; } = null!;

    /// <summary>
    /// 加密后的密码
    /// </summary>
    [Column("password")]
    [StringLength(60)]
    public string? Password { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Column("nickname")]
    [StringLength(16)]
    public string? Nickname { get; set; }

    /// <summary>
    /// 用户角色（user/author/admin）
    /// </summary>
    [Column("role")]
    [StringLength(16)]
    public string? Role { get; set; }

    /// <summary>
    /// 注册时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 头像路径
    /// </summary>
    [Column("avatar")]
    [StringLength(255)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 签名
    /// </summary>
    [Column("signature")]
    [StringLength(255)]
    public string? Signature { get; set; }

    /// <summary>
    /// 生活照路径
    /// </summary>
    [Column("life_photo")]
    [StringLength(255)]
    public string? LifePhoto { get; set; }

    /// <summary>
    /// 代表作
    /// </summary>
    [Column("masterpiece")]
    [StringLength(64)]
    public string? Masterpiece { get; set; }

    /// <summary>
    /// 作家等级
    /// </summary>
    [Column("author_level")]
    [StringLength(16)]
    public string? AuthorLevel { get; set; }

    /// <summary>
    /// 成为作家的时间
    /// </summary>
    [Column("become_author_at", TypeName = "datetime")]
    public DateTime? BecomeAuthorAt { get; set; }

    /// <summary>
    /// 作家数值等级（Lv.0 起）
    /// </summary>
    [Column("level")]
    public int Level { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    [InverseProperty("ReplyToUser")]
    public virtual ICollection<Comment> CommentReplyToUsers { get; set; } = new List<Comment>();

    [InverseProperty("User")]
    public virtual ICollection<Comment> CommentUsers { get; set; } = new List<Comment>();

    [InverseProperty("User")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [InverseProperty("Followed")]
    public virtual ICollection<Follow> FollowFolloweds { get; set; } = new List<Follow>();

    [InverseProperty("Follower")]
    public virtual ICollection<Follow> FollowFollowers { get; set; } = new List<Follow>();
}
