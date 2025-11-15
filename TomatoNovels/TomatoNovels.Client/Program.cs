using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// 注册 HttpClient，BaseAddress 会自动来自 Host 项目的 URL
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
);

// 注册状态、事件等（可选）
// builder.Services.AddSingleton<AppState>();

// 注册根组件

await builder.Build().RunAsync();
