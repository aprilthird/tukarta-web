using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.SERVICE.Extensions;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.SERVICE.Services.Interfaces;
using TUKARTA.PE.WEB.PANEL.ADMIN.Helpers;

namespace TUKARTA.PE.WEB.PANEL.ADMIN.ViewModels.OrdersViewModels
{
    public class IndexViewModel : ViewModel
    {
        public Guid RestaurantId { get; set; }
        
        public IEnumerable<OrderResource> Orders { get; set; }
        
        public IEnumerable<PurchaseDetailResource> Detail { get; set; }

        private IExcelService ExcelService { get; set; }

        public IndexViewModel(TuKartaDbContext context, IExcelService excelService) : base(context)
        {
            ExcelService = excelService;
        }

        public async Task LoadAsync()
        {
            Orders = await DbContext.Purchases
                .Where(x => x.Type == ConstantHelpers.Service.Type.ORDER)
                .Select(x => new OrderResource
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
                    ConsumptionModality = x.ConsumptionModality.Value,
                    Status = x.Status,
                    Details = x.PurchaseDetails.Select(p => new PurchaseDetailResource
                    {
                        UnitPrice = p.UnitPrice,
                        Quantity = p.Quantity
                    }).ToList(),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();
        }
        public async Task<bool> ChangeStatusAsync(Guid id, int status)
        {
            try
            {
                var order = await DbContext.Purchases.
                    FindAsync(id);

                order.Status = status;
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return false;
        }

        public async Task LoadDetailByOrderIdAsync(Guid purchaseId)
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
                    PromotionId = x.PromotionId,
                }).ToListAsync();
        }

        public async Task<string> GetReportFileString(PurchaseReportParametersResource purchaseReportParameters)
        {
            var query = DbContext.Purchases
                .Where(x => x.Type == ConstantHelpers.Service.Type.ORDER)
                .AsQueryable();
            
            if (purchaseReportParameters.StartDateTime.HasValue)
                query = query.Where(x => x.CreatedAt.Date >= purchaseReportParameters.StartDateTime.Value.Date);

            if (purchaseReportParameters.EndDateTime.HasValue)
                query = query.Where(x => x.CreatedAt.Date <= purchaseReportParameters.EndDateTime.Value.Date);

            var orders = await query
                .Select(x => new OrderResource
                {
                    Id = x.Id,
                    Code = x.Code,
                    OperationNumber = x.OperationNumber,
                    IssueType = x.IssueType.Value,
                    User = new UserResource
                    {
                        Name = x.User.Name,
                        Surname = x.User.Surname,
                        Email = x.User.Email,
                        PhoneNumber = x.User.PhoneNumber
                    },
                    TicketName = x.TicketName,
                    BusinessName = x.BusinessName,
                    RUC = x.RUC,
                    PeopleAmount = x.PeopleAmount.Value,
                    Restaurant = new RestaurantResource
                    {
                        Name = x.Restaurant.Name,
                        CurrencyType = x.Restaurant.CurrencyType
                    },
                    ConsumptionModality = x.ConsumptionModality.Value,
                    Status = x.Status,
                    Annotations = x.Annotations,
                    RejectionReason = x.RejectionReason,
                    Details = x.PurchaseDetails.Select(p => new PurchaseDetailResource
                    {
                        UnitPrice = p.UnitPrice,
                        Quantity = p.Quantity
                    }).ToList(),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                }).ToListAsync();
            return ExcelService.GeneratePurchaseReport(ConstantHelpers.Service.Type.ORDER, orders).ToBase64String();
        }
    }
}
