using AzRefArc.AspNetBlazorServer.BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

// ※ 既定でインストールされている System.Drawing.Common.dll のバージョンが古く（5.0.0）、コンテナスキャンでエラーが出るため、
// アプリでは利用していないが System.Drawing.Common.dll の 7.0.0 の NuGet パッケージを参照している
Console.WriteLine(Assembly.Load("System.Drawing.Common").FullName);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// AddDbContext() は Blazor Server では使ってはいけない (Scoped になる)、AddDbContextFactory() を使う
// https://docs.microsoft.com/ja-jp/aspnet/core/blazor/blazor-server-ef-core
builder.Services.AddDbContextFactory<PubsEntities>(opt =>
{
    // DbContext 構成設定
    // https://docs.microsoft.com/ja-jp/ef/core/dbcontext-configuration/#other-dbcontext-configuration
    if (builder.Environment.IsDevelopment())
    {
        opt = opt.EnableSensitiveDataLogging().EnableDetailedErrors();
    }
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("PubsEntities"),
        providerOptions =>
        {
            providerOptions.EnableRetryOnFailure();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) => {
    try
    {
        await next.Invoke();
    }
    catch (Exception error)
    {
        app.Logger.LogError(error, "パイプライン処理で未処理例外が発生しました。");
        throw;
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
