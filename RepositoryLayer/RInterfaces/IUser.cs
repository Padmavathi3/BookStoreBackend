using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RInterfaces
{
    public interface IUser
    {
        //Signup
        public Task<int> SignUp(UserEntity re_var);

        //Get
        public Task<IEnumerable<UserEntity>> GetUsers();



        //delete
        public Task<int> DeleteUserByEmail(string email);

        //login
        public Task<IEnumerable<UserEntity>> Login(string email, string password);

        //forgotPassword

        public Task<String> ChangePasswordRequest(string Email);
        public Task<string> ChangePassword(string otp, string password);
    }
}
