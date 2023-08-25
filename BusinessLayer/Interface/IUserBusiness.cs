using CommonLayer.Models;
using RepoLayer.Entity;
using RepoLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        public Users UserRegistration(UserRegistrationModel userRegistrationModel);
        public string UserLogin(UserLoginModel userLoginModel);
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        public bool ResetPassword(string Email, string NewPassword, string ConfirmPassword);
        public List<Users> GetAllUsers();
        public List<Users> GetUserById(long UserId);

    }
}
