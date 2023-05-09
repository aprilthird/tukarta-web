using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.Services
{
    public class BusinessService
    {
        private readonly TuKartaDbContext _context;

        public BusinessService(TuKartaDbContext context)
        {
            _context = context;
        }

        public async Task<List<RestaurantResource>> GetBusinessForUserAsync(Guid userId)
        {
            var result = await _context.Restaurants
                .Where(x => x.UserId == userId)
                .Select(x => new RestaurantResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    LogoUrl = x.LogoUrl,
                }).ToListAsync();
            return result;
        }

        public async Task<int> GetPendingOrdersCount(Guid restaurantId)
        {
            var result = await _context.Purchases
                .Where(x => x.RestaurantId == restaurantId)
                .Where(x => x.Type == ConstantHelpers.Service.Type.ORDER)
                .Where(x => x.Status == ConstantHelpers.Service.Status.PENDING_APPROVAL)
                .CountAsync();
            return result;
        }

        public async Task<int> GetPendingBookingsCount(Guid restaurantId)
        {
            var result = await _context.Purchases
                .Where(x => x.RestaurantId == restaurantId)
                .Where(x => x.Type == ConstantHelpers.Service.Type.BOOKING)
                .Where(x => x.Status == ConstantHelpers.Service.Status.PENDING_APPROVAL)
                .CountAsync();
            return result;
        }

        public async Task<int> GetPendingDeliveriesCount(Guid restaurantId)
        {
            var result = await _context.Purchases
                .Where(x => x.RestaurantId == restaurantId)
                .Where(x => x.Type == ConstantHelpers.Service.Type.DELIVERY)
                .Where(x => x.Status == ConstantHelpers.Service.Status.PENDING_APPROVAL)
                .CountAsync();
            return result;
        }
    }
}
