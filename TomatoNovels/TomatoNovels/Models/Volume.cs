using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("volume")]
[Index("BookId", Name = "book_id")]
public partial class Volume
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 所属书籍ID
    /// </summary>
    [Column("book_id")]
    public int BookId { get; set; }

    /// <summary>
    /// 卷标题
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 排序
    /// </summary>
    [Column("sort")]
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Volumes")]
    public virtual Book Book { get; set; } = null!;

    [InverseProperty("Volume")]
    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
}
