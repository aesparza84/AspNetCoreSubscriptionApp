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
        }
        public async Task<bool> AddSubAsync(Subscription sub)
        {
            List<Subscription> subList = subContext.Subscriptions.ToList();
            
            if (subList.Any(e => e.Name == sub.Name && e.Email == sub.Email))
            {
                Console.WriteLine("This subscription exists on this email");
                return false;
            }


            await subContext.AddAsync(sub);
            await subContext.SaveChangesAsync();
            
            Console.WriteLine("Sub is valid, Added");
            return true;
        }

        public async Task<List<Subscription>>? ViewAllAsync()
        {
            List<Subscription> list = await subContext.Subscriptions.ToListAsync();

            return list;
        }
    }
}
