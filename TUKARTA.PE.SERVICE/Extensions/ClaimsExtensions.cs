using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TUKARTA.PE.SERVICE.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var strId = user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.NameIdentifier).Value;
                if (!string.IsNullOrEmpty(strId))
                    return Guid.Parse(strId);
            }

            return Guid.Empty;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Email).Value;
            }

            return "";
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.UserData).Value;
            }

            return "";
        }

        public static string GetSurname(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Surname).Value;
            }

            return "";
        }

        public static string GetFullName(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return $"{user.GetSurname()}, {user.GetName()}";
            }

            return "";
        }

        public static string GetRawFullName(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return $"{user.GetName()} {user.GetSurname()}";
            }

            return "";
        }

        public static string GetPhoneNumber(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.MobilePhone).Value;
            }

            return "";
        }

        public static string[] GetRoles(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Role).Value.Split(",");
            }

            return null;
        }

        public static bool IsInAnyRole(this ClaimsPrincipal user, string roleNames)
        {
            if (user.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(roleNames))
                {
                    var roleNamesList = roleNames.Split(",");
                    var roles = user.GetRoles();
                    return roles.Any(r => roleNamesList.Contains(r));
                }
            }

            return false;
        }

        public static bool IsInAnyRole(this ClaimsPrincipal user, List<string> roleNames)
        {
            if (user.Identity.IsAuthenticated)
            {
                var roles = user.GetRoles();
                return roles.Any(r => roleNames.Contains(r));
            }

            return false;
        }
    }
}
