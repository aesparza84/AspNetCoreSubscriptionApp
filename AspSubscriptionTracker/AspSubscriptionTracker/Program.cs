using AspSubscriptionTracker.Models;
using AspSubscriptionTracker.Services.Contracts;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AspSubscriptionTracker.Repository;

namespace AspSubscriptionTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            //Add Controllers
            builder.Services.AddControllersWithViews();
            builder.Services.AddMemoryCache();

            //Add injections
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<ISubcriptionRepository, SubscriptionRepository>();

            //Add the dbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options => 
            {
                //options.UseSqlServer("Server=MACHACITO;Database=SubscriptionDB;Trusted_Connection=True;TrustServerCertificate=True");

                options.EnableDetailedErrors();

                string connection = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connection);
            });

            var app = builder.Build();

            //Enable the components
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(en => { en.MapControllers(); });

            app.Run();
        }
    }
}
