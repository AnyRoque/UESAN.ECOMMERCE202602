using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using UESAN.ECOMMERCE.CORE.Core.DTOs;

namespace UESAN.ECOMMERCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IChatClient _chatClient;

        public AIController(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequestDTO request)
        { 
            if(string.IsNullOrWhiteSpace(request?.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            var response = await _chatClient.GetResponseAsync(request.Message);

            return Ok(new ChatResponseDTO { Response = response.Text, Model = response.ModelId });

        }
    }
}
