using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UESAN.ECOMMERCE.CORE.Core.DTOs;
using UESAN.ECOMMERCE.CORE.Core.Entities;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Infrastructure.Data;

namespace UESAN.ECOMMERCE.CORE.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly StoreDbContext _dbContext;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, StoreDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> CreateOrder(OrderCreateDTO orderCreateDTO)
        {
            using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = new Orders
                {
                    UserId = orderCreateDTO.UserId,
                    TotalAmount = orderCreateDTO.TotalAmount,
                    Status = "P",
                    CreatedAt = System.DateTime.Now
                };

                var orderId = await _orderRepository.CreateOrder(order);

                foreach (var d in orderCreateDTO.Details)
                {
                    var detail = new OrderDetail
                    {
                        OrdersId = orderId,
                        ProductId = d.ProductId,
                        Quantity = d.Quantity,
                        Price = d.Price,
                        CreatedAt = System.DateTime.Now
                    };

                    var ok = await _orderDetailRepository.CreateOrderDetail(detail);
                    if (!ok)
                        throw new System.Exception("Failed to create order detail");
                }

                await tx.CommitAsync();
                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                return false;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserId(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId);
            var list = orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                UserId = o.UserId ?? 0,
                CreatedAt = o.CreatedAt,
                TotalAmount = o.TotalAmount ?? 0,
                Status = o.Status,
                Details = o.OrderDetail?.Select(d => new OrderDetailDTO
                {
                    Id = d.Id,
                    ProductId = d.ProductId ?? 0,
                    Quantity = d.Quantity ?? 0,
                    Price = d.Price ?? 0,
                    Product = d.Product != null ? new ProductListDTO
                    {
                        Id = d.Product.Id,
                        Description = d.Product.Description,
                        Price = d.Product.Price ?? 0,
                        ImageUrl = d.Product.ImageUrl,
                        Stock = d.Product.Stock ?? 0,
                        Category = d.Product.Category != null ? new CategoryListDTO { Id = d.Product.Category.Id, Description = d.Product.Category.Description } : null
                    } : null
                }).ToList() ?? new List<OrderDetailDTO>()
            }).ToList();

            return list;
        }

        public async Task<OrderDTO> GetOrderById(int id, int userId)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null || order.UserId != userId) return null!;

            var dto = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId ?? 0,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount ?? 0,
                Status = order.Status,
                Details = order.OrderDetail?.Select(d => new OrderDetailDTO
                {
                    Id = d.Id,
                    ProductId = d.ProductId ?? 0,
                    Quantity = d.Quantity ?? 0,
                    Price = d.Price ?? 0,
                    Product = d.Product != null ? new ProductListDTO
                    {
                        Id = d.Product.Id,
                        Description = d.Product.Description,
                        Price = d.Product.Price ?? 0,
                        ImageUrl = d.Product.ImageUrl,
                        Stock = d.Product.Stock ?? 0,
                        Category = d.Product.Category != null ? new CategoryListDTO { Id = d.Product.Category.Id, Description = d.Product.Category.Description } : null
                    } : null
                }).ToList() ?? new List<OrderDetailDTO>()
            };

            return dto;
        }
    }
}
