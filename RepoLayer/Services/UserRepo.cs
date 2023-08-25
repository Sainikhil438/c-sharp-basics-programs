using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;


namespace RepoLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooContext _fundooContext;
        private readonly IConfiguration configuration;
        
        public UserRepo(FundooContext fundooContext,IConfiguration configuration)
        {
            this._fundooContext = fundooContext;
            this.configuration = configuration;
            
        }

        public Users UserRegistration(UserRegistrationModel userRegisterModel)
        {
            try
            {
                Users users = new Users();
                users.FirstName = userRegisterModel.FirstName;
                users.LastName = userRegisterModel.LastName;
                users.Email = userRegisterModel.Email;
                users.Password = userRegisterModel.Password;
                users.DateOfBirth = userRegisterModel.DateOfBirth;
                _fundooContext.Users.Add(users);
                _fundooContext.SaveChanges();
                if (users != null)
                {
                    return users;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UserLogin(UserLoginModel model)
        {
            try
            {

                var userEntity = _fundooContext.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (userEntity != null)
                {
                    
                   // return userEntity.Email;
                    var token = GenerateJwtToken(userEntity.Email, userEntity.UserID);
                    return token;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public class UserLoginResult
        {
            public Users UserEntity { get; set; }
            public string JwtToken { get; set; }
        }
        public string GenerateJwtToken(string Email, long UserId)
        {
           
                var claims = new List<Claim>
                {
                    new Claim("UserId", UserId.ToString()),
                    new Claim("Email", Email)
                };
                // You can add more claims as needed, such as roles or custom claims.


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(configuration["JwtSettings:Issuer"], configuration["JwtSettings:Audience"], claims, DateTime.Now, DateTime.Now.AddHours(1), creds);
            

                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            
        }
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                var emailValidity = _fundooContext.Users.FirstOrDefault(u => u.Email == forgotPasswordModel.Email);
                if(emailValidity != null)
                {
                    var token = GenerateJwtToken(emailValidity.Email, emailValidity.UserID);
                    //MSMQ msmq = new MSMQ();
                    //msmq.sendData2Queue(token);
                    //rabbitMQSubscriber.PublishMessage("password-reset-queue", token);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ResetPassword(string Email, string NewPassword, string ConfirmPassword)
        {
            try
            {
                if(NewPassword == ConfirmPassword)
                {
                    var emailValid = _fundooContext.Users.FirstOrDefault(u => u.Email == Email);
                    if(emailValid != null)
                    {
                        emailValid.Password = ConfirmPassword;
                        _fundooContext.Users.Update(emailValid);
                        _fundooContext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
        public List<Users> GetAllUsers()
        {
            try
            {
                List<Users> users = new List<Users>();
                users = _fundooContext.Users.ToList();
                return users;
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }
        public List<Users> GetUserById(long UserId)
        {
            try
            {
                var userEntity = _fundooContext.Users.FirstOrDefault(x => x.UserID == UserId);
                if(UserId == userEntity.UserID)
                {
                    List<Users> userData = new List<Users>();
                    userData.Add(userEntity);
                    return userData;
                }
                return null;
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }



    }

    
}
