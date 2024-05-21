using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RInterfaces
{
    public interface ICart
    {
        //AddBookin cart
        public Task<int> AddToCart(CartEntity re_var);

        //Get
        public Task<IEnumerable<Object>> GetAllCarts();
        //update quantity
        public Task<int> UpdateQuantity(int bookId, int quantity);

        public Task<bool> DeleteBook(int userId, int bookId);
    }
}
