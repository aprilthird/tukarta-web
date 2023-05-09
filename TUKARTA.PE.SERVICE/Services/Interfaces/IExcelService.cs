using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Services.Interfaces
{
    public interface IExcelService
    {
        IXLWorkbook GeneratePurchaseReport(int purchaseType, IEnumerable<PurchaseResource> purchases);
    }
}
