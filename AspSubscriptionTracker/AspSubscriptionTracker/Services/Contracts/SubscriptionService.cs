using AspSubscriptionTracker.Models;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AspSubscriptionTracker.Services.Contracts
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly SubscriptionContext subContext;

        public SubscriptionService(SubscriptionContext ctx)
        {
            subContext = ctx;

            if (subContext == null)
                Console.WriteLine("subContext is null");
        }
        public async Task AddSubAsync(Subscription sub)
        {
            await subContext.AddAsync(sub);
            await subContext.SaveChangesAsync();
        }

        public async Task<List<Subscription>> ViewAllAsync()
        {
            List<Subscription> list = await subContext.Subscriptions.ToListAsync();

            return list;
        }
    }
}
