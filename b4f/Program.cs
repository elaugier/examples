using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build();

        host.Run();
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure your services here
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        string exePath = Assembly.GetEntryAssembly().Location;
        string exeDirPath = Path.GetDirectoryName(exePath);
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        // Configure the SPA static file serving middleware for app1
        app.Map("/app1", app1 =>
        {
            var app1StaticFilesPath = Path.Combine(exeDirPath, "app1");
            var app1FileProvider = new PhysicalFileProvider(app1StaticFilesPath);
            var app1DefaultFileOptions = new DefaultFilesOptions();
            app1DefaultFileOptions.DefaultFileNames.Clear();
            app1DefaultFileOptions.DefaultFileNames.Add("index.html");
            app1.UseDefaultFiles(app1DefaultFileOptions);
            app1.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = app1FileProvider,
                RequestPath = "",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000,immutable");
                }
            });
        });

        // Configure the SPA static file serving middleware for app2
        app.Map("/app2", app2 =>
        {
            var app2StaticFilesPath = Path.Combine(exeDirPath, "app2");
            var app2FileProvider = new PhysicalFileProvider(app2StaticFilesPath);
            var app2DefaultFileOptions = new DefaultFilesOptions();
            app2DefaultFileOptions.DefaultFileNames.Clear();
            app2DefaultFileOptions.DefaultFileNames.Add("index.html");
            app2.UseDefaultFiles(app2DefaultFileOptions);
            app2.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = app2FileProvider,
                RequestPath = "",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000,immutable");
                }
            });
        });

        app.Run(async (context) =>
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Page not found");
        });
    }
}
