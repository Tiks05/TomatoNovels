using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("book")]
[Index("UserId", Name = "user_id")]
public partial class Book
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 作者ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 小说名称
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 读者性别 男生/女生
    /// </summary>
    [Column("reader_type")]
    [StringLength(8)]
    public string? ReaderType { get; set; }

    /// <summary>
    /// 情节分类标签
    /// </summary>
    [Column("plot_type")]
    [StringLength(64)]
    public string? PlotType { get; set; }

    /// <summary>
    /// 连载状态 已完结/连载中
    /// </summary>
    [Column("status")]
    [StringLength(16)]
    public string? Status { get; set; }

    /// <summary>
    /// 实际字数
    /// </summary>
    [Column("word_count")]
    public int? WordCount { get; set; }

    /// <summary>
    /// 字数区间
    /// </summary>
    [Column("word_count_range")]
    [StringLength(20)]
    public string? WordCountRange { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    [Column("tags")]
    [StringLength(255)]
    public string? Tags { get; set; }

    /// <summary>
    /// 简介
    /// </summary>
    [Column("intro", TypeName = "text")]
    public string? Intro { get; set; }

    /// <summary>
    /// 封面图片
    /// </summary>
    [Column("cover_url")]
    [StringLength(255)]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// 被收藏数量
    /// </summary>
    [Column("favorite_count")]
    public int FavoriteCount { get; set; }

    /// <summary>
    /// 主题分类标签
    /// </summary>
    [Column("theme_type")]
    [StringLength(64)]
    public string? ThemeType { get; set; }

    /// <summary>
    /// 角色分类标签
    /// </summary>
    [Column("role_type")]
    [StringLength(64)]
    public string? RoleType { get; set; }

    /// <summary>
    /// 主角名
    /// </summary>
    [Column("hero")]
    [StringLength(64)]
    public string? Hero { get; set; }

    /// <summary>
    /// 签约状态 未签约/已签约
    /// </summary>
    [Column("sign_status")]
    [StringLength(16)]
    public string SignStatus { get; set; } = null!;

    [InverseProperty("Book")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Book")]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [ForeignKey("UserId")]
    [InverseProperty("Books")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Book")]
    public virtual ICollection<Volume> Volumes { get; set; } = new List<Volume>();
}
