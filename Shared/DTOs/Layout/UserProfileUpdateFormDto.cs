using System.Text.Json.Serialization;

namespace TomatoNovels.Shared.DTOs.Layout
{
    /// <summary>
    /// 用户资料更新表单（对应 Python 的 UserProfileUpdateForm）
    /// </summary>
    public class UserProfileUpdateFormDto
    {
        /// <summary>
        /// 用户 ID
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// 当前头像完整 URL（没上传新头像时用这个做回退）
        /// </summary>
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        /// <summary>
        /// 昵称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 简介
        /// </summary>
        [JsonPropertyName("introduction")]
        public string Introduction { get; set; } = string.Empty;
    }
}
