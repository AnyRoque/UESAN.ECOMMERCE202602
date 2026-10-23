using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Core.DTOs;
using System.Globalization;
using Microsoft.Extensions.AI;

namespace UESAN.ECOMMERCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IChatClient _chatClient;

        public OrderController(IOrderService orderService, IChatClient chatClient)
        {
            _orderService = orderService;
            _chatClient = chatClient;
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

        //My-Orders
        [HttpPost("my-orders")]
        public async Task<IActionResult> MyOrders([FromBody] MyOrdersRequestDTO request)
        {
            //AQ.Ab8RN6KQXhqak2emhjVE9SGLLC5Nghpmst-0XDCeqDyITfaCDg
            int userId;
            try { userId = GetUserIdFromToken(); } catch { return Unauthorized(); }

            var result = await _orderService.GetOrdersByUserId(userId);
            
            var context = string.Join("/n", result.Select(o => $"Pedido: {o.Id} - Fecha: {o.CreatedAt?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}" + 
             $" - Total: {o.TotalAmount}"));
            
            if(context.Length == 0)
            {
                context = "No tienes pedidos realizados.";
            }

            var messages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, "Eres un asistente de pedidos de e-commerce. Responde a las preguntas del usuario basándote en la información proporcionada."),
                new ChatMessage(ChatRole.User, $"Aquí está el contexto de los pedidos del usuario:\n{context}\n\nPregunta: {request.Message}")
            };

            var response = await _chatClient.GetResponseAsync(messages);

            return Ok(response);
        }

    }
}
