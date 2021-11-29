using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd.Models
{
    public partial class CustomerOrderDetail
    {
        public int OrderDetailsId { get; set; }
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
