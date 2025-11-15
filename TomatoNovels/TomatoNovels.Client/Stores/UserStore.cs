using System.Text.Json;
using Microsoft.JSInterop;
using TomatoNovels.Shared.DTOs.Auth.Response;

namespace TomatoNovels.Client.Stores
{
    public class UserStore
    {
        private readonly IJSRuntime _js;

        private const string TokenKey = "token";
        private const string UserKey = "user";

        public string? Token { get; private set; }
        public UserInfoDto? User { get; private set; }

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        public event Action? OnChange;

        public UserStore(IJSRuntime js)
        {
            _js = js;
        }

        // 初始化：从 localStorage 里把状态拉回来
        public async Task InitializeAsync()
        {
            Token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

            var userJson = await _js.InvokeAsync<string?>("localStorage.getItem", UserKey);

            if (!string.IsNullOrWhiteSpace(userJson))
            {
                try
                {
                    User = JsonSerializer.Deserialize<UserInfoDto>(userJson);
                }
                catch
                {
                    // 解析失败就清掉本地脏数据，防止一直报错
                    User = null;
                    await _js.InvokeVoidAsync("localStorage.removeItem", UserKey);
                }
            }

            NotifyStateChanged();
        }

        // 登录/注册成功后调用（类似 pinia 里的 action）
        public async Task SetLoginAsync(LoginOrRegisterResponseDto dto)
        {
            Token = dto.Token;
            User = dto.User;

            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, Token);
            await _js.InvokeVoidAsync("localStorage.setItem", UserKey,
                JsonSerializer.Serialize(User));

            NotifyStateChanged();
        }

        // 退出登录
        public async Task LogoutAsync()
        {
            Token = null;
            User = null;

            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", UserKey);

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
