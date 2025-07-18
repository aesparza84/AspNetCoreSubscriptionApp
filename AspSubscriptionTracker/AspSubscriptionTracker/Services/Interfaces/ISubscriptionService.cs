using Models;

namespace AspSubscriptionTracker.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task<Guid> AddSubAsync(Subscription sub);
        public Task<List<Subscription>>? ViewAllAsync();
        public Task<Subscription>? FindAsync(Guid? id);
        public Task<bool> Update(Subscription sub);
        public Task DeleteSub(Guid trackingId);
    }
}
