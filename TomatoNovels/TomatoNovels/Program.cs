using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TomatoNovels.Client.Services;
using TomatoNovels.Client.ApiRequest;
using TomatoNovels.Client.Utils;
using TomatoNovels.Client.Stores;
using TomatoNovels.Components;
using TomatoNovels.Core.Exceptions;
using TomatoNovels.Core.Auth;
using TomatoNovels.Core.Auth.Impl;
using TomatoNovels.Data;
using TomatoNovels.Services;
using TomatoNovels.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

//
// ===============================
// 0. 【新增】SECRET_KEY 自动加载（等价 Flask）
// ===============================
//

// 1) 尝试从环境变量读取（等价 os.getenv("SECRET_KEY")）
var secret = Environment.GetEnvironmentVariable("SECRET_KEY");

// 2) 如果没有 → 自动生成（等价 secrets.token_urlsafe(32)）
if (string.IsNullOrWhiteSpace(secret))
{
    var keyBytes = RandomNumberGenerator.GetBytes(64);
    secret = Convert.ToBase64String(keyBytes);
    Console.WriteLine($"[WARN] SECRET_KEY 未设置，已自动生成：{secret}");
}

// 3) 注入到配置，使 JwtTokenGenerator 可以读取
builder.Configuration["SECRET_KEY"] = secret;



//
// ===============================
// 1. 前端：Blazor 组件系统
// ===============================
//
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//
// ===============================
// 2. 后端：EF Core + MySQL
// ===============================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)));
});

builder.Services.AddHttpContextAccessor();

//
// ===============================
// 3. 后端：业务服务 DI
// ===============================
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookInfoService, BookInfoService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IWriterService, WriterService>();
builder.Services.AddScoped<IWriterInfoService, WriterInfoService>();
builder.Services.AddScoped<NavigationHelper>();
builder.Services.AddScoped<UserStore>();

//
// ===============================
// 4. 后端 Controller(JSON API)
// ===============================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// ===============================
// 5. 前端 Request.cs + AuthApi
// ===============================
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(nav.BaseUri)
    };
});

builder.Services.AddScoped<ApiRequest>();
builder.Services.AddScoped<AuthApi>();
builder.Services.AddScoped<HomeApi>();
builder.Services.AddScoped<ModuleApi>();

var app = builder.Build();

//
// ===============================
// 6. 中间件流水线
// ===============================
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// 提供静态文件（头像、封面等）
app.UseStaticFiles();

// 全局 API 异常捕获
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 防 CSRF
app.UseAntiforgery();

//
// ===============================
// 7. 路由映射
// ===============================
app.MapControllers();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TomatoNovels.Client._Imports).Assembly);

app.Run();
