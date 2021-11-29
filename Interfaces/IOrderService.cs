using BackEnd.Dto;
using System.Collections.Generic;

namespace BackEnd.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(int userId, OrdersDto orderDetails);
        List<OrdersDto> GetOrderList(int userId);
    }
}
