using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.CORE.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToPercentage(this double value)
        {
            return value.ToString("0.00") + " %";
        }

        public static string ToRoundDouble(this double value)
        {
            return value.ToString("0.00");
        }

        public static string ToMoney(this double value)
        {
            return "S/ " + value.ToString("0.00");
        }
    }
}
