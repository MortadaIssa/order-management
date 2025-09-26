using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Orders;
using OrderManagement.Application.Features.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using System.Security.Claims;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderHandler _createOrderHandler;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(CreateOrderHandler createOrderHandler, IOrderRepository orderRepository)
        {
            _createOrderHandler = createOrderHandler;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            // extract user id from JWT subject claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst(ClaimTypes.Name) ?? User.FindFirst("sub") ?? User.FindFirst(ClaimTypes.Sid);
            string? userIdString = userIdClaim?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                // fallback to JwtRegisteredClaimNames.Sub
                var sub = User.FindFirst("sub")?.Value;
                if (!Guid.TryParse(sub, out userId))
                {
                    return Unauthorized();
                }
            }

            var order = await _createOrderHandler.HandleAsync(dto, userId);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, new { order.Id, order.TotalPrice });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetWithItemsAsync(id);
            if (order == null) return NotFound();

            return Ok(new
            {
                order.Id,
                order.UserId,
                order.OrderDate,
                order.TotalPrice,
                Items = order.Items
            });
        }
    }
}
