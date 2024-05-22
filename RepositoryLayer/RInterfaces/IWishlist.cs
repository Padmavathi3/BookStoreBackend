using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RInterfaces
{
    public interface IWishlist
    {
        Task<bool> AddToWishlist(WishlistEntity re_var);
        Task<IEnumerable<Object>> GetAllProducts();
        public Task<bool> DeleteBook(int userId, int bookId);

    }
}
