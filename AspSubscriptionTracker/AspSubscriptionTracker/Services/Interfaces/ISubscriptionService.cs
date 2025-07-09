using Models;

namespace AspSubscriptionTracker.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task AddSubAsync(Subscription sub);
        public Task<List<Subscription>> ViewAllAsync();
    }
}
