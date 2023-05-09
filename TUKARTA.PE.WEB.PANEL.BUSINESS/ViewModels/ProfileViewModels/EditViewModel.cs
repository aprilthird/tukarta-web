using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.ProfileViewModels
{
    public class EditViewModel : ViewModel
    {
        public UserResource User { get; set; }

        public EditViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync(Guid userId)
        {
            User = await DbContext.Users
                .Where(x => x.Id == userId)
                .Select(x =>
                new UserResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Name,
                    BirthDate = x.BirthDate,
                    Email = x.UserName,
                    PhoneNumber = x.PhoneNumber
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync(UserResource resource)
        {
            var user = await DbContext.Users.FindAsync(resource.Id.Value);
            if (await DbContext.Users.AnyAsync(x => x.Email == resource.Email && x.Id != resource.Id))
            {
                ErrorMessage = "El correo electrónico proporcionado ya se encuentra registrado.";
                return false;
            }
            user.Name = resource.Name;
            user.Surname = resource.Surname;
            user.BirthDate = resource.BirthDate;
            user.UserName = resource.Email;
            user.Email = resource.Email;
            user.PhoneNumber = resource.PhoneNumber;
            try
            {
                if (!resource.Id.HasValue)
                    await DbContext.Users.AddAsync(user);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }
    }
}
