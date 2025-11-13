using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("favorite")]
[Index("BookId", Name = "book_id")]
[Index("UserId", "BookId", Name = "uniq_favorite", IsUnique = true)]
public partial class Favorite
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 收藏者ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 被收藏的小说ID
    /// </summary>
    [Column("book_id")]
    public int BookId { get; set; }

    /// <summary>
    /// 收藏时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Favorites")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Favorites")]
    public virtual User User { get; set; } = null!;
}
