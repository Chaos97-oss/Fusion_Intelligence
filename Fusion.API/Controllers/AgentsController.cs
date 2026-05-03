using Fusion.Core.DTOs;
using Fusion.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgentsController : ControllerBase
{
    private readonly IAgentService _agentService;

    public AgentsController(IAgentService agentService)
    {
        _agentService = agentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AgentDto>>> GetAll()
    {
        var agents = await _agentService.GetAllAgentsAsync();
        return Ok(agents);
    }

    [HttpPost]
    public async Task<ActionResult<AgentDto>> Create(CreateAgentDto dto)
    {
        var agent = await _agentService.CreateAgentAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = agent.Id }, agent);
    }
}
