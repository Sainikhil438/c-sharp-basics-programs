using CommonLayer.Models;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using static RepoLayer.Services.UserRepo;

namespace RepoLayer.Interface
{
    public interface IUserRepo
    {
        public Users UserRegistration(UserRegistrationModel userRegisterModel);
        public string UserLogin(UserLoginModel model);
        public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        public bool ResetPassword(string Email, string NewPassword, string ConfirmPassword);
        public List<Users> GetAllUsers();
        public List<Users> GetUserById(long UserId);

    }
}
