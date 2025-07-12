using Microsoft.EntityFrameworkCore;
using Models;

namespace AspSubscriptionTracker.Models
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
