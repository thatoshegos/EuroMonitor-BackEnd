﻿using System;
using System.Collections.Generic;

namespace BackEnd.Models
{
    public partial class WishlistItems
    {
        public int WishlistItemId { get; set; }
        public string WishlistId { get; set; }
        public int ProductId { get; set; }
    }
}
