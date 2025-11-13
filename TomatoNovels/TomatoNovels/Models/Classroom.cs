using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("classroom")]
public partial class Classroom
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 主标题
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 分类标签
    /// </summary>
    [Column("category_type")]
    [StringLength(50)]
    public string? CategoryType { get; set; }

    /// <summary>
    /// 封面图路径
    /// </summary>
    [Column("cover_url")]
    [StringLength(255)]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 简介/摘要
    /// </summary>
    [Column("intro")]
    [StringLength(255)]
    public string? Intro { get; set; }

    /// <summary>
    /// 是否展示视频按钮
    /// </summary>
    [Column("is_include_video")]
    public bool IsIncludeVideo { get; set; }

    /// <summary>
    /// HTML内容，含视频或正文
    /// </summary>
    [Column("content", TypeName = "text")]
    public string? Content { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("create_at", TypeName = "datetime")]
    public DateTime CreateAt { get; set; }
}
