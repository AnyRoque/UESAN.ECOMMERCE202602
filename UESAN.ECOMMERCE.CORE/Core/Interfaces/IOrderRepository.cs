using System.Collections.Generic;
using UESAN.ECOMMERCE.CORE.Core.Entities;

namespace UESAN.ECOMMERCE.CORE.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrder(Orders order);
        Task<Orders> GetOrderById(int id);
        Task<IEnumerable<Orders>> GetOrdersByUserId(int userId);
    }
}
