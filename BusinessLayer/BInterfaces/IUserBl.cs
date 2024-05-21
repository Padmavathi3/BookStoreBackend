using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO.Request;
using ModelLayer.Entities;

namespace BusinessLayer.BInterfaces
{
    public interface IUserBl
    {
        //Signup
        public Task<int> SignUp(UserRequest requestDto);

        //Get
        public Task<IEnumerable<UserEntity>> GetUsers();

        //Get user details based on email
        //public Task<IEnumerable<UserEntity>> GetUsersByEmail(string email);

        //delete
        public Task<int> DeleteUserByEmail(string email);

        //login
        public Task<IEnumerable<UserEntity>> Login(string email, string password);

        //forgotPassword

        public Task<String> ChangePasswordRequest(string Email);
        public Task<string> ChangePassword(string otp, string password);
    }
}

