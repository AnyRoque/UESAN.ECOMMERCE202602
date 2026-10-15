using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.ECOMMERCE.CORE.Core.DTOs;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;

namespace UESAN.ECOMMERCE.CORE.Core.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<IEnumerable<FavoriteUserDTO>> GetFavorites()
        {
            var favorites = await _favoriteRepository.GetFavorites();

            var grouped = favorites
                .Where(f => f.User != null && f.Product != null)
                .GroupBy(f => new { f.User!.Id, FullName = (f.User!.FirstName ?? "") + " " + (f.User!.LastName ?? "") })
                .Select(g => new FavoriteUserDTO
                {
                    UserId = g.Key.Id,
                    FullName = g.Key.FullName.Trim(),
                    Products = g
                        .Select(x => new FavoriteProductDTO { Id = x.Product!.Id, Description = x.Product!.Description ?? string.Empty })
                        .DistinctBy(p => p.Id)
                        .ToList()
                })
                .ToList();

            return grouped;
        }

        public async Task<FavoriteUserDTO> GetFavoritesByUserId(int userId)
        {
            var favorites = await _favoriteRepository.GetFavoritesByUserId(userId);
            var list = favorites
                .Where(f => f.User != null && f.Product != null)
                .Select(x => new FavoriteProductDTO { Id = x.Product!.Id, Description = x.Product!.Description ?? string.Empty })
                .DistinctBy(p => p.Id)
                .ToList();

            var user = favorites.Select(f => f.User).FirstOrDefault();
            if (user == null) return null!;

            return new FavoriteUserDTO
            {
                UserId = user.Id,
                FullName = ((user.FirstName ?? "") + " " + (user.LastName ?? "")).Trim(),
                Products = list
            };
        }

        public async Task<bool> CreateFavorite(FavoriteCreateDTO favoriteCreateDTO)
        {
            var favorite = new UESAN.ECOMMERCE.CORE.Core.Entities.Favorite
            {
                UserId = favoriteCreateDTO.UserId,
                ProductId = favoriteCreateDTO.ProductId
                // CreatedAt left null so DB server default can be used
            };

            return await _favoriteRepository.CreateFavorite(favorite);
        }

        public async Task<bool> DeleteFavorite(FavoriteDeleteDTO favoriteDeleteDTO)
        {
            return await _favoriteRepository.DeleteFavorite(favoriteDeleteDTO.UserId, favoriteDeleteDTO.ProductId);
        }
    }
}
