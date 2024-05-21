using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RInterfaces
{
    public interface IAddress
    {
        public Task<bool> AddAddress(AddressEntity re_var);
        public Task<bool> DeleteAddress(int userId, long mobileNumber);
        public Task<int> UpdateAddress(int userId, long mobileNumber, string address);
        public Task<IEnumerable<object>> GetCustomerDetails();



    }
}
