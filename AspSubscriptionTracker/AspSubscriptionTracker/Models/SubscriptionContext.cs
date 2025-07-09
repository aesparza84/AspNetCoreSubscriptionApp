using Microsoft.EntityFrameworkCore;
using Models;

namespace AspSubscriptionTracker.Models
{
    public class SubscriptionContext : DbContext
    {
        public DbSet<Subscription> Subscriptions { get; set; }

        public SubscriptionContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
