using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RServices
{
    public class CartService:ICart
    {
        private readonly DapperContext _context;
        public CartService(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddToCart(CartEntity re_var)
        {
            var query = "INSERT INTO Cart (UserId, BookId, Quantity) VALUES (@UserId,@BookId,@Quantity)";

            var parameters = new DynamicParameters();

            parameters.Add("@UserId", re_var.UserId);
            parameters.Add("@BookId", re_var.BookId);
            parameters.Add("@Quantity", re_var.Quantity);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

         
        public async Task<IEnumerable<Object>> GetAllCarts()
        {
            var query = "SELECT U.UserId, B.BookId, B.BookName, B.BookImage, B.AuthorName, C.Quantity, B.Price FROM Cart AS C INNER JOIN Books AS B ON C.BookId = B.BookId INNER JOIN Users AS U ON C.UserId = U.UserId;";

            using (var connection = _context.CreateConnection())
            {
                var books = await connection.QueryAsync(query);

                if (books.Any())
                {
                    return books;
                }
                return Enumerable.Empty<object>();

            }
        }
   
        public async Task<int> UpdateQuantity(int bookId, int quantity)
        {
            var query = @"UPDATE Cart SET
                  Quantity = @Quantity
                  WHERE BookId = @BookId";

            //var parameters = new DynamicParameters();
            //parameters.Add("@Quantity", quanity);
            //parameters.Add("@BookId", bookId);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new {BookId = bookId, Quantity=quantity });
            }
        }

        public async Task<bool> DeleteBook(int userId, int bookId)
        {
            var checkQuery = "SELECT COUNT(*) FROM Cart WHERE UserId = @UserId AND BookId = @BookId";
            var delete_book_query = "DELETE FROM Cart WHERE UserId = @UserId AND BookId = @BookId";

            using (var connection = _context.CreateConnection())
            {
                // Check if the note exists for the given email
                int bookCount = await connection.ExecuteScalarAsync<int>(checkQuery, new { UserId = userId, BookId = bookId });

                if (bookCount < 0)
                {
                    return false;
                }
                else
                {
                    await connection.ExecuteAsync(delete_book_query, new { UserId = userId, BookId = bookId });
                    return true;
                }
            }
        }
    }
}
