using Ghtk.Repository.Abstractions.Entities;

namespace Ghtk.Repository;

public interface IOrderRepository
{
    Task<bool> CancelOrderAsync(string trackingId, string partnerId);
    Task CreateOrderAsync(Order orderEntity);
    Task<Order> FindOrderAsync(string id, string partnerId);
}
