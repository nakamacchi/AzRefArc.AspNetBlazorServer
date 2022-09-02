using AzRefArc.AspNetBlazorServer.BlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// AddDbContext() �� Blazor Server �ł͎g���Ă͂����Ȃ� (Scoped �ɂȂ�)�AAddDbContextFactory() ���g��
// https://docs.microsoft.com/ja-jp/aspnet/core/blazor/blazor-server-ef-core
builder.Services.AddDbContextFactory<PubsEntities>(opt =>
{
    // DbContext �\���ݒ�
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
        app.Logger.LogError(error, "�p�C�v���C�������Ŗ�������O���������܂����B");
        throw;
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
