using AspSubscriptionTracker.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AspSubscriptionTracker.Repository
{
    public class SubscriptionRepository : ISubcriptionRepository
    {
        private readonly ApplicationDbContext dbContext;
        public SubscriptionRepository(ApplicationDbContext ctx)
        {
            dbContext = ctx;
        }

        public async Task<Guid> Create(Subscription sub)
        {
            dbContext.Subscriptions.Add(sub);
            await dbContext.SaveChangesAsync();
            return sub.Id;
        }

        public async Task Delete(Guid id)
        {
            Subscription s = await dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);

            dbContext.Subscriptions.Remove(s);
            await dbContext.SaveChangesAsync();
        }

        public Task<List<Subscription>> GetAllSubscriptions()
        {
            return dbContext.Subscriptions.ToListAsync();
        }

        public async Task<Subscription> GetSubscription(Guid id)
        {
            return await dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
        }

        public void Update(Subscription sub)
        {
            dbContext.Subscriptions.Update(sub);
            dbContext.SaveChanges();
        }
    }
}
