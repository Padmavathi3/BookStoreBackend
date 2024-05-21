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
    public class CartServiceBl:ICartBl
    {
        private readonly ICart cart;


        public CartServiceBl(ICart cart)
        {
            this.cart = cart;
        }
        private CartEntity MapToEntity(CartRequest request)
        {
            return new CartEntity { UserId = request.UserId, BookId = request.BookId, Quantity = request.Quantity };
        }
        public Task<int> AddToCart(CartRequest re_var)
        {
            return cart.AddToCart(MapToEntity(re_var));
        }

        public Task<IEnumerable<Object>> GetAllCarts()
        {
            return cart.GetAllCarts();
        }

        public Task<int> UpdateQuantity(int bookId, int quantity) 
        {
            return cart.UpdateQuantity(bookId, quantity);    
        }

        public Task<bool> DeleteBook(int userId, int bookId)
        {
            return cart.DeleteBook(userId, bookId); 
        }
    }
}
