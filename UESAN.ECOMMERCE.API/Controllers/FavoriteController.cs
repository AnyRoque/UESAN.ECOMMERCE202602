using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;

namespace UESAN.ECOMMERCE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var result = await _favoriteService.GetFavorites();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoritesByUserId(int id)
        {
            var result = await _favoriteService.GetFavoritesByUserId(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavorite([FromBody] UESAN.ECOMMERCE.CORE.Core.DTOs.FavoriteCreateDTO favoriteCreateDTO)
        {
            var result = await _favoriteService.CreateFavorite(favoriteCreateDTO);
            if (!result) return BadRequest();
            return Created();
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> DeleteFavorite(int userId, int productId)
        {
            var dto = new UESAN.ECOMMERCE.CORE.Core.DTOs.FavoriteDeleteDTO { UserId = userId, ProductId = productId };
            var result = await _favoriteService.DeleteFavorite(dto);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
