using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL.Data;

namespace Asp.Net_Core_MVC_CRUD_Operations_Using_PostgresSQL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("HealthCareDbContextPostgresSQL") ?? throw new InvalidOperationException("Connection string 'HealthCareDbContext' not found.");

            builder.Services.AddDbContext<HealthCareDbContext>(options => options.UseNpgsql(connectionString));


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Seed data for Physician and Specialization.
            await SeedData.MySeedData(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}