using Fusion.Core.Entities;
using Fusion.Core.Enums;
using Fusion.Core.Exceptions;
using Fusion.Infrastructure.Services;
using Fusion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fusion.Tests.Unit;

public class OrderServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task UpdateOrderStatus_InvalidTransition_ThrowsDomainException()
    {
        // Arrange
        var context = GetDbContext();
        var order = new Order { Status = OrderStatus.Created };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var service = new OrderService(context);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => 
            service.UpdateOrderStatusAsync(order.Id, OrderStatus.Delivered));
    }

    [Fact]
    public async Task AssignOrder_AgentAlreadyHasActiveOrder_ThrowsDomainException()
    {
        // Arrange
        var context = GetDbContext();
        var agentId = Guid.NewGuid();
        context.DeliveryAgents.Add(new DeliveryAgent { Id = agentId, Name = "Test", IsActive = true });
        
        var activeOrder = new Order { Status = OrderStatus.Assigned, DeliveryAgentId = agentId };
        var newOrder = new Order { Status = OrderStatus.Created };
        
        context.Orders.AddRange(activeOrder, newOrder);
        await context.SaveChangesAsync();

        var service = new OrderService(context);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => 
            service.AssignOrderAsync(newOrder.Id, agentId));
    }
}
