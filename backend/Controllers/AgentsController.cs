using backend.Data;
using backend.Models;
using backend.DTOs.Agent;
using backend.Mappings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AgentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AgentDto>>> GetAgents()
    {
        var agents = await _context.Agents.ToListAsync();

        return Ok(agents.Select(a => a.ToDto()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AgentDto>> GetAgent(int id)
    {
        var agent = await _context.Agents.FindAsync(id);

        if (agent == null)
        {
            return NotFound();
        }

        return Ok(agent.ToDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAgent(CreateAgentDto dto)
    {
        var emailExists = await _context.Agents
            .AnyAsync(a => a.email == dto.email);

        if (emailExists)
        {
            return Conflict(new {
                message = "An agent with this email already exists! ( Nje agjend me kete email vetem se ekziston!)"
            });
        }

        if (!Enum.IsDefined(typeof(Department), dto.department))
        {
            return BadRequest(new
            {
                message = "Invalid department! ( Departament jo valid!)"
            });
        }

        var agent = new Agent
        {
            fullName   = dto.fullName,
            email      = dto.email,
            department = dto.department,
            active     = dto.active
        };

        _context.Agents.Add(agent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAgents),
            new { id = agent.id },
            agent
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAgent(int id, UpdateAgentDto dto)
    {
        var agent = await _context.Agents.FindAsync(id);

        if (agent == null)
        {
            return BadRequest(new { message = "Agjendi nuk u gjet!" });
        }

        var emailExists = await _context.Agents
            .AnyAsync(a => a.email == dto.email && a.id != id);
        
        if (emailExists)
        {
            return Conflict(new {
                message = "A different agent with this email already exists! ( Nje agjend tjeter me kete email vetem se ekziston!)"
            });
        }

        if (!Enum.IsDefined(typeof(Department), dto.department))
        {
            return BadRequest(new
            {
                message = "Invalid department! ( Departament jo valid!)"
            });
        }

        agent.fullName = dto.fullName;
        agent.email = dto.email;
        agent.department = dto.department;
        agent.active = dto.active;

        await _context.SaveChangesAsync();

        return Ok(agent.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgent(int id)
    {
        var agent = await _context.Agents.FindAsync(id);

        if (agent == null)
        {
            return NotFound();
        }

        _context.Agents.Remove(agent);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}