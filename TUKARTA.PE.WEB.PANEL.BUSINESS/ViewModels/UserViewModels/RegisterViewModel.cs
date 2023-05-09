using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.UserViewModels
{
    public class RegisterViewModel : ViewModel
    {
        public UserResource User { get; set; }

        private UserManager<ApplicationUser> UserManager { get; set; }

        public RegisterViewModel(TuKartaDbContext context,
            UserManager<ApplicationUser> userManager) : base(context)
        {
            UserManager = userManager;
        }

        public async Task<bool> SaveAsync(UserResource resource)
        {
            if (await DbContext.Users.AnyAsync(x => x.Email == resource.Email))
            {
                ErrorMessage = "El Correo Electrónico proporcionado ya se encuentra registrado.";
                return false;
            }
            var user = new ApplicationUser();
            user.Email = resource.Email;
            user.UserName = resource.Email;
            user.PhoneNumber = resource.PhoneNumber;
            user.Name = resource.Name;
            user.Surname = resource.Surname;
            user.BirthDate = resource.BirthDate;
            try
            {
                var result = await UserManager.CreateAsync(user, resource.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user, ConstantHelpers.Roles.BUSINESS);
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
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }
    }
}
