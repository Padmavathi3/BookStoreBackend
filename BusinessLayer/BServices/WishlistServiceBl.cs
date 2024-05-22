using BusinessLayer.BInterfaces;
using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BServices
{
    public class WishlistServiceBl:IWishlistBl
    {
        private readonly IWishlist wishlist;
        public WishlistServiceBl(IWishlist wishlist)
        {
            this.wishlist = wishlist;
        }
        //-------------------------------------------------------------

        private WishlistEntity MapToEntity(WishlistRequest request)
        {
            return new WishlistEntity { UserId = request.UserId, BookId = request.BookId};
        }
        public Task<bool> AddToWishlist(WishlistRequest re_var)
        {
           return wishlist.AddToWishlist(MapToEntity(re_var));   
        }

        public Task<bool> DeleteBook(int userId, int bookId)
        {
            return wishlist.DeleteBook(userId, bookId);
        }

        public Task<IEnumerable<object>> GetAllProducts()
        {
            return wishlist.GetAllProducts();
        }
    }
}
