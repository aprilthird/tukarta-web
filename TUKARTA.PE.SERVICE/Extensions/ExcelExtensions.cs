using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TUKARTA.PE.SERVICE.Extensions
{
    public static class ExcelExtensions
    {
        public static string ToBase64String(this IXLWorkbook workbook)
        {
            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
