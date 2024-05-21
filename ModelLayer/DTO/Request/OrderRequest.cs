using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.DTO.Request
{
    public class OrderRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
