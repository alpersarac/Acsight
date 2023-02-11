using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Acsight.Utility
{
    public static class SD
    {
        public static async Task<bool> Post(object obj)
        {

            string apiUrl = "https://localhost:7132/api/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                var response = await client.PostAsJsonAsync("CategoryAPI/Create", obj);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
    }
}
