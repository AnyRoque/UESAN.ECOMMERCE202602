using System.Collections.Generic;
using UESAN.ECOMMERCE.CORE.Core.DTOs;

namespace UESAN.ECOMMERCE.CORE.Core.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteUserDTO>> GetFavorites();
        Task<FavoriteUserDTO> GetFavoritesByUserId(int userId);
        Task<bool> CreateFavorite(FavoriteCreateDTO favoriteCreateDTO);
        Task<bool> DeleteFavorite(FavoriteDeleteDTO favoriteDeleteDTO);
    }
}
