using BusinessLayer.Interface;
using CommonLayer.Models;
using RepoLayer.Entity;
using RepoLayer.Interface;
using RepoLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepo _UserRepo;
        public UserBusiness(IUserRepo UserRepo)
        {
            _UserRepo = UserRepo;
        }

        public Users UserRegistration(UserRegistrationModel Model)
        {
            try
            {
                return _UserRepo.UserRegistration(Model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public string UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                return _UserRepo.UserLogin(userLoginModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                return _UserRepo.ForgotPassword(forgotPasswordModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        public bool ResetPassword(string Email, string NewPassword, string ConfirmPassword)
        {
            try
            {
                return _UserRepo.ResetPassword(Email, NewPassword, ConfirmPassword);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Users> GetAllUsers()
        {
            try
            {
                return _UserRepo.GetAllUsers();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<Users> GetUserById(long UserId)
        {
            try
            {
                return _UserRepo.GetUserById(UserId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
