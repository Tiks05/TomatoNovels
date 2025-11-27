namespace TomatoNovels.Shared.DTOs.Home;

public class MenuItemDto
{
    /// <summary>
    /// 跳转路径，例如 /home /library
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 菜单显示的名字，例如 首页 / 书库
    /// </summary>
    public string Label { get; set; } = string.Empty;
}
