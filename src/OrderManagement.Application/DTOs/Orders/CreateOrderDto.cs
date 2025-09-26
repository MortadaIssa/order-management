using System.Collections.Generic;

namespace OrderManagement.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
