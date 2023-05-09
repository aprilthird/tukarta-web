using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.ADMIN.Helpers;

namespace TUKARTA.PE.WEB.PANEL.ADMIN.ViewModels.UsersViewModels
{
    public class DinerViewModel : ViewModel
    {
        public IEnumerable<UserResource> Diners { get; set; }

        public DinerViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
        {
            Diners = await DbContext.Users
                .Where(x => x.UserRoles.Any(ur => ur.Role.Name == ConstantHelpers.Roles.DINER))
                .Select(x => new UserResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    BirthDate = x.BirthDate,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();
        }

        public async Task<bool> ChangeStatusAsync(Guid id)
        {
            try
            {
                var diner = await DbContext.Users.FindAsync(id);
                diner.IsEnabled = !diner.IsEnabled;
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
