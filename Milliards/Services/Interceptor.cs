using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Milliards.Models;
using System;
using System.Linq;
using System.Net;

namespace Milliards.Services
{
    public class Interceptor : ActionFilterAttribute
    {
        private readonly AppDbContext context;

        public Interceptor(AppDbContext context)
        {
            this.context = context;
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var value = actionContext.HttpContext.Request.Headers["access-token"].ToString();

            var loginUser = context.Login.Where(a => a.Token == value.ToString()).SingleOrDefault();
            if (loginUser == null || loginUser.UserName == null)
            {
                actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                actionContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
            }
            else
            {
                if (loginUser.ExpireTime != null && loginUser.ExpireTime < DateTime.Now)
                {
                    actionContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    loginUser.Token = null;
                    context.Login.Update(loginUser);
                    context.SaveChanges();
                    context.Dispose();
                    actionContext.Result = new JsonResult(new { HttpStatusCode.Unauthorized });
                }
                else
                {
                    loginUser.ExpireTime = DateTime.Now.AddMinutes(30);
                    context.Login.Update(loginUser);
                    context.SaveChanges();
                    context.Dispose();
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}
