using OrderManagement.Application.DTOs.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Orders
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;

        public CreateOrderHandler(IOrderRepository orderRepository,
            IUserRepository userRepository,
            ILoggingService loggingService)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        public async Task<Order> HandleAsync(CreateOrderDto dto, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _loggingService.LogError($"User not found: {userId}");
                throw new InvalidOperationException("User not found.");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.UtcNow
            };

            foreach (var itemDto in dto.Items)
            {
                var item = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Quantity = itemDto.Quantity,
                    OrderId = order.Id
                };
                order.Items.Add(item);
            }

            order.TotalPrice = order.Items.Sum(i => i.Price * i.Quantity);

            await _orderRepository.AddAsync(order);

            _loggingService.LogAudit($"Order created: {order.Id} by user {userId}");

            return order;
        }
    }
}
