using BlazorInputFile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Services.Interfaces;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.RestaurantViewModels
{
    public class EditViewModel : ViewModel
    {
        public Guid RestaurantId { get; set; }

        public RestaurantResource Restaurant { get; set; }

        public IBlobService BlobService { get; set; }

        public EditViewModel(TuKartaDbContext context, IBlobService blobService) : base(context)
        {
            BlobService = blobService;
        }

        public async Task LoadAsync()
        {
            Restaurant = await DbContext.Restaurants
                .Where(x => x.Id == RestaurantId)
                .Select(x => new RestaurantResource
                {
                    Id = x.Id,
                    Name = x.Name,
                    RUC = x.RUC,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    CommisionPrice = x.CommisionPrice,
                    CurrencyType = x.CurrencyType,
                    KilometersCoverage = x.KilometersCoverage,
                    EstimatedDeliveryMinutes = x.EstimatedDeliveryMinutes,
                    DeliveryCostPerKilometer = x.DeliveryCostPerKilometer,
                    MinimumAmount = x.MinimumAmount,    
                    WebSiteUrl = x.WebSiteUrl.ToString(),
                    IsOrderEnabled = x.IsOrderEnabled,
                    IsBookingEnabled = x.IsBookingEnabled,
                    IsDeliveryEnabled = x.IsDeliveryEnabled,
                    Story = x.Story,
                    LogoUrl = x.LogoUrl,
                    Latitude = x.Location.X,
                    Longitude = x.Location.Y,
                    Address = x.Address,
                    Schedules = x.ServiceSchedules.Select(s => new ServiceScheduleResource
                    {
                        Day = s.Day, 
                        IsClosed = s.IsClosed,
                        OpeningTime = new DateTime(s.OpeningTime.Ticks),
                        ClosingTime = new DateTime(s.ClosingTime.Ticks),
                        SecondOpeningTime = s.SecondOpeningTime.HasValue 
                            ? new DateTime(s.SecondOpeningTime.Value.Ticks) : (DateTime?)null,
                        SecondClosingTime = s.SecondClosingTime.HasValue
                            ? new DateTime(s.SecondClosingTime.Value.Ticks) : (DateTime?)null
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync(RestaurantResource resource, IFileListEntry file, Stream fileStream)
        {
            var restaurant = await DbContext.Restaurants
                .Include(x => x.ServiceSchedules)
                .FirstOrDefaultAsync(x => x.Id == resource.Id.Value);
            if (await DbContext.Restaurants.AnyAsync(x => x.RUC == resource.RUC && x.Id != resource.Id))
            {
                ErrorMessage = "El RUC proporcionado ya se encuentra registrado.";
                return false;
            }
            restaurant.Name = resource.Name;
            restaurant.RUC = resource.RUC;
            restaurant.Email = resource.Email;
            restaurant.PhoneNumber = resource.PhoneNumber;
            restaurant.CommisionPrice = resource.CommisionPrice;
            restaurant.CurrencyType = resource.CurrencyType;
            restaurant.KilometersCoverage = resource.KilometersCoverage;
            restaurant.EstimatedDeliveryMinutes = resource.EstimatedDeliveryMinutes;
            restaurant.WebSiteUrl = string.IsNullOrEmpty(resource.WebSiteUrl) 
                ? null : new Uri(resource.WebSiteUrl);
            restaurant.Story = resource.Story;
            restaurant.LogoUrl = resource.LogoUrl;
            restaurant.IsOrderEnabled = resource.IsOrderEnabled;
            restaurant.IsBookingEnabled = resource.IsBookingEnabled;
            restaurant.IsDeliveryEnabled = resource.IsDeliveryEnabled;
            restaurant.Location = new NetTopologySuite.Geometries.Point(resource.Latitude, resource.Longitude)
                { SRID = ConstantHelpers.Geometry.DEFAULT_COORD_SYSTEM };
            restaurant.Address = resource.Address;
            if (!restaurant.ServiceSchedules.Any())
            {
                restaurant.ServiceSchedules.AddRange(resource.Schedules.Select(x => new ENTITIES.Models.ServiceSchedule
                {
                    Day = x.Day,
                    IsClosed = false
                }));
            }
            foreach (var dayScheduleResource in resource.Schedules)
            {
                var daySchedule = restaurant.ServiceSchedules.FirstOrDefault(x => x.Day == dayScheduleResource.Day);
                daySchedule.IsClosed = dayScheduleResource.IsClosed;
                daySchedule.OpeningTime = new TimeSpan(dayScheduleResource.OpeningTime.Ticks);
                daySchedule.ClosingTime = new TimeSpan(dayScheduleResource.ClosingTime.Ticks);
                daySchedule.SecondOpeningTime = dayScheduleResource.SecondOpeningTime.HasValue
                    ? new TimeSpan(dayScheduleResource.SecondOpeningTime.Value.Ticks) : (TimeSpan?)null;
                daySchedule.SecondClosingTime = dayScheduleResource.SecondClosingTime.HasValue
                    ? new TimeSpan(dayScheduleResource.SecondClosingTime.Value.Ticks) : (TimeSpan?)null;
            }
            try
            {
                if (file != null)
                {
                    if (restaurant.LogoUrl != null)
                        await BlobService.DeleteBlobAsync(restaurant.LogoUrl);
                    restaurant.LogoUrl = await BlobService.UploadFileBlobAsync(fileStream, file.Name, ConstantHelpers.Storage.Containers.RESTAURANTS);
                }
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
