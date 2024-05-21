using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BInterfaces
{
    public interface IOrderBl
    {
        public Task<bool> AddOrder(OrderRequest re_var);
        public Task<IEnumerable<object>> GetOrder();
    }
}
