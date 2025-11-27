namespace TomatoNovels.Shared.DTOs.Home;

public class ShortcutItemDto
{
    /// <summary>
    /// 快捷入口标题，如：作家福利、作家专区
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 快捷入口描述，如：番茄作家福利区
    /// </summary>
    public string Desc { get; set; } = string.Empty;

    /// <summary>
    /// 点击跳转路径
    /// </summary>
    public string Path { get; set; } = string.Empty;
}
