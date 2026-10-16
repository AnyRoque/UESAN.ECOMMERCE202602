using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.ECOMMERCE.CORE.Core.Entities;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Infrastructure.Data;

namespace UESAN.ECOMMERCE.CORE.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreDbContext _dbContext;

        public OrderRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateOrder(Orders order)
        {
            order.CreatedAt = order.CreatedAt ?? System.DateTime.Now;
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Orders> GetOrderById(int id)
        {
            var order = await _dbContext
                        .Orders
                        .Where(o => o.Id == id)
                        .Include(o => o.OrderDetail)
                        .ThenInclude(d => d.Product)
                        .FirstOrDefaultAsync();
            return order!;
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserId(int userId)
        {
            var orders = await _dbContext
                        .Orders
                        .Where(o => o.UserId == userId)
                        .Include(o => o.OrderDetail)
                        .ThenInclude(d => d.Product)
                        .ToListAsync();
            return orders;
        }
    }
}
