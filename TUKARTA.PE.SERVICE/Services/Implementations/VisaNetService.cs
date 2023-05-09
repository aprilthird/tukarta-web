using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TUKARTA.PE.SERVICE.Services.Interfaces;

namespace TUKARTA.PE.SERVICE.Implementations
{
    public class VisaNetService : IVisaNetService
    {
        private readonly IConfiguration _configuration;

        public VisaNetService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetSecurityToken()
        {
            var visaNETConfiguration = _configuration.GetSection("VisaNET");
            var request = new HttpRequestMessage(HttpMethod.Post, visaNETConfiguration["SecurityTokenEndpoint"]);
            var credentials = Encoding.ASCII.GetBytes($"{visaNETConfiguration["User"]}:{visaNETConfiguration["Password"]}");
            request.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(credentials)}");
            var client = new HttpClient();
            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                var byteArray = await response.Content.ReadAsByteArrayAsync();
                return Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            }
            else
            {
                return null;
            }
        }
    }
}
