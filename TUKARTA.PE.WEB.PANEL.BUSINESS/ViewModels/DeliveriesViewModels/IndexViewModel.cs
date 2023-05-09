using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.PANEL.BUSINESS.Helpers;

namespace TUKARTA.PE.WEB.PANEL.BUSINESS.ViewModels.DeliveriesViewModels
{
    public class IndexViewModel : ViewModel
    {
        public Guid RestaurantId { get; set; }
        public List<DeliveryResource> Deliveries { get; set; }

        public IEnumerable<PurchaseDetailResource> Detail { get; set; }

        public IndexViewModel(TuKartaDbContext context) : base(context)
        {
        }

        public async Task LoadAsync()
        {
            Deliveries = await DbContext.Purchases
                .Where(x => x.Type == ConstantHelpers.Service.Type.DELIVERY && x.RestaurantId == RestaurantId)
                .Select(x=>new DeliveryResource
                {
                    Id = x.Id,
                    Code = x.Code,
                    User = new UserResource
                    {
                        Name = x.User.Name,
                        Surname = x.User.Surname
                    },
                    Restaurant = new RestaurantResource
                    {
                        Name = x.Restaurant.Name
                    },
                    PaymentAmount = x.PaymentAmount,
                    PaymentMethod = x.PaymentMethod.Value,
                    Address = x.DeliveryAddress,
                    Details = x.PurchaseDetails.Select(p => new PurchaseDetailResource
                    {
                        UnitPrice = p.UnitPrice,
                        Quantity = p.Quantity
                    }).ToList(),
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();
                
        }

        public async Task<bool> ChangeStatusAsync(Guid id, int status)
        {
            try
            {
                var delivery = await DbContext.Purchases.
                    FindAsync(id);

                delivery.Status = status;
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task LoadDetailByDeliveryIdAsync(Guid purchaseId)
        {
            Detail = await DbContext.PurchaseDetails
                .Where(x => x.PurchaseId == purchaseId)
                .Select(x => new PurchaseDetailResource
                {
                    Id = x.Id,
                    OriginalPrice = x.OriginalPrice,
                    Description = x.Description,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    PromotionId = x.PromotionId
                }).ToListAsync();
        }
    }
}
