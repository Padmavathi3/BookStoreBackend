using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class AddressEntity
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public long MobileNumber { get; set; }

        public string FullAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Type { get; set; }
    }
}
