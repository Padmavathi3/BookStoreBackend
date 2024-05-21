using BusinessLayer.BInterfaces;
using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BServices
{
    public class AddressServiceBl:IAddressBl
    {
        private IAddress _address;
        public AddressServiceBl(IAddress address)
        {
            _address = address;
        }
        //--------------------------------------
        private AddressEntity MapToEntity(AddressRequest request)
        {
            return new AddressEntity
            {
                UserId = request.UserId,
                Name = request.Name,
                MobileNumber = request.MobileNumber,
                FullAddress = request.FullAddress,
                City = request.City,
                State = request.State,
                Type = request.Type
            };
        }

        public Task<bool> AddAddress(AddressRequest re_var)
        {
            return _address.AddAddress(MapToEntity(re_var));
        }


        public Task<bool> DeleteAddress(int userId, long mobileNumber)
        {
            return _address.DeleteAddress(userId, mobileNumber);
        }

        public Task<IEnumerable<object>> GetCustomerDetails()
        {
           return _address.GetCustomerDetails();
        }

        public Task<int> UpdateAddress(int userId, long mobileNumber, string address)
        {
            return _address.UpdateAddress(userId, mobileNumber, address);
        }
       
    }
}
