using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd.Models
{
    public partial class WishlistItem
    {
        public int WishlistItemId { get; set; }
        public string WishlistId { get; set; }
        public int ProductId { get; set; }
    }
}
