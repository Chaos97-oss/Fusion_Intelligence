using Fusion.Core.Enums;

namespace Fusion.Core.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid? DeliveryAgentId { get; set; }
}

public class CreateOrderDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
}

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
}

public class AgentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateAgentDto
{
    public string Name { get; set; } = string.Empty;
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
