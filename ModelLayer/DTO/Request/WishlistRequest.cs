using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelLayer.DTO.Request
{
    public class WishlistRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
