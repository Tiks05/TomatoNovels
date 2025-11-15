using TomatoNovels.Shared.DTOs.Comment;

namespace TomatoNovels.Services
{
    /// <summary>
    /// 评论相关业务接口
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// 根据书籍 ID 获取评论列表（顶级 + children 平铺）
        /// </summary>
        Task<List<CommentItemDto>> GetCommentsByBookAsync(int bookId);

        /// <summary>
        /// 批量增加点赞数
        /// </summary>
        Task IncreaseLikesByIdsAsync(IEnumerable<int> ids);

        /// <summary>
        /// 创建评论
        /// </summary>
        Task CreateCommentAsync(CreateCommentRequestDto request);
    }
}
