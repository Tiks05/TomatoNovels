using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("news")]
public partial class News
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 资讯标题
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 资讯内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string? Content { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// 封面图路径
    /// </summary>
    [Column("cover_url")]
    [StringLength(255)]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 类型：notice资讯，active活动
    /// </summary>
    [Column("type")]
    [StringLength(16)]
    public string Type { get; set; } = null!;

    /// <summary>
    /// 首页Banner路径
    /// </summary>
    [Column("banner_url")]
    [StringLength(255)]
    public string? BannerUrl { get; set; }

    /// <summary>
    /// 是否设为首页Banner图
    /// </summary>
    [Column("is_banner")]
    public bool IsBanner { get; set; }

    /// <summary>
    /// 资讯图路径
    /// </summary>
    [Column("notice_url")]
    [StringLength(255)]
    public string? NoticeUrl { get; set; }

    /// <summary>
    /// 是否notice资讯
    /// </summary>
    [Column("is_notice")]
    public bool IsNotice { get; set; }
}
