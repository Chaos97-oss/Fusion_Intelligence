using Fusion.Core.DTOs;
using Fusion.Core.Entities;
using Fusion.Core.Interfaces;
using Fusion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Infrastructure.Services;

public class AgentService : IAgentService
{
    private readonly AppDbContext _context;

    public AgentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AgentDto> CreateAgentAsync(CreateAgentDto dto)
    {
        var agent = new DeliveryAgent
        {
            Name = dto.Name,
            IsActive = true
        };

        _context.DeliveryAgents.Add(agent);
        await _context.SaveChangesAsync();

        return new AgentDto
        {
            Id = agent.Id,
            Name = agent.Name,
            IsActive = agent.IsActive
        };
    }

    public async Task<IEnumerable<AgentDto>> GetAllAgentsAsync()
    {
        return await _context.DeliveryAgents
            .Select(a => new AgentDto
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive
            })
            .ToListAsync();
    }
}
