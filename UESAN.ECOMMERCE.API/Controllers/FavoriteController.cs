using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Core.DTOs;

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

        private int GetUserIdFromToken()
        {
            var claim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(claim)) throw new UnauthorizedAccessException("UserId claim missing");
            return int.Parse(claim);
        }
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            int userId;
            try
            {
                userId = GetUserIdFromToken();
            }
            catch
            {
                return Unauthorized();
            }

            var result = await _favoriteService.GetFavoritesByUserId(userId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFavorite([FromBody] FavoriteCreateDTO favoriteCreateDTO)
        {
            int userId;
            try
            {
                userId = GetUserIdFromToken();
            }
            catch
            {
                return Unauthorized();
            }

            favoriteCreateDTO.UserId = userId;
            var result = await _favoriteService.CreateFavorite(favoriteCreateDTO);
            if (!result) return BadRequest();
            return Created();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteFavorite(int productId)
        {
            int userId;
            try
            {
                userId = GetUserIdFromToken();
            }
            catch
            {
                return Unauthorized();
            }

            var dto = new FavoriteDeleteDTO { UserId = userId, ProductId = productId };
            var result = await _favoriteService.DeleteFavorite(dto);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
