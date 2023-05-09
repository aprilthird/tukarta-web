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
    public class BusinessViewModel : ViewModel
    {
        public IEnumerable<UserResource> UserBusiness { get; set; }

        public IEnumerable<RestaurantResource> Restaurants { get; set; }

        public BusinessViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
        {
            UserBusiness = await DbContext.Users
                .Where(x => x.UserRoles.Any(ur => ur.Role.Name == ConstantHelpers.Roles.BUSINESS))
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
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task LoadRestaurantsByUserIdAsync(Guid userId)
        {
            Restaurants = await DbContext.Restaurants
                .Where(x => x.UserId == userId)
                .Select(x => new RestaurantResource
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    RUC = x.RUC,
                    IsEnabled = x.IsEnabled,
                    LogoUrl = x.LogoUrl,
                    CreatedAt = x.CreatedAt,
                }).ToListAsync();
        }

        public async Task<bool> ChangeRestaurantStatusAsync(Guid restaurantId, bool isEnabled)
        {
            try
            {
                var restaurant = await DbContext.Restaurants.FindAsync(restaurantId);
                restaurant.IsEnabled = isEnabled;
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
