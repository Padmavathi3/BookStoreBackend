using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.NestedMethods;
using RepositoryLayer.RInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RServices
{
    public class UserService:IUser
    {
        private readonly DapperContext _context;
        private static string otp;
        private static string mailid;
        private static UserEntity entity;
        public UserService(DapperContext context)
        {
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------
        public async Task<string> ChangePassword(string otp, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(otp))
                {
                    return "Generate OTP First";
                }

                if (NestedMethodsClass.DecryptPassword(entity.Password).Equals(password))
                {
                    throw new PasswordMissMatchException("Don't give the existing password");
                }

                await Console.Out.WriteLineAsync(password);
                if (!NestedMethodsClass.IsStrongPassword(password))
                {
                    return "Password is not followed regex";
                }

                if (!otp.Equals(otp))
                {
                    return "OTP mismatch";
                }

                var result = await ResetPasswordByEmail(mailid, NestedMethodsClass.EncryptPassword(password));
                entity = null;
                otp = null;
                mailid = null;
                return $"password changed";
            }
            catch (PasswordMissMatchException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        public Task<String> ChangePasswordRequest(string Email)
        {
            try
            {
                entity = GetUsersByEmail(Email).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId" + e.Message);
            }

            string generatedotp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                generatedotp += r.Next(0, 10);
            }
            otp = generatedotp;
            mailid = Email;
            NestedMethodsClass.sendMail(Email, generatedotp);
            Console.WriteLine(otp);
            return Task.FromResult("MailSent ✔️");
        }
        //-----------------------------------------------------------------------------------------
        //update password using email

        private async Task<int> ResetPasswordByEmail(string emailid, string newPassword)
        {
            var users = await GetUsersByEmail(emailid);
            if (!users.Any())
            {
                // If no users are found with the given email, throw custom exception
                throw new EmailNotFoundException("Email does not exist.");
            }
            else
            {
                var query = "UPDATE users SET Password = @NewPassword WHERE Email = @Email";
                var parameters = new DynamicParameters();
                parameters.Add("@NewPassword", newPassword, DbType.String);
                parameters.Add("@Email", emailid, DbType.String);
                int rowsAffected = 0;
                using (var connection = _context.CreateConnection())
                {

                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return rowsAffected;
                    }
                    else
                    {
                        return 0;
                    }


                }
            }
        }
        //----------------------------------------------------------------------------------------
        public Task<int> DeleteUserByEmail(string email)
        {
            throw new NotImplementedException();
        }
        //------------------------------------------------------------------------------------------
        public async Task<IEnumerable<UserEntity>> GetUsers()
        {
            var query = "SELECT * FROM users";

            using (var connection = _context.CreateConnection())
            {
                var persons = await connection.QueryAsync<UserEntity>(query);

                if (persons.Any())
                {
                    return persons;
                }
                else
                {
                    throw new EmptyListException("No user is present in the table.");
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------
        //Get the user details based on email

        private async Task<IEnumerable<UserEntity>> GetUsersByEmail(string email)
        {
            var query = "select * from users WHERE Email = @Email";
            using (var connection = _context.CreateConnection())
            {
                var persons = await connection.QueryAsync<UserEntity>(query, new { Email = email });
                if (persons.Any())
                {
                    return persons;
                }
                else
                {
                    throw new EmptyListException("No user is present with this emailId in the table.");
                }
            }

        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        //login
        public async Task<IEnumerable<UserEntity>> Login(string email, string password)
        {

            var query = "SELECT * FROM users WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserEntity>(query, new { Email = email });

                // Iterate through each user retrieved
                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        // Decrypt the stored password
                        string storedPassword = NestedMethodsClass.DecryptPassword(user.Password);

                        // Compare the decrypted stored password with the encrypted provided password
                        if (password == storedPassword)
                        {
                            // If matched, return a list containing the matched user
                            return new List<UserEntity> { user };

                        }
                        else
                        {
                            throw new PasswordMissMatchException("password is not matched");
                        }
                    }
                }
                else
                    throw new UserNotFoundException("user is not registered so please create account");

                // If no matching user is found, return an empty list
                return Enumerable.Empty<UserEntity>();
            }
        }
        //------------------------------------------------------------------------

        public async Task<int> SignUp(UserEntity re_var)
        {
            var insert_query = "insert into users(Name,Email,Password,MobileNumber) values(@Name,@Email,@Password,@MobileNumber)";
            string encryptedPassword = NestedMethodsClass.EncryptPassword(re_var.Password);

            var parameters = new DynamicParameters();
            parameters.Add("@Name", re_var.Name, DbType.String);
            if (!NestedMethodsClass.IsValidGmailAddress(re_var.Email))
            {
                throw new InvalidEmailFormatException("Invalid Gmail address format");
            }
            else
            {
                parameters.Add("@Email", re_var.Email, DbType.String);
            }
            if (!NestedMethodsClass.IsStrongPassword(re_var.Password))
            {
                throw new InvalidPasswordException("password is invalid format");
            }
            else
            {
                parameters.Add("@Password", encryptedPassword, DbType.String);
            }
            if (!NestedMethodsClass.IsValidMobileNumber(re_var.MobileNumber))
            {
                throw new InvalidMobileNumberException("Mobile Number is invalid format");
            }
            else
            {
                parameters.Add("@MobileNumber", re_var.MobileNumber);
            }

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(insert_query, parameters);
            }
        }
    }
}
