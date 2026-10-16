using System;
using System.Collections.Generic;

namespace UESAN.ECOMMERCE.CORE.Core.DTOs
{
    public class OrderDetailCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderCreateDTO
    {
        // UserId will be taken from token in controller/service
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailCreateDTO> Details { get; set; } = new List<OrderDetailCreateDTO>();
    }

    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public ProductListDTO? Product { get; set; }
    }

    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public List<OrderDetailDTO> Details { get; set; } = new List<OrderDetailDTO>();
    }
}
