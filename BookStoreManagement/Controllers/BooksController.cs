using BusinessLayer.BInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO.Request;
using ModelLayer.DTO.Response;
using ModelLayer.Entities;
using RepositoryLayer.CustomExceptions;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookBl _books;

        public BooksController(IBookBl books)
        {
            _books = books;

        }
        //-----------------------------------------------------------------------------------------------------

        [HttpPost("AddBooks")]
        public async Task<IActionResult> AddBooks(BookRequest requestDto)
        {
            try
            {

                int rowsAffected = await _books.AddBook(requestDto);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "Book Added successfully",
                        Data = requestDto
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to add book",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    //Message = ex.StackTrace,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //-----------------------------------------------------------
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _books.GetAllBooks();

                if (books.Any())
                {
                    var response = new ResponseModel<IEnumerable<Object>>
                    {
                        Success = true,
                        Message = "Books retrieved successfully",
                        Data = books
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<Object>>
                    {
                        Success = false,
                        Message = "No users found",
                        Data = null
                    };

                    return NotFound(response);
                }
            }
            catch (EmptyListException)
            {
                var response = new ResponseModel<IEnumerable<Object>>
                {
                    Success = false,
                    Message = "Books List is Empty",
                    Data = null
                };

                return NotFound(response);
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

        //----------------------------------------------------------------------------------------------------------------------------------------------

    }
}
