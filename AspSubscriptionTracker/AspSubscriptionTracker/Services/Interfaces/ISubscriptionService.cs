using Models;

namespace AspSubscriptionTracker.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task AddAsync(Subscription sub);
        public Task<List<Subscription>> ViewAllAsync();
    }
}
