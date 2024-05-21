using BusinessLayer.BInterfaces;
using ModelLayer.DTO.Request;
using ModelLayer.Entities;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BServices
{
    public class OrderServiceBl:IOrderBl
    {
        private readonly IOrder order;
        public OrderServiceBl(IOrder order) 
        {
            this.order = order;
        }
        //-------------------------------------
        private OrderEntity MapToEntity(OrderRequest request)
        {
            return new OrderEntity
            {
                UserId = request.UserId,
                BookId = request.BookId,
                AddressId = request.AddressId,
                OrderDate = request.OrderDate
            };
        }

        public Task<bool> AddOrder(OrderRequest re_var)
        {
            return order.AddOrder(MapToEntity(re_var));
        }

        public Task<IEnumerable<object>> GetOrder()
        {
           return order.GetOrder();
        }
       
    }
}
