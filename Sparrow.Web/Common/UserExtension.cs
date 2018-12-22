using Sparrow.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sparrow.Web.Common
{
    public static class UserExtension
    {
        public static ClaimsIdentity ToIdentity(this User user, string authenticationType = "cookies")
        {
            var identity = new ClaimsIdentity(authenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            return identity;
        }

        public static int GetCurrentID(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier);
            int.TryParse(claim.Value, out int userId);
            return userId;
        }

        public static UserRole GetCurrentRole(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Role);
            Enum.TryParse(claim.Value, out UserRole role);
            return role;
        }
    }
}
