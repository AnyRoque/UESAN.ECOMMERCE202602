using System.Collections.Generic;

namespace UESAN.ECOMMERCE.CORE.Core.DTOs
{
    public class FavoriteProductDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class FavoriteUserDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public List<FavoriteProductDTO> Products { get; set; } = new List<FavoriteProductDTO>();
    }

    public class FavoriteCreateDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class FavoriteDeleteDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
