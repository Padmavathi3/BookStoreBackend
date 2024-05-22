using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entities
{
    public class WishlistEntity
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

    }
}
