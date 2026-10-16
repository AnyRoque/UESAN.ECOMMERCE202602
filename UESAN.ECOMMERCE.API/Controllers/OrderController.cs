using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Core.DTOs;

namespace UESAN.ECOMMERCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserIdFromToken()
        {
            var claim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("UserId claim missing");
            return int.Parse(claim);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderCreateDTO)
        {
            int userId;
            try { userId = GetUserIdFromToken(); } catch { return Unauthorized(); }

            orderCreateDTO.UserId = userId;
            var result = await _orderService.CreateOrder(orderCreateDTO);
            if (!result) return BadRequest();
            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            int userId;
            try { userId = GetUserIdFromToken(); } catch { return Unauthorized(); }

            var result = await _orderService.GetOrdersByUserId(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            int userId;
            try { userId = GetUserIdFromToken(); } catch { return Unauthorized(); }

            var result = await _orderService.GetOrderById(id, userId);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
