using Milliards.DTO;
using Milliards.Models;
using System;

namespace Milliards.Services
{
    public interface IAuthService
    {
        Object Login(Login user);
        Object Logout(string token);
        Object InsertUser(LoginDTO postLogin);
    }
}
