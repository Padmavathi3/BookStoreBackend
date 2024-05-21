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
    public class AddressController : ControllerBase
    {
        private readonly IAddressBl _addressBl;

        public AddressController(IAddressBl addressBl)
        {
            _addressBl = addressBl;
        }

        [HttpPost("AddAddress")]
        [Authorize]
        public async Task<IActionResult> AddAddress(AddressRequest request)
        {
            request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                bool isSuccess = await _addressBl.AddAddress(request);
                if (isSuccess)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Address added successfully",
                        Data = request
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to add address",
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

        [HttpDelete("DeleteAddress")]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(long mobileNumber)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                bool isSuccess = await _addressBl.DeleteAddress(userId, mobileNumber);
                if (isSuccess)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Address deleted successfully",
                        Data = null
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Address not found",
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

        [HttpPatch("UpdateAddress/{mobileNumber}/{fullAddress}")]
        [Authorize]
        public async Task<IActionResult> UpdateAddress(long mobileNumber, string fullAddress)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                int rowsAffected = await _addressBl.UpdateAddress(userId, mobileNumber, fullAddress);
                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Address updated successfully",
                        Data = null
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Address not found",
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

        [HttpGet("GetCustomerDetails")]
        [Authorize]
        public async Task<IActionResult> GetCustomerDetails()
        {
            try
            {
                var customerDetails = await _addressBl.GetCustomerDetails();
                if (customerDetails.Any())
                {
                    return Ok(new ResponseModel<IEnumerable<object>>
                    {
                        Success = true,
                        Message = "Customer details retrieved successfully",
                        Data = customerDetails
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<object>>
                    {
                        Success = false,
                        Message = "No customer details found",
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

    }
}
