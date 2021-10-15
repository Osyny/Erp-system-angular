using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Erp_ang2.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
            UserId = userId;
            FirstName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);
            LastName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname);
            Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            IpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
            PhoneNumber = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.HomePhone);
            FbLink = httpContextAccessor.HttpContext?.User?.FindFirstValue("fbLink");
        }

        public int UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string IpAddress { get; }
        public string PhoneNumber { get; set; }
        public string FbLink { get; set; }
    }
}
