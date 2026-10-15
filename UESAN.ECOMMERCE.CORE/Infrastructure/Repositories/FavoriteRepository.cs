using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UESAN.ECOMMERCE.CORE.Core.Entities;
using UESAN.ECOMMERCE.CORE.Core.Interfaces;
using UESAN.ECOMMERCE.CORE.Infrastructure.Data;

namespace UESAN.ECOMMERCE.CORE.Infrastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly StoreDbContext _dbContext;

        public FavoriteRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Favorite>> GetFavorites()
        {
            var favorites = await _dbContext
                                .Favorite
                                .Include(f => f.User)
                                .Include(f => f.Product)
                                .ToListAsync();
            return favorites;
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesByUserId(int userId)
        {
            var favorites = await _dbContext
                                .Favorite
                                .Where(f => f.UserId == userId)
                                .Include(f => f.User)
                                .Include(f => f.Product)
                                .ToListAsync();
            return favorites;
        }

        public async Task<bool> CreateFavorite(Favorite favorite)
        {
            // Do not set CreatedAt here so database default (server time) can apply if present
            await _dbContext.Favorite.AddAsync(favorite);
            var rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteFavorite(int userId, int productId)
        {
            var existing = await _dbContext
                               .Favorite
                               .Where(f => f.UserId == userId && f.ProductId == productId)
                               .FirstOrDefaultAsync();
            if (existing != null)
            {
                _dbContext.Favorite.Remove(existing);
                var rows = await _dbContext.SaveChangesAsync();
                return rows > 0;
            }
            return false;
        }
    }
}
