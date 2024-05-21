using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.DTO.Request
{
    public class AddressRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Name { get; set; }
        public long MobileNumber { get; set; }

        public string FullAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Type { get; set; }
    }
}
