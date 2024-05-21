using BusinessLayer.BInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTO.Request;
using ModelLayer.DTO.Response;
using ModelLayer.Entities;
using RepositoryLayer.CustomExceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBl _users;
        private readonly IConfiguration configuration;
        public UserController(IUserBl users, IConfiguration configuration)
        {
            _users = users;
            this.configuration = configuration;
        }
        //---------------------------------------------------
        [HttpPost("SignUp")]
        public async Task<IActionResult> registerUser(UserRequest requestDto)
        {
            try
            {
                // Inserting user details into the database
                int rowsAffected = await _users.SignUp(requestDto);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "User registered successfully",
                        Data = requestDto
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to register user",
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                var users = await _users.GetUsers();

                if (users.Any())
                {
                    var response = new ResponseModel<IEnumerable<UserEntity>>
                    {
                        Success = true,
                        Message = "Users retrieved successfully",
                        Data = users
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new ResponseModel<IEnumerable<UserEntity>>
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
                var response = new ResponseModel<IEnumerable<UserEntity>>
                {
                    Success = false,
                    Message = "Users List is Empty",
                    Data = null
                };

                return NotFound(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<IEnumerable<UserEntity>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };

                return NotFound(response);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("Login")]

        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var values = await _users.Login(email, password);
                UserEntity entity = values.FirstOrDefault();

                if (entity != null)
                {
                    string token = TokenGeneration(entity);
                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Login successful",
                        Data = token
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "User not found or invalid credentials",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Login not successful",
                    Data = ex.Message
                });
            }
        }

        //--------------------------------------------------------------------------------

        private string TokenGeneration(UserEntity entity)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(7);
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Email, entity.Email),
                 new Claim(ClaimTypes.NameIdentifier, entity.UserId.ToString())
                // Add additional claims if needed
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPut("ForgotPassword/{Email}")]

        public async Task<IActionResult> ChangePasswordRequest(string Email)
        {
            try
            {
                var result = await _users.ChangePasswordRequest(Email);
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "mail send successfully",
                    Data = result
                };

                return Ok(response);
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



        [HttpPut("ChangePassword/{otp}/{password}")]

        public async Task<IActionResult> ChangePassword(string otp, string password)
        {
            try
            {
                var res = await _users.ChangePassword(otp, password);

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = res,
                    Data = res
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------
    }
}
