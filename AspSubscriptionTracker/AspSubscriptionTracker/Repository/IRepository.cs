using Models;

namespace AspSubscriptionTracker.Repository
{
    public interface ISubcriptionRepository
    {
        public Task<Guid> Create(Subscription sub);
        public Task<List<Subscription>> GetAllSubscriptions();
        public Task<Subscription> GetSubscription(Guid id);
        public void Update(Subscription sub);
        public Task Delete(Guid id);
    }
}
