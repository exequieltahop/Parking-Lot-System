using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Parking_Lot_System.Classes;
using Newtonsoft.Json;

namespace Parking_Lot_System.Services
{
    public class LogInStatusChecker
    {
        public async Task<bool> IsLoggedIn(string username, string url)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync($"{url}/user_logs/{username}.json");

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();

            UserStatus? user_status = JsonConvert.DeserializeObject<UserStatus>(responseContent);
            if (string.IsNullOrWhiteSpace(responseContent))
            {

            }
            return true;

        }
    }
}
