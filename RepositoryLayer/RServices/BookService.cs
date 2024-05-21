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
    public class BookService:IBook
    {
        private readonly DapperContext _context;

        public BookService(DapperContext context)
        {
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------
        public async Task<int> AddBook(BookEntity re_var)
        {
            var query = "INSERT INTO Books (BookName, BookImage, Description, AuthorName, Quantity, Price) VALUES (@BookName, @BookImage, @Description, @AuthorName, @Quantity, @Price)";

            var parameters = new DynamicParameters();

            parameters.Add("@BookName", re_var.BookName, DbType.String);
            parameters.Add("@BookImage", re_var.BookImage, DbType.String);
            parameters.Add("@Description", re_var.Description, DbType.String);
            parameters.Add("@AuthorName", re_var.AuthorName, DbType.String);
            parameters.Add("@Quantity", re_var.Quantity, DbType.Int32);
            parameters.Add("@Price", re_var.Price, DbType.Decimal);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
        //-------------------------------------------------------------------------------------------
        public async Task<IEnumerable<Object>> GetAllBooks()
        {
            var query = "SELECT * FROM Books";

            using (var connection = _context.CreateConnection())
            {
                var books = await connection.QueryAsync<Object>(query);

                if (books.Any())
                {
                    return books;
                }
                else
                {
                    throw new EmptyListException("No user is present in the table.");
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
