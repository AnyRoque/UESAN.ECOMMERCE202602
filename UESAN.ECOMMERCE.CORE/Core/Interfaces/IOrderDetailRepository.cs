using System.Collections.Generic;
using UESAN.ECOMMERCE.CORE.Core.Entities;

namespace UESAN.ECOMMERCE.CORE.Core.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<bool> CreateOrderDetail(OrderDetail orderDetail);
        Task<IEnumerable<OrderDetail>> GetDetailsByOrderId(int orderId);
    }
}
