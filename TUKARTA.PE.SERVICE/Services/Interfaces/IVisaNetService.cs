using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TUKARTA.PE.SERVICE.Services.Interfaces
{
    public interface IVisaNetService
    {
        Task<string> GetSecurityToken();
    }
}
