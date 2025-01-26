using Ghtk.Repository.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghtk.Repository;

public interface IOrderRepository
{
    Task<bool> CancelOrderAsync(string trackingId, string partnerId);
    Task CreateOrderAsync(Order orderEntity);
    Task<Order> FindOrderAsync(string id, string partnerId);
}
