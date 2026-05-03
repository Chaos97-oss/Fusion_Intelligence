namespace Fusion.Core.Entities;

public class DeliveryAgent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
