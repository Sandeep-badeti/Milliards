using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Milliards.DTO;
using Milliards.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Milliards.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        public AuthService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public AuthService(AppDbContext context, IConfiguration iconfiguration)
        {
            this.context = context;
            _iconfiguration = iconfiguration;
        }
        public Object Login(Login user)
        {
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["JWT:key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _iconfiguration["JWT:Issuer"],
                    audience: _iconfiguration["JWT:Audience"],
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var userData = context.Login.Where(x => x.UserName.ToLower() == user.UserName.ToLower()).FirstOrDefault();
                userData.Token = tokenString;
                userData.ExpireTime = DateTime.Now.AddMinutes(30);
                context.Login.Update(userData);
                context.SaveChanges();
                context.Dispose();
                user.Password = "";
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                user.EmailAddress = userData.EmailAddress;
                user.Token = tokenString;
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Object Logout(string token)
        {
            try
            {
                if (token != null)
                {
                    var userData = context.Login.Where(x => x.Token == token).FirstOrDefault();
                    if (userData != null)
                    {
                        userData.Token = "";
                        context.SaveChanges();
                    }
                }
                return new { Logout = true };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Object InsertUser(LoginDTO postLogin)
        {
            var message = string.Empty;
            try
            {
                context.Login.Add(new Login()
                {
                    FirstName = postLogin.FirstName,
                    LastName = postLogin.LastName,
                    CreatedTime = DateTime.UtcNow,
                    EmailAddress = postLogin.EmailAddress,
                    Password = Encrypt(postLogin.Password),
                    UserName = postLogin.UserName,
                });
                context.SaveChanges();
                context.Dispose();
                message = _iconfiguration["DATA_INSERTED_SUCCESSFUL"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new { Token = message };
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
        public string Decrypt(string password)
        {
            var key = _iconfiguration["Ukey"];
            byte[] SrctArray;
            byte[] EnctArray = Convert.FromBase64String(password);
            TripleDESCryptoServiceProvider objt = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrpt = new MD5CryptoServiceProvider();
            SrctArray = objcrpt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objt.Key = SrctArray;
            objt.Mode = CipherMode.ECB;
            objt.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = objt.CreateDecryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            objt.Clear();
            return UTF8Encoding.UTF8.GetString(resArray);
        }
    }
}
