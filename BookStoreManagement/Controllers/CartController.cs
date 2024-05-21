using BusinessLayer.BInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.DTO.Request;
using ModelLayer.DTO.Response;
using RepositoryLayer.CustomExceptions;
using System.Security.Claims;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBl cartBl;
        public CartController(ICartBl cartBl)
        {
            this.cartBl = cartBl;
        }

        //-----------------------------------------------------------------------------------------------------

        [HttpPost("AddBookToCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart(CartRequest requestDto)
        {
            requestDto.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {

                int rowsAffected = await cartBl.AddToCart(requestDto);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Book Added in caert successfully",
                        Data = requestDto
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to add book in cart",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.StackTrace,
                    //Message = ex.Message,
                    Data = ex.Message
                });
            }
        }


        [HttpGet("GetAllcarts")]
        [Authorize]
        public async Task<IActionResult> GetAllCartBooks()
        {
            try
            {
                var books = await cartBl.GetAllCarts();

                if (books.Any())
                {
                    var response = new ResponseModel<IEnumerable<Object>>
                    {
                        Success = true,
                        Message = "cart Books retrieved successfully",
                        Data = books
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<Object>>
                    {
                        Success = false,
                        Message = "No book found in cart",
                        Data = null
                    };

                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<Object>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

                return NotFound(response);
            }
        }

        [HttpPatch("UpdateQuantity")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int bookId,int quantity)
        {
            try
            {
                int rowsAffected = await cartBl.UpdateQuantity(bookId, quantity);

                if (rowsAffected > 0)
                {
                    
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "quantity updated successfully",
                        Data = null
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to update quantity",
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

        [HttpDelete("DeleteBook")]
        [Authorize]
        public async Task<IActionResult> Deletebook(int bookId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                bool result = await cartBl.DeleteBook(userId, bookId);

                if (result)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Book deleted from cart successfully",
                        Data = bookId
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Book not deleted from the cart successfully",
                        Data = bookId
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

        //-----------------------------------------------------------
    }
}
