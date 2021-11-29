using BackEnd.Models;

namespace BackEnd.Dto
{
    public class CartItemDto
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
