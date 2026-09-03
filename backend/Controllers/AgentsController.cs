using backend.Data;
using backend.Models;
using backend.DTOs.Agent;

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
    public async Task<IActionResult> GetAgents()
    {
        var agents = await _context.Agents.ToListAsync();

        return Ok(agents);
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
}