using OrderManagement.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderRepository : Domain.Interfaces.IRepository<Order>
    {
        Task<Order?> GetWithItemsAsync(Guid id);
    }
}
