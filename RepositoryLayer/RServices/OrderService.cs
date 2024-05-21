using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RServices
{
    public class OrderService:IOrder
    {
        private readonly DapperContext _context;
        public OrderService(DapperContext context)
        {
            _context = context;
        }
        //--------------------------------------------------
        public async Task<bool> AddOrder(OrderEntity order)
        {
            var insertQuery = "INSERT INTO Orders (UserId, BookId, AddressId, OrderDate) VALUES (@UserId, @BookId, @AddressId, @OrderDate)";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", order.UserId);
            parameters.Add("@BookId", order.BookId);
            parameters.Add("@AddressId", order.AddressId);
            parameters.Add("@OrderDate", FormatDate(order.OrderDate),DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, parameters);
                return true;
            }
        }

        private static string FormatDate(DateTime input)
        {
            return input.ToString("MMMM d");
        }

        public async Task<IEnumerable<object>> GetOrder()
        {
            var query = @"
                SELECT 
                    O.OrderId,
                    U.UserId, 
                    U.Email,
                    A.AddressId,
                    A.Name AS CustomerName,
                    A.MobileNumber, 
                    A.FullAddress, 
                    B.BookId,
                    B.BookName,
                    B.AuthorName,
                    O.OrderDate
                FROM 
                    Orders AS O
                INNER JOIN 
                    Users AS U ON O.UserId = U.UserId
                INNER JOIN 
                    Address AS A ON O.AddressId = A.AddressId
                INNER JOIN 
                    Books AS B ON O.BookId = B.BookId";

            using (var connection = _context.CreateConnection())
            {
                var orders = await connection.QueryAsync<object>(query);
                return orders;
            }
        }
    }
}
