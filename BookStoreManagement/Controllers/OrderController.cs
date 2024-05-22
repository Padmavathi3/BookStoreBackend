using BusinessLayer.BInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.Request;
using ModelLayer.DTO.Response;
using ModelLayer.Entities;
using System.Security.Claims;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBl _orderBl;

        public OrderController(IOrderBl orderBl)
        {
            _orderBl = orderBl;
        }
        //----------------------------------------------
        [HttpPost("AddOrder")]
        [Authorize]
        public async Task<IActionResult> AddOrder(OrderRequest request)
        {
            request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                bool isSuccess = await _orderBl.AddOrder(request);
                if (isSuccess)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Order added successfully",
                        Data = request
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to add order",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet("GetAllOrders")]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {

            try
            {
                var orders = await _orderBl.GetOrder();
                if (orders.Any())
                {
                    return Ok(new ResponseModel<IEnumerable<object>>
                    {
                        Success = true,
                        Message = "Orders retrieved successfully",
                        Data = orders
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<object>>
                    {
                        Success = false,
                        Message = "No orders found",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<IEnumerable<object>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

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
    }
}
