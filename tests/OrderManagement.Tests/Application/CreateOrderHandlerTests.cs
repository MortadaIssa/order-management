using Moq;
using OrderManagement.Application.DTOs.Orders;
using OrderManagement.Application.Features.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Tests.Application
{
    public class CreateOrderHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateOrderAndCalculateTotal_WhenValidCommand()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var orderRepoMock = new Mock<IOrderRepository>();
            orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(new User { Id = userId, Name = "Test", Email = "t@test.com" });

            var logMock = new Mock<ILoggingService>();
            var handler = new CreateOrderHandler(orderRepoMock.Object, userRepoMock.Object, logMock.Object);

            var dto = new CreateOrderDto
            {
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { Name = "A", Price = 10m, Quantity = 2 },
                    new OrderItemDto { Name = "B", Price = 5.5m, Quantity = 1 }
                }
            };

            // Act
            var created = await handler.HandleAsync(dto, userId);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(10m * 2 + 5.5m * 1, created.TotalPrice);
            orderRepoMock.Verify(r => r.AddAsync(It.Is<Order>(o => o.Id == created.Id && o.TotalPrice == created.TotalPrice)), Times.Once);
            logMock.Verify(l => l.LogAudit(It.Is<string>(s => s.Contains(created.Id.ToString()))), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowExceptionAndLogError_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var orderRepoMock = new Mock<IOrderRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User?)null);

            var logMock = new Mock<ILoggingService>();
            var handler = new CreateOrderHandler(orderRepoMock.Object, userRepoMock.Object, logMock.Object);

            var dto = new CreateOrderDto
            {
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { Name = "A", Price = 10m, Quantity = 1 }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(dto, userId));
            logMock.Verify(l => l.LogError(It.Is<string>(s => s.Contains(userId.ToString()))), Times.Once);
            orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
        }
    }
}
