using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("chapter")]
[Index("VolumeId", Name = "volume_id")]
public partial class Chapter
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 章节序号
    /// </summary>
    [Column("chapter_num")]
    public int ChapterNum { get; set; }

    /// <summary>
    /// 章节标题
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 章节字数
    /// </summary>
    [Column("word_count")]
    public int? WordCount { get; set; }

    /// <summary>
    /// 章节正文内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

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
    /// 所属卷ID
    /// </summary>
    [Column("volume_id")]
    public int VolumeId { get; set; }

    /// <summary>
    /// 章节审核状态：published已发布、reviewing审核中、rejected未通过、pending待发布
    /// </summary>
    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [ForeignKey("VolumeId")]
    [InverseProperty("Chapters")]
    public virtual Volume Volume { get; set; } = null!;
}
