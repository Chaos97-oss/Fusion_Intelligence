using Fusion.Core.DTOs;
using Fusion.Core.Enums;

namespace Fusion.Core.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
    Task<OrderDto?> GetOrderByIdAsync(Guid id);
    Task<PagedResult<OrderDto>> GetOrdersAsync(int page, int pageSize, OrderStatus? status);
    Task AssignOrderAsync(Guid orderId, Guid agentId);
    Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
}

public interface IAgentService
{
    Task<AgentDto> CreateAgentAsync(CreateAgentDto dto);
    Task<IEnumerable<AgentDto>> GetAllAgentsAsync();
}
