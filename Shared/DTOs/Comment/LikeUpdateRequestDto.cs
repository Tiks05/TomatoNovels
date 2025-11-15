using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Comment
{
    /// <summary>
    /// 点赞更新请求（批量）
    /// 对应 Python: LikeUpdateRequest
    /// </summary>
    public class LikeUpdateRequestDto
    {
        /// <summary>
        /// 需要增加点赞数的评论 Id 列表
        /// Python: ids: List[int]
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "ids 不能为空")]
        [JsonPropertyName("ids")]
        public List<int> Ids { get; set; } = new();
    }
}
