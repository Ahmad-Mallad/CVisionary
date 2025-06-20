using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CVisionary.Data;
using CVisionary.Models;
using CVisionary.Repositories.Interfaces;
using CVisionary.Repositories.Repos;
using Microsoft.SemanticKernel;
using CVisionary.Services;
using Rotativa.AspNetCore;

namespace CVisionary;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddTransient<IResumeRepository, ResumeRepository>();
        builder.Services.AddTransient<IPortfolioRepository, PortfolioRepository>();
        builder.Services.AddTransient<IServiceRepository, ServiceRepository>();



        var Key = builder.Configuration["OpenAi:Key"];
        builder.Services.AddSingleton<Kernel>(sp =>
        {
            var Kernelbuilder = Kernel.CreateBuilder();
            Kernelbuilder.AddOpenAIChatCompletion("gpt-4",Key);
            return Kernelbuilder.Build();
        });

        builder.Services.AddSingleton<ICVParser,CvParserService>();
        builder.Services.AddSingleton<IPortfolioParser, PortfolioParserService>();


        builder.Services.AddDefaultIdentity<Person>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        Rotativa.AspNetCore.RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
