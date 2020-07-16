using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace KHCN.Api.Controllers
{
    public interface IUserProvider
    {
        string GetUserId();
        string GetUserName();
        string GetFullName();
    }

    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserId()
        {
            return _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
        }

        public string GetUserName()
        {
            return _context.HttpContext.User.FindFirst(i => i.Type == "UserName") == null ? "Anonymous" : _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserName").Value;
        }

        public string GetFullName()
        {
            return _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "FullName").Value;
        }
    }
}