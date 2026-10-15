using System.Collections.Generic;
using UESAN.ECOMMERCE.CORE.Core.Entities;

namespace UESAN.ECOMMERCE.CORE.Core.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<Favorite>> GetFavorites();
        Task<IEnumerable<Favorite>> GetFavoritesByUserId(int userId);
        Task<bool> CreateFavorite(Favorite favorite);
        Task<bool> DeleteFavorite(int userId, int productId);
    }
}
