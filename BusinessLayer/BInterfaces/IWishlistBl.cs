using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BInterfaces
{
    public interface IWishlistBl
    {
        Task<bool> AddToWishlist(WishlistRequest re_var);
        Task<IEnumerable<Object>> GetAllProducts();
        public Task<bool> DeleteBook(int userId, int bookId);
    }
}
