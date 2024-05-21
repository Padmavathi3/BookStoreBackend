using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RInterfaces
{
    public interface IOrder
    {
        public Task<bool> AddOrder(OrderEntity re_var);
        public Task<IEnumerable<object>> GetOrder();
    }
}
