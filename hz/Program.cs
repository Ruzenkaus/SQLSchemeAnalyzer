using hz.Classes;
using hz.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<SchemeModel>();
        builder.Services.AddScoped<SQLSchemeAnalyzer>();

        var app = builder.Build();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}"
            );

        app.Run();
    }
}