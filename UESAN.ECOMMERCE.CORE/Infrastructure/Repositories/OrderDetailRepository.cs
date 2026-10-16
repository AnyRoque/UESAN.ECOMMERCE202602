using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.ECOMMERCE.CORE.Core.Entities;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Infrastructure.Data;

namespace UESAN.ECOMMERCE.CORE.Infrastructure.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly StoreDbContext _dbContext;

        public OrderDetailRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateOrderDetail(OrderDetail orderDetail)
        {
            await _dbContext.OrderDetail.AddAsync(orderDetail);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<IEnumerable<OrderDetail>> GetDetailsByOrderId(int orderId)
        {
            var details = await _dbContext
                        .OrderDetail
                        .Where(d => d.OrdersId == orderId)
                        .Include(d => d.Product)
                        .ToListAsync();
            return details;
        }
    }
}
