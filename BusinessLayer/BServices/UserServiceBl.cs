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
    public class UserServiceBl:IUserBl
    { 
        private readonly IUser _user;
        public UserServiceBl(IUser user)
        {
            _user = user;
        }
        public Task<string> ChangePassword(string otp, string password)
        {
            return _user.ChangePassword(otp, password);
        }

        public Task<string> ChangePasswordRequest(string Email)
        {
            return _user.ChangePasswordRequest(Email);
        }

        public Task<int> DeleteUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserEntity>> GetUsers()
        {
            return _user.GetUsers();
        }

        public Task<IEnumerable<UserEntity>> GetUsersByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserEntity>> Login(string email, string password)
        {
            return _user.Login(email, password);
        }
        //-------------------------------------------------------------------------------------------------------------------------------
        private UserEntity MapToEntity(UserRequest request)
        {
            return new UserEntity { Email = request.Email, Password = request.Password, Name = request.Name, MobileNumber = request.MobileNumber };
        }
        public Task<int> SignUp(UserRequest requestDto)
        {

            return _user.SignUp(MapToEntity(requestDto));
        }
    }
}
