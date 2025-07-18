using AspSubscriptionTracker.Models;
using AspSubscriptionTracker.Repository;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AspSubscriptionTracker.Services.Contracts
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubcriptionRepository repository;

        public SubscriptionService(ISubcriptionRepository subRepo)
        {
            repository = subRepo;
        }
        public async Task<Guid> AddSubAsync(Subscription sub)
        {
            //Repository here
            List<Subscription> subList = await repository.GetAllSubscriptions();
            if (subList.Any(e => e.Name == sub.Name && e.Email == sub.Email))
            {
                Console.WriteLine("This subscription exists on this email");
                return Guid.Empty;
            }

            //Repository here
            await repository.Create(sub);


            Console.WriteLine("Sub is valid, Added");
            return sub.Id;
        }


        public async Task<Subscription>? FindAsync(Guid? id)
        {
            try
            {
                //Repository here
                List<Subscription> subList = await repository.GetAllSubscriptions();

                return subList.FirstOrDefault(sub => sub.Id == id);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task DeleteSub(Guid trackingId)
        {
            await repository.Delete(trackingId);
        }

        public async Task<bool> Update(Subscription sub)
        {   
            List<Subscription> subList = await repository.GetAllSubscriptions();

            var dbSub = subList.FirstOrDefault(d => d.Id == sub.Id);

            if (sub.Email != dbSub.Email)
            {
                //Confirm that email doesn't belong to same subscription name
                if (subList.Any(e => e.Email == sub.Email && e.Name == sub.Name))
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
             
            //Repository here
            repository.Update(dbSub);

            return true;
        }

        public async Task<List<Subscription>>? ViewAllAsync()
        {
            //Repository here
            List<Subscription> list = await repository.GetAllSubscriptions();

            return list;
        }
    }
}
