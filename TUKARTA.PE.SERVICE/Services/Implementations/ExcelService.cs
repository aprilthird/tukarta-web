using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.SERVICE.Services.Interfaces;

namespace TUKARTA.PE.SERVICE.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public IXLWorkbook GeneratePurchaseReport(int purchaseType, IEnumerable<PurchaseResource> purchases)
        {
            var wb = new XLWorkbook();

            var dt = new DataTable
            {
                TableName = purchaseType == ConstantHelpers.Service.Type.ORDER
                    ? "ORDENES" : purchaseType == ConstantHelpers.Service.Type.BOOKING
                    ? "RESERVAS" : "DELIVERYS"
            };

            dt.Columns.Add("Código", typeof(int));
            dt.Columns.Add("Operación", typeof(int));
            dt.Columns.Add("Estado", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Hora", typeof(string));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Correo Electrónico", typeof(string));
            dt.Columns.Add("Teléfono", typeof(string));
            dt.Columns.Add("Restaurante", typeof(string));

            dt.Columns.Add("Facturación", typeof(string));
            dt.Columns.Add("Forma de Pago", typeof(string));
            dt.Columns.Add("Nombre de Boleta", typeof(string));
            dt.Columns.Add("Razón Social", typeof(string));
            dt.Columns.Add("RUC", typeof(string));

            dt.Columns.Add("Cantidad de Personas", typeof(int));
            dt.Columns.Add("Moneda", typeof(string));
            dt.Columns.Add("Monto", typeof(double));
            dt.Columns.Add("Nota", typeof(string));
            dt.Columns.Add("Rechazo", typeof(string));

            if(purchaseType == ConstantHelpers.Service.Type.DELIVERY)
            {
                dt.Columns.Add("Dirección", typeof(string));
            }

            foreach(var purchase in purchases)
            {
                var row = new List<object> { purchase.Code, purchase.OperationNumber, 
                    ConstantHelpers.Service.Status.VALUES[purchase.Status], purchase.CreatedAt.ToString("d"), 
                    purchase.CreatedAt.ToString("hh:mm"), purchase.User.RawFullName, purchase.User.Email,
                    purchase.User.PhoneNumber, purchase.Restaurant.Name };
                if (purchaseType == ConstantHelpers.Service.Type.ORDER)
                {
                    var order = purchase as OrderResource;
                    row.Add(ConstantHelpers.Service.IssueType.VALUES.GetValueOrDefault(order.IssueType));
                    row.Add(ConstantHelpers.Service.PaymentMethod.VALUES.GetValueOrDefault(ConstantHelpers.Service.PaymentMethod.CARD));
                    row.Add(order.TicketName);
                    row.Add(order.BusinessName);
                    row.Add(order.RUC);
                    row.Add(order.PeopleAmount);
                    //dt.Rows.Add(order.Code, order.OperationNumber, ConstantHelpers.Service.IssueType.VALUES[order.IssueType], )
                }
                else if (purchaseType == ConstantHelpers.Service.Type.BOOKING)
                {
                    var booking = purchase as BookingResource;
                }
                row.Add(ConstantHelpers.Service.CurrencyType.VALUES.GetValueOrDefault(purchase.Restaurant.CurrencyType));
                row.Add(purchase.Total);
                row.Add(purchase.Annotations);
                row.Add(purchase.RejectionReason);
                dt.Rows.Add(row.ToArray());
            }
            
            // Add a DataTable as a worksheet
            wb.Worksheets.Add(dt);
            wb.Worksheets.First().Columns().AdjustToContents();
            wb.Worksheets.First().Row(1).CellsUsed(x => x.GetString() == "Monto").First().WorksheetColumn().Style.NumberFormat.Format = "0.00";
            //wb.SaveAs($"{reportTitle}.xlsx");
            
            return wb;
        }
    }
}
