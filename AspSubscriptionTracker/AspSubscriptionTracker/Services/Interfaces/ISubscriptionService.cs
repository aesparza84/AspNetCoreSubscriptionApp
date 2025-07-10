using Models;

namespace AspSubscriptionTracker.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task<bool> AddSubAsync(Subscription sub);
        public Task<List<Subscription>>? ViewAllAsync();
    }
}
