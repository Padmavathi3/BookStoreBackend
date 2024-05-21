using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RServices
{
    public class AddressService:IAddress
    {
         private DapperContext _context;
         public AddressService(DapperContext context)
        {
            _context = context; 
        }
        //--------------------------------------
        public async Task<bool> AddAddress(AddressEntity re_var)
        {
            var insertQuery = "INSERT INTO Address (UserId, Name, MobileNumber, FullAddress, City, State, Type) VALUES (@UserId, @Name, @MobileNumber, @FullAddress, @City, @State, @Type)";

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", re_var.UserId);
            parameters.Add("@Name", re_var.Name);
            parameters.Add("@MobileNumber", re_var.MobileNumber);
            parameters.Add("@FullAddress", re_var.FullAddress);
            parameters.Add("@City", re_var.City);
            parameters.Add("@State", re_var.State);
            parameters.Add("@Type", re_var.Type);

            using (var connection = _context.CreateConnection())
            {
                // Insert the address into the Address table
                await connection.ExecuteAsync(insertQuery, parameters);
                // Address added successfully, return true
                return true;
            }
        }
        //-----------------------------------------

        public async Task<bool> DeleteAddress(int userId, long mobileNumber)
        {
            var deleteAddressQuery = "DELETE FROM Address WHERE UserId = @UserId AND MobileNumber = @MobileNumber";

            using (var connection = _context.CreateConnection())
            {
                // Check if the address exists for the given userId and mobileNumber
                var checkQuery = "SELECT COUNT(1) FROM Address WHERE UserId = @UserId AND MobileNumber = @MobileNumber";
                int addressCount = await connection.ExecuteScalarAsync<int>(checkQuery, new { UserId = userId, MobileNumber = mobileNumber });

                if (addressCount == 0)
                {
                    return false; // Address does not exist
                }
                else
                {
                    await connection.ExecuteAsync(deleteAddressQuery, new { UserId = userId, MobileNumber = mobileNumber });
                    return true; // Address deleted successfully
                }
            }
        }
        //-----------------------------------------

        public async Task<int> UpdateAddress(int userId, long mobileNumber, string address)
        {
            var query = @"UPDATE Address 
                  SET FullAddress = @FullAddress
                  WHERE UserId = @UserId AND MobileNumber = @MobileNumber";

            var parameters = new DynamicParameters();
            parameters.Add("@FullAddress", address);
            parameters.Add("@UserId", userId);
            parameters.Add("@MobileNumber", mobileNumber);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
        //------------------------------------------
        public async Task<IEnumerable<object>> GetCustomerDetails()
        {
            var query = @"
        SELECT 
            U.UserId, 
            A.Name AS CustomerName,
            A.MobileNumber, 
            A.FullAddress, 
            A.City, 
            A.State, 
            A.Type
        FROM 
            Users AS U
            INNER JOIN Address AS A ON U.UserId = A.UserId";

            using (var connection = _context.CreateConnection())
            {
                var customerDetails = await connection.QueryAsync<object>(query);

                if (customerDetails.Any())
                {
                    return customerDetails;
                }
                else
                {
                    throw new EmptyListException("No book is present in the cart.");
                }
            }
        }


    }
}
