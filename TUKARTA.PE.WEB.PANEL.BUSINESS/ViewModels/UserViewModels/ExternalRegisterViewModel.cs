using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.UserViewModels
{
    public class ExternalRegisterViewModel : ViewModel
    {
        public UserResource User { get; set; }

        private UserManager<ApplicationUser> UserManager { get; set; }
        
        private SignInManager<ApplicationUser> SignInManager { get; set; }

        public ExternalRegisterViewModel(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager) : base(context)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<bool> SaveAsync(UserResource resource)
        {
            if (await DbContext.Users.AnyAsync(x => x.Email == resource.Email))
            {
                ErrorMessage = "El Correo Electrónico proporcionado ya se encuentra registrado.";
                return false;
            }

            var info = await SignInManager.GetExternalLoginInfoAsync();
            var user = new ApplicationUser();
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                user.UserName = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            user.PhoneNumber = resource.PhoneNumber;
            user.Name = resource.Name;
            user.Surname = resource.Surname;
            user.BirthDate = resource.BirthDate;
            try
            {
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user, ConstantHelpers.Roles.BUSINESS);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            return true;
                        }
                        else
                        {
                            ErrorMessage = string.Join("\\n", result.Errors.Select(x => $"{x.Code} - {x.Description}"));
                        }
                    }
                    else
                    {
                        ErrorMessage = string.Join("\\n", result.Errors.Select(x => $"{x.Code} - {x.Description}"));
                    }
                }
                else
                {
                    ErrorMessage = string.Join("\\n", result.Errors.Select(x => $"{x.Code} - {x.Description}"));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }
    }
}
