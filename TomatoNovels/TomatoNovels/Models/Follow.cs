using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TomatoNovels.Models;

[Table("follow")]
[Index("FollowedId", Name = "followed_id")]
[Index("FollowerId", "FollowedId", Name = "uniq_follow", IsUnique = true)]
public partial class Follow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 关注者ID（谁去关注别人）
    /// </summary>
    [Column("follower_id")]
    public int FollowerId { get; set; }

    /// <summary>
    /// 被关注者ID（被谁关注）
    /// </summary>
    [Column("followed_id")]
    public int FollowedId { get; set; }

    /// <summary>
    /// 关注时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("FollowedId")]
    [InverseProperty("FollowFolloweds")]
    public virtual User Followed { get; set; } = null!;

    [ForeignKey("FollowerId")]
    [InverseProperty("FollowFollowers")]
    public virtual User Follower { get; set; } = null!;
}
