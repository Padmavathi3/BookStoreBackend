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
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistBl wishlistBl;
        public WishlistController(IWishlistBl wishlistBl)
        {
            this.wishlistBl = wishlistBl;
        }
        //--------------------------------------------

        [HttpPost("AddToWishlist")]
        [Authorize]
        public async Task<IActionResult> AddToWishlist(WishlistRequest request)
        {
            request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                bool isSuccess = await wishlistBl.AddToWishlist(request);
                if (isSuccess)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Book added to wishlist successfully",
                        Data = request
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to add book to wishlist",
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
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetWishlist")]
        [Authorize]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
               
                var wishlist = await wishlistBl.GetAllProducts();

                if (wishlist.Any())
                {
                    return Ok(new ResponseModel<IEnumerable<Object>>
                    {
                        Success = true,
                        Message = "Wishlist retrieved successfully",
                        Data = wishlist
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<WishlistEntity>>
                    {
                        Success = false,
                        Message = "No items found in wishlist",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseModel<IEnumerable<WishlistEntity>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpDelete("RemoveFromWishlist/{bookId}")]
        public async Task<IActionResult> RemoveFromWishlist(int bookId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                bool isSuccess = await wishlistBl.DeleteBook(userId, bookId);
                if (isSuccess)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Item removed from wishlist successfully",
                        Data = bookId
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to remove item from wishlist",
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
                    Data = ex.Message
                });
            }
        }

    }
}
