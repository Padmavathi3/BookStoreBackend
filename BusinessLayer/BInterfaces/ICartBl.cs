using ModelLayer.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BInterfaces
{
    public interface ICartBl
    {
        //AddBookin cart
        public Task<int> AddToCart(CartRequest re_var);

        //Get
        public Task<IEnumerable<Object>> GetAllCarts();
        //update quantity
        public Task<int> UpdateQuantity(int bookId, int quantity);

        public  Task<bool> DeleteBook(int userId, int bookId);

    }
}
