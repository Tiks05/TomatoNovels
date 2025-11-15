using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TomatoNovels.Data;
using TomatoNovels.Shared.DTOs.Comment;
using TomatoNovels.Models;

namespace TomatoNovels.Services.Impl
{
    /// <summary>
    /// 评论业务实现，对应 Python 的 comment_service.py
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(
            AppDbContext db,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public async Task<List<CommentItemDto>> GetCommentsByBookAsync(int bookId)
        {
            // === 对应 Python:
            // all_comments = (
            //   db.session.query(Comment)
            //   .options(joinedload(Comment.user), joinedload(Comment.reply_to_user))
            //   .filter(Comment.book_id == book_id)
            //   .order_by(Comment.created_at.asc())
            //   .all()
            // )

            var allComments = await _db.Comments
                .Include(c => c.User)
                .Include(c => c.ReplyToUser)
                .Where(c => c.BookId == bookId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            var hostPrefix = GetHostPrefix();

            // children_map + top_level_comments
            var childrenMap = new Dictionary<int, List<Comment>>();
            var topLevelComments = new List<Comment>();

            foreach (var comment in allComments)
            {
                if (comment.ParentId.HasValue)
                {
                    var pid = comment.ParentId.Value;
                    if (!childrenMap.TryGetValue(pid, out var list))
                    {
                        list = new List<Comment>();
                        childrenMap[pid] = list;
                    }
                    list.Add(comment);
                }
                else
                {
                    topLevelComments.Add(comment);
                }
            }

            // 对应 Python flatten_comment_tree(parent)
            List<CommentChildDto> FlattenCommentTree(Comment parent)
            {
                var result = new List<CommentChildDto>();

                if (!childrenMap.TryGetValue(parent.Id, out var children))
                {
                    return result;
                }

                var parentUserId = parent.User?.Id;

                foreach (var child in children)
                {
                    var isFlat = child.ReplyToUserId.HasValue &&
                                 child.ReplyToUserId.Value != parentUserId;

                    CommentUserDto? replyToUserDto = null;
                    if (child.ReplyToUser != null)
                    {
                        replyToUserDto = new CommentUserDto
                        {
                            Id = child.ReplyToUser.Id,
                            Name = child.ReplyToUser.Nickname,
                            Avatar = BuildAvatarUrl(hostPrefix, child.ReplyToUser.Avatar)
                        };
                    }

                    var userDto = new CommentUserDto
                    {
                        Id = child.User!.Id,
                        Name = child.User.Nickname,
                        Avatar = BuildAvatarUrl(hostPrefix, child.User.Avatar)
                    };

                    var item = new CommentChildDto
                    {
                        Id = child.Id,
                        Content = child.Content,
                        Time = child.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                        Likes = child.Likes,
                        ParentId = child.ParentId,
                        IsFlat = isFlat,
                        ReplyToUser = replyToUserDto,
                        User = userDto
                    };

                    result.Add(item);

                    // 递归收集后代评论
                    result.AddRange(FlattenCommentTree(child));
                }

                return result;
            }

            var results = new List<CommentItemDto>();

            foreach (var top in topLevelComments)
            {
                var topUserDto = new CommentUserDto
                {
                    Id = top.User!.Id,
                    Name = top.User.Nickname,
                    Avatar = BuildAvatarUrl(hostPrefix, top.User.Avatar)
                };

                var item = new CommentItemDto
                {
                    Id = top.Id,
                    Content = top.Content,
                    Time = top.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    Likes = top.Likes,
                    ParentId = null,
                    IsFlat = false,
                    ReplyToUser = null,
                    User = topUserDto,
                    Children = FlattenCommentTree(top)
                };

                results.Add(item);
            }

            return results;
        }

        /// <inheritdoc />
        public async Task IncreaseLikesByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids?.Distinct().ToList() ?? new List<int>();
            if (idList.Count == 0)
            {
                return;
            }

            var comments = await _db.Comments
                .Where(c => idList.Contains(c.Id))
                .ToListAsync();

            foreach (var c in comments)
            {
                c.Likes += 1;
            }

            await _db.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task CreateCommentAsync(CreateCommentRequestDto request)
        {
            var comment = new Comment
            {
                UserId = request.UserId,
                BookId = request.BookId,
                Content = request.Content,
                ParentId = request.ParentId,
                ReplyToUserId = request.ReplyToUserId,
                CreatedAt = DateTime.Now,
                Likes = 0
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
        }

        #region 辅助方法

        private string GetHostPrefix()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return string.Empty;
            }

            var req = httpContext.Request;
            var hostUrl = $"{req.Scheme}://{req.Host.Value}";
            return hostUrl.TrimEnd('/');
        }

        private static string BuildAvatarUrl(string hostPrefix, string? avatarPath)
        {
            if (string.IsNullOrWhiteSpace(avatarPath))
            {
                return string.Empty;
            }

            return $"{hostPrefix}{avatarPath}";
        }

        #endregion
    }
}
