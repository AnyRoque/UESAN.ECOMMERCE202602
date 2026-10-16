using System.Collections.Generic;
using UESAN.ECOMMERCE.CORE.Core.DTOs;

namespace UESAN.ECOMMERCE.CORE.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(OrderCreateDTO orderCreateDTO);
        Task<IEnumerable<OrderDTO>> GetOrdersByUserId(int userId);
        Task<OrderDTO> GetOrderById(int id, int userId);
    }
}
