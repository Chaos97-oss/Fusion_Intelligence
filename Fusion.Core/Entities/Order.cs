using Fusion.Core.Enums;

namespace Fusion.Core.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerName { get; set; } = string.Empty;
    public string PickupLocation { get; set; } = string.Empty;
    public string DropoffLocation { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Optional reference to a delivery agent if assigned
    public Guid? DeliveryAgentId { get; set; }
    public DeliveryAgent? DeliveryAgent { get; set; }
}
