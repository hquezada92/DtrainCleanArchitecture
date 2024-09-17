using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task AddAsync(Subscription subscription);
    Task<Subscription?> GetByIdAsync(Guid id);
}