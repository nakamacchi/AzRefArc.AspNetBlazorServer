using AzRefArc.AspNetBlazorServer.Components;
using AzRefArc.AspNetBlazorServer.Data;
using Microsoft.EntityFrameworkCore;

namespace AzRefArc.AspNetBlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // global, interactive server �Ńv���W�F�N�g�𐶐�

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // ��O���O�̃t�@�C���o�͋@�\�̒ǉ�
            builder.Logging.AddProvider(new ExceptionFileLoggerProvider());

            // DB �T�[�r�X�o�^
            // AddDbContext() �� Blazor Server �ł͎g���Ă͂����Ȃ� (Scoped �ɂȂ�)�AAddDbContextFactory() ���g��
            // https://docs.microsoft.com/ja-jp/aspnet/core/blazor/blazor-server-ef-core
            builder.Services.AddDbContextFactory<PubsDbContext>(opt =>
            {
                // DbContext �\���ݒ�
                // https://docs.microsoft.com/ja-jp/ef/core/dbcontext-configuration/#other-dbcontext-configuration
                if (builder.Environment.IsDevelopment())
                {
                    opt = opt.EnableSensitiveDataLogging().EnableDetailedErrors();
                }
                opt.UseSqlServer(
                    builder.Configuration.GetConnectionString("PubsDbContext"),
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

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
