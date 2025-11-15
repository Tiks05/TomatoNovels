using Microsoft.AspNetCore.Mvc;
using TomatoNovels.Controllers;
using TomatoNovels.Shared.ApiResponse;
using TomatoNovels.Shared.DTOs.Comment;
using TomatoNovels.Services;

namespace TomatoNovels.Controllers
{
    /// <summary>
    /// 评论相关接口
    /// 对应 Flask 的 comment_bp 蓝图：
    ///   /comment/list
    ///   /comment/likes
    ///   /comment/create
    /// </summary>
    [Route("api/comment")]
    public class CommentController : ApiControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// 获取某本书的评论列表
        /// GET /api/comment/list?bookId=123
        /// </summary>
        [HttpGet("list")]
        public async Task<ActionResult<ApiResponse<List<CommentItemDto>>>> GetCommentList(
            [FromQuery] int bookId)
        {
            // 对应 Python：
            // book_id = int(request.args.get('book_id', 0))
            // comment_list = get_comments_by_book(book_id)
            var commentList = await _commentService.GetCommentsByBookAsync(bookId);
            return Success(commentList);
        }

        /// <summary>
        /// 批量点赞（增加点赞数）
        /// POST /api/comment/likes
        /// Body: { "ids": [1,2,3] }
        /// </summary>
        [HttpPost("likes")]
        public async Task<ActionResult<ApiResponse>> UpdateLikes(
            [FromBody] LikeUpdateRequestDto request)
        {
            // 对应 Python：
            // data = LikeUpdateRequest(**request.get_json())
            // increase_likes_by_ids(data.ids)
            await _commentService.IncreaseLikesByIdsAsync(request.Ids);
            return Success();
        }

        /// <summary>
        /// 创建评论（支持一级/子评论）
        /// POST /api/comment/create
        /// Body: CreateCommentRequestDto
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse>> CreateComment(
            [FromBody] CreateCommentRequestDto request)
        {
            // 对应 Python：
            // data = CreateCommentRequest(**request.json)
            // create_comment(data)
            await _commentService.CreateCommentAsync(request);
            return Success();
        }
    }
}
