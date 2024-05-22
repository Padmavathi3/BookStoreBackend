using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RServices
{
    public class WishlistService:IWishlist
    {
        private readonly DapperContext _context;
        public WishlistService(DapperContext context) 
        {
            _context = context;
        }
        //----------------------------------------
        public async Task<bool> AddToWishlist(WishlistEntity re_var)
        {
            var insertQuery = "INSERT INTO Wishlist (UserId, BookId) VALUES (@UserId, @BookId)";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", re_var.UserId);
            parameters.Add("@BookId", re_var.BookId);


            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, parameters);
                return true;
            }
        }

        public async Task<bool> DeleteBook(int userId, int bookId)
        {
            var checkQuery = "SELECT COUNT(*) FROM Wishlist WHERE UserId = @UserId AND BookId = @BookId";
            var delete_book_query = "DELETE FROM Wishlist WHERE UserId = @UserId AND BookId = @BookId";

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

        public async Task<IEnumerable<object>> GetAllProducts()
        {
            var query = "SELECT W.ProductId,U.UserId, B.BookId, B.BookName, B.BookImage, B.AuthorName, B.Price FROM Wishlist AS W INNER JOIN Books AS B ON W.BookId = B.BookId INNER JOIN Users AS U ON W.UserId = U.UserId;";

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
        

    }
}
