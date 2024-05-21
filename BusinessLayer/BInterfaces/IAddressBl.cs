using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BInterfaces
{
    public interface IAddressBl
    {
        public Task<bool> AddAddress(AddressRequest re_var);
        public Task<bool> DeleteAddress(int userId, long mobileNumber);
        public Task<int> UpdateAddress(int userId, long mobileNumber, string address);
        public Task<IEnumerable<object>> GetCustomerDetails();
    }
}
