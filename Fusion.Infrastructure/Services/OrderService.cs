using Fusion.Core.DTOs;
using Fusion.Core.Entities;
using Fusion.Core.Enums;
using Fusion.Core.Exceptions;
using Fusion.Core.Interfaces;
using Fusion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerName = dto.CustomerName,
            PickupLocation = dto.PickupLocation,
            DropoffLocation = dto.DropoffLocation,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        return order == null ? null : MapToDto(order);
    }

    public async Task<PagedResult<OrderDto>> GetOrdersAsync(int page, int pageSize, OrderStatus? status)
    {
        var query = _context.Orders.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<OrderDto>
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items.Select(MapToDto).ToList()
        };
    }

    public async Task AssignOrderAsync(Guid orderId, Guid agentId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new DomainException("Order not found.");

        var agent = await _context.DeliveryAgents.FindAsync(agentId);
        if (agent == null)
            throw new DomainException("Agent not found.");

        if (!agent.IsActive)
            throw new DomainException("Cannot assign to an inactive agent.");

        if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
            throw new DomainException("Cannot reassign a Delivered or Cancelled order.");

        // Prevent multiple active assignments: An agent can only have one active order at a time
        var activeOrderForAgent = await _context.Orders
            .FirstOrDefaultAsync(o => o.DeliveryAgentId == agentId && 
                (o.Status == OrderStatus.Assigned || o.Status == OrderStatus.InTransit));
                
        if (activeOrderForAgent != null && activeOrderForAgent.Id != orderId)
        {
            throw new DomainException("Agent already has an active assignment.");
        }

        order.DeliveryAgentId = agentId;
        order.Status = OrderStatus.Assigned;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
            throw new DomainException("Order not found.");

        if (order.Status == newStatus) return;

        // Valid transitions: Created -> Assigned -> InTransit -> Delivered
        // Cancelled allowed only from Created or Assigned
        bool isValidTransition = false;

        switch (newStatus)
        {
            case OrderStatus.Assigned:
                isValidTransition = order.Status == OrderStatus.Created;
                break;
            case OrderStatus.InTransit:
                isValidTransition = order.Status == OrderStatus.Assigned;
                break;
            case OrderStatus.Delivered:
                isValidTransition = order.Status == OrderStatus.InTransit;
                break;
            case OrderStatus.Cancelled:
                isValidTransition = order.Status == OrderStatus.Created || order.Status == OrderStatus.Assigned;
                break;
            case OrderStatus.Created:
                // Cannot transition back to created
                isValidTransition = false;
                break;
        }

        if (!isValidTransition)
        {
            throw new DomainException($"Invalid transition from {order.Status} to {newStatus}.");
        }

        order.Status = newStatus;
        await _context.SaveChangesAsync();
    }

    private OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            PickupLocation = order.PickupLocation,
            DropoffLocation = order.DropoffLocation,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            DeliveryAgentId = order.DeliveryAgentId
        };
    }
}
