using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Parking_Lot_System.Classes
{
    public class ParkingLogs
    {
        [JsonProperty("slot_no")]
        public string? Slot_no { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("timestamp")]
        public string? Timestamp { get; set; }

        //public string? Username { get; set; }
        //public string? Password { get; set; }
    }
}
