using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Milliards.DTO;
using Milliards.Models;
using Milliards.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Milliards.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        public AuthController(AppDbContext context, IAuthService authService, IConfiguration iconfiguration)
        {
            this.context = context;
            _authService = authService;
            _iconfiguration = iconfiguration;
        }
        [HttpPost]
        [ActionName("login")]
        public ActionResult<Object> Login(Login user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid request.");
                }
                if (context.Login.Any(x => x.UserName == user.UserName) && context.Login.Any(x => x.Password == Encrypt(user.Password)))
                {
                    var result = _authService.Login(user);
                    return Ok(result);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return string.Format("Exception Message:{0}\n\nInnerException: {1}\n\n", ex.Message ?? "", ex.InnerException != null ? ex.InnerException.Message ?? "" : "");
            }
        }
        [HttpGet]
        [ActionName("logout")]
        public ActionResult<Object> Logout()
        {
            string token = Request.Headers["access-token"].First();
            var result = _authService.Logout(token);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("UserRegistration")]
        public ActionResult<Object> InsertUser(LoginDTO postLogin)
        {
            var result = _authService.InsertUser(postLogin);
            return Ok(result);
        }
        public string Encrypt(string password)
        {
            var key = _iconfiguration["Ukey"];

            byte[] SrctArray;

            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(password);

            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();

            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();

            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            objcrpt.Clear();

            objt.Key = SrctArray;

            objt.Mode = CipherMode.ECB;

            objt.Padding = PaddingMode.PKCS7;

            ICryptoTransform crptotrns = objt.CreateEncryptor();

            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);

            objt.Clear();

            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
    }
}