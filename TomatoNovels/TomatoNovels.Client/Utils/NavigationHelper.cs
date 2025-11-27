using Microsoft.AspNetCore.Components;
using System;

namespace TomatoNovels.Client.Utils
{
    /// <summary>
    /// 封装的导航工具类
    /// 提供 GoTo(path, replace) 方法
    /// </summary>
    public class NavigationHelper
    {
        private readonly NavigationManager _nav;

        /// 注入 NavigationManager
        public NavigationHelper(NavigationManager nav)
        {
            _nav = nav;
        }

        /// <summary>
        /// 跳转到指定路径（对标 Vue：router.push / router.replace）
        /// </summary>
        /// <param name="path">目标路径（例如 "/login/sms"）</param>
        /// <param name="replace">true = 替换当前历史记录；false = push</param>
        public void GoTo(string path, bool replace = false)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            var currentUri = _nav.Uri;
            var currentPath = new Uri(currentUri).PathAndQuery;

            // 防止跳转到当前路径
            if (string.Equals(currentPath, path, StringComparison.OrdinalIgnoreCase))
                return;

            _nav.NavigateTo(path, replace);
        }
    }
}
