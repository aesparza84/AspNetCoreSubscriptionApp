using AspSubscriptionTracker.Models;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AspSubscriptionTracker.Services.Contracts
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext subContext;

        public SubscriptionService(ApplicationDbContext ctx)
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


            //await subContext.AddAsync(sub);
            await subContext.Subscriptions.AddAsync(sub);
            await subContext.SaveChangesAsync();

            int count = subContext.Subscriptions.Count();
            Console.WriteLine("Sub is valid, Added");
            return true;
        }

        public async Task<Subscription>? FindAsync(Guid? id)
        {
            return await subContext.Subscriptions.FirstOrDefaultAsync(sub => sub.Id == id);
        }

        public bool Update(Subscription sub)
        {   
            //Grab the element we are updating
            var dbSub = subContext.Subscriptions.FirstOrDefault(sub => sub.Id == sub.Id);

            //Email Changed
            if (sub.Email != dbSub.Email)
            {
                //Confirm that email doesn't belong to same subscription name
                if (subContext.Subscriptions.Any(e => e.Email == sub.Email && e.Name == sub.Name))
                    return false;
            }

            if (dbSub.Name != sub.Name)
                dbSub.Name = sub.Name;
            
            if (dbSub.Email != sub.Email)
                dbSub.Price = sub.Price;
            
            if (dbSub.Email != sub.Email)
                dbSub.Email = sub.Email;
            
            if (dbSub.Category != sub.Category)
                dbSub.Category = sub.Category;
           
            if (dbSub.PurchaseDate != sub.PurchaseDate)
                dbSub.PurchaseDate = sub.PurchaseDate;
            
            if (dbSub.RenewalType != sub.RenewalType)
            {
                dbSub.RenewalType = sub.RenewalType;
                dbSub.SetNextRenewalDate();
            }
             
            //subContext.Entry(dbSub).State = EntityState.Modified;
            subContext.Update(dbSub);
            subContext.SaveChanges();
            return true;
        }

        public async Task<List<Subscription>>? ViewAllAsync()
        {
            List<Subscription> list = await subContext.Subscriptions.ToListAsync();

            return list;
        }
    }
}
