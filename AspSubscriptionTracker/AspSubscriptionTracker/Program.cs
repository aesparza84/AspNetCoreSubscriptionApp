using AspSubscriptionTracker.Models;
using AspSubscriptionTracker.Services.Contracts;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspSubscriptionTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            //Add Controllers
            builder.Services.AddControllersWithViews();

            //Add injection
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

            //Add the dbContext
            builder.Services.AddDbContext<SubscriptionContext>(options => 
            {
                //options.UseSqlServer("Server=MACHACITO;Database=SubscriptionDB;Trusted_Connection=True;TrustServerCertificate=True");

                options.EnableDetailedErrors();

                string connection = builder.Configuration.GetConnectionString("DefaultConnection");
                string connectionA = "Server=(localdb)\\MSSQLLocalDB;Database=SubscriptionsDb;Trusted_Connection=True;TrustServerCertificate=True;";
                string connectionB = "Server=MACHACITO;Database=SubscriptionDB;Trusted_Connection=True;TrustServerCertificate=True;";
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
