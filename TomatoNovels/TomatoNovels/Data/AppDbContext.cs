using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using TomatoNovels.Models;

namespace TomatoNovels.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AlembicVersion> AlembicVersions { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Volume> Volumes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=tomato_novels;user=root;password=123456;charset=utf8mb4;sslmode=None;allowpublickeyretrieval=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.43-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AlembicVersion>(entity =>
        {
            entity.HasKey(e => e.VersionNum).HasName("PRIMARY");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CoverUrl).HasComment("封面图片");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("创建时间");
            entity.Property(e => e.FavoriteCount).HasComment("被收藏数量");
            entity.Property(e => e.Hero).HasComment("主角名");
            entity.Property(e => e.Intro).HasComment("简介");
            entity.Property(e => e.PlotType).HasComment("情节分类标签");
            entity.Property(e => e.ReaderType).HasComment("读者性别 男生/女生");
            entity.Property(e => e.RoleType).HasComment("角色分类标签");
            entity.Property(e => e.SignStatus).HasComment("签约状态 未签约/已签约");
            entity.Property(e => e.Status).HasComment("连载状态 已完结/连载中");
            entity.Property(e => e.Tags).HasComment("标签");
            entity.Property(e => e.ThemeType).HasComment("主题分类标签");
            entity.Property(e => e.Title).HasComment("小说名称");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("更新时间");
            entity.Property(e => e.UserId).HasComment("作者ID");
            entity.Property(e => e.WordCount).HasComment("实际字数");
            entity.Property(e => e.WordCountRange).HasComment("字数区间");

            entity.HasOne(d => d.User).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_ibfk_1");
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.ChapterNum).HasComment("章节序号");
            entity.Property(e => e.Content).HasComment("章节正文内容");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("创建时间");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'published'")
                .HasComment("章节审核状态：published已发布、reviewing审核中、rejected未通过、pending待发布");
            entity.Property(e => e.Title).HasComment("章节标题");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("更新时间");
            entity.Property(e => e.VolumeId).HasComment("所属卷ID");
            entity.Property(e => e.WordCount).HasComment("章节字数");

            entity.HasOne(d => d.Volume).WithMany(p => p.Chapters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chapter_ibfk_1");
        });

        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CategoryType).HasComment("分类标签");
            entity.Property(e => e.Content).HasComment("HTML内容，含视频或正文");
            entity.Property(e => e.CoverUrl).HasComment("封面图路径");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("now()")
                .HasComment("创建时间");
            entity.Property(e => e.Intro).HasComment("简介/摘要");
            entity.Property(e => e.IsIncludeVideo).HasComment("是否展示视频按钮");
            entity.Property(e => e.Title).HasComment("主标题");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.BookId).HasComment("小说ID");
            entity.Property(e => e.Content).HasComment("评论内容");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("评论时间");
            entity.Property(e => e.Likes).HasComment("点赞数");
            entity.Property(e => e.ParentId).HasComment("父评论ID（用于楼中楼回复）");
            entity.Property(e => e.ReplyToUserId).HasComment("被@用户ID");
            entity.Property(e => e.UserId).HasComment("评论者ID");

            entity.HasOne(d => d.Book).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_ibfk_1");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("comment_ibfk_2");

            entity.HasOne(d => d.ReplyToUser).WithMany(p => p.CommentReplyToUsers).HasConstraintName("comment_ibfk_4");

            entity.HasOne(d => d.User).WithMany(p => p.CommentUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_ibfk_3");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.BookId).HasComment("被收藏的小说ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("收藏时间");
            entity.Property(e => e.UserId).HasComment("收藏者ID");

            entity.HasOne(d => d.Book).WithMany(p => p.Favorites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorite_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorite_ibfk_2");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("关注时间");
            entity.Property(e => e.FollowedId).HasComment("被关注者ID（被谁关注）");
            entity.Property(e => e.FollowerId).HasComment("关注者ID（谁去关注别人）");

            entity.HasOne(d => d.Followed).WithMany(p => p.FollowFolloweds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("follow_ibfk_1");

            entity.HasOne(d => d.Follower).WithMany(p => p.FollowFollowers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("follow_ibfk_2");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.BannerUrl).HasComment("首页Banner路径");
            entity.Property(e => e.Content).HasComment("资讯内容");
            entity.Property(e => e.CoverUrl).HasComment("封面图路径");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("发布时间");
            entity.Property(e => e.IsBanner).HasComment("是否设为首页Banner图");
            entity.Property(e => e.IsNotice).HasComment("是否notice资讯");
            entity.Property(e => e.NoticeUrl).HasComment("资讯图路径");
            entity.Property(e => e.Title).HasComment("资讯标题");
            entity.Property(e => e.Type).HasComment("类型：notice资讯，active活动");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("更新时间");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.AuthorLevel).HasComment("作家等级");
            entity.Property(e => e.Avatar).HasComment("头像路径");
            entity.Property(e => e.BecomeAuthorAt).HasComment("成为作家的时间");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("注册时间");
            entity.Property(e => e.Level).HasComment("作家数值等级（Lv.0 起）");
            entity.Property(e => e.LifePhoto).HasComment("生活照路径");
            entity.Property(e => e.Masterpiece).HasComment("代表作");
            entity.Property(e => e.Nickname).HasComment("昵称");
            entity.Property(e => e.Password).HasComment("加密后的密码");
            entity.Property(e => e.Phone).HasComment("手机号");
            entity.Property(e => e.Role).HasComment("用户角色（user/author/admin）");
            entity.Property(e => e.Signature).HasComment("签名");
        });

        modelBuilder.Entity<Volume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.BookId).HasComment("所属书籍ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasComment("创建时间");
            entity.Property(e => e.Sort).HasComment("排序");
            entity.Property(e => e.Title).HasComment("卷标题");

            entity.HasOne(d => d.Book).WithMany(p => p.Volumes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("volume_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
