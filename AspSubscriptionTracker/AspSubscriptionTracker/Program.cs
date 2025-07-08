namespace AspSubscriptionTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            //Add Controllers
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            //Enable the components
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(en => { en.MapControllers(); });

            app.Run();
        }
    }
}
