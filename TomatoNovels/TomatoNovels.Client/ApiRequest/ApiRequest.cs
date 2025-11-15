using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;
using TomatoNovels.Shared.ApiResponse; // 使用 Shared 中统一的响应模型

namespace TomatoNovels.Client.ApiRequest
{
    // ===========================================
    // HTTP 请求封装（等价 axios）
    // ===========================================
    public class ApiRequest
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public ApiRequest(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;

            _http.Timeout = TimeSpan.FromSeconds(5);

            // 改成你后端的 host
            _http.BaseAddress = new Uri("https://localhost:7139/api/");
        }

        // ================================
        // 从 localStorage 获取 token
        // ================================
        private async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", "token");
        }

        // ================================
        // 自动附带 Authorization 头
        // ================================
        private async Task AttachTokenAsync()
        {
            var token = await GetTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ================================
        // GET
        // ================================
        public async Task<ApiResponse<T>> GetAsync<T>(string url)
        {
            await AttachTokenAsync();

            var res = await _http.GetAsync(url);
            return await HandleResponse<T>(res);
        }

        // ================================
        // POST JSON
        // ================================
        public async Task<ApiResponse<T>> PostAsync<T>(string url, object? body = null)
        {
            await AttachTokenAsync();

            HttpResponseMessage res;

            if (body == null)
                res = await _http.PostAsync(url, null);
            else
                res = await _http.PostAsJsonAsync(url, body);

            return await HandleResponse<T>(res);
        }

        // ================================
        // POST 表单（上传）
        // ================================
        public async Task<ApiResponse<T>> PostFormAsync<T>(string url, MultipartFormDataContent form)
        {
            await AttachTokenAsync();

            var res = await _http.PostAsync(url, form);
            return await HandleResponse<T>(res);
        }

        // ================================
        // 响应统一处理（等价 axios 拦截器）
        // ================================
        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage res)
        {
            var text = await res.Content.ReadAsStringAsync();

            var api = JsonSerializer.Deserialize<ApiResponse<T>>(
                text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (api == null)
                throw new Exception("服务器返回格式错误");

            if (api.Success)
                return api;

            throw new Exception(api.Error?.Message ?? "请求失败");
        }
    }
}
