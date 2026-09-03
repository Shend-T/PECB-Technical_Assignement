using backend.Data;
using backend.DTOs.Ticket;
using backend.DTOs.Agent;
using backend.DTOs.Comment;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        Status? status = null,
        Priority? priority = null,
        int? assignedAgentId = null,
        bool overdueOnly = false
        )
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        var query = _context.Tickets
            .Include(t => t.assignedAgent)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.referenceId.Contains(search) ||
                t.title.Contains(search) ||
                t.customerName.Contains(search));
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.priority == priority.Value);
        }

        if (assignedAgentId.HasValue)
        {
            query = query.Where(t =>
                t.assignedAgentId == assignedAgentId.Value);
        }

        if (overdueOnly)
        {
            query = query.Where(t =>
                t.dueDate < DateTime.UtcNow &&
                t.status != Status.Resolved &&
                t.status != Status.Closed);
        }

        var totalCount = await query.CountAsync();

        var tickets = await query
            .OrderByDescending(t => t.createdDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = tickets.Select(ToTicketDto);

        return Ok(new
        {
            items = result,
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.assignedAgent)
            .Include(t => t.comments)
            .FirstOrDefaultAsync(t => t.id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        return Ok(ToTicketDto(ticket));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketDto dto)
    {
        if (!Enum.IsDefined(typeof(Priority), dto.priority))
        {
            return BadRequest(new
            {
                message = "Prioritet gabim."
            });
        }

        if (dto.assignedAgentId.HasValue)
        {
            var agent = await _context.Agents
                .FindAsync(dto.assignedAgentId.Value);

            if (agent == null)
            {
                return BadRequest(new
                {
                    message = "Agjendi i zgjedhur nuk ekziston."
                });
            }

            if (!agent.active)
            {
                return BadRequest(new
                {
                    message = "Nje agjend jo aktiv nuk mund ti jepet nje tiket."
                });
            }
        }

        var createdDate = DateTime.UtcNow;

        var ticket = new Ticket
        {
            title = dto.title,
            description = dto.description,
            customerName = dto.customerName,
            customerEmail = dto.customerEmail,

            priority = dto.priority,
            status = Status.New,

            assignedAgentId = dto.assignedAgentId,

            createdDate = createdDate,
            lastModifiedDate = createdDate,

            dueDate = CalculateDueDate(dto.priority, createdDate)
        };

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync();

        ticket.referenceId =
            $"TCK-{ticket.createdDate.Year}-{ticket.id:D4}";

        await _context.SaveChangesAsync();

        var result = await _context.Tickets
            .Include(t => t.assignedAgent)
            .FirstAsync(t => t.id == ticket.id);

        return CreatedAtAction(
            nameof(GetTicket),
            new { id = ticket.id },
            ToTicketDto(result)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(
        int id,
        UpdateTicketDto dto)
    {
        var ticket = await _context.Tickets
            .Include(t => t.assignedAgent)
            .FirstOrDefaultAsync(t => t.id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.status == Status.Closed)
        {
            return BadRequest(new
            {
                message = "Nje tiket e mbyller eshte `read-only`."
            });
        }

        if (!Enum.IsDefined(typeof(Priority), dto.priority))
        {
            return BadRequest(new
            {
                message = "Prioritet gabim."
            });
        }

        if (!Enum.IsDefined(typeof(Status), dto.status))
        {
            return BadRequest(new
            {
                message = "Status gabim."
            });
        }

        if (dto.assignedAgentId.HasValue)
        {
            var agent = await _context.Agents
                .FindAsync(dto.assignedAgentId.Value);

            if (agent == null)
            {
                return BadRequest(new
                {
                    message = "Agjendi qe po kerkoni nuk ekziston."
                });
            }

            if (!agent.active)
            {
                return BadRequest(new
                {
                    message = "Nje agjend jo-aktiv nuk mund t'i qaset tiketave."
                });
            }
        }

        if (ticket.status != dto.status)
        {
            if (!IsValidStatusTransition(ticket.status, dto.status))
            {
                return BadRequest(new
                {
                    message =
                        $"Statusi nuk mund te kaloj nga: {ticket.status} ne {dto.status}."
                });
            }
        }

        if (dto.status == Status.InProgress)
        {
            if (!dto.assignedAgentId.HasValue)
            {
                return BadRequest(new
                {
                    message =
                        "Nje tiket duhet te kete nje agjend per te kaluar ne `In Progress`."
                });
            }

            var assignedAgent = await _context.Agents
                .FindAsync(dto.assignedAgentId.Value);

            if (assignedAgent == null || !assignedAgent.active)
            {
                return BadRequest(new
                {
                    message =
                        "Nje tiket nuk mund t'kaloj ne In Progress nese agjendi nuk eshte aktiv."
                });
            }
        }

        var oldPriority = ticket.priority;
        var oldStatus = ticket.status;

        ticket.title = dto.title;
        ticket.description = dto.description;
        ticket.customerName = dto.customerName;
        ticket.customerEmail = dto.customerEmail;
        ticket.priority = dto.priority;
        ticket.assignedAgentId = dto.assignedAgentId;

        ticket.status = dto.status;

        ticket.lastModifiedDate = DateTime.UtcNow;

        if (oldPriority != ticket.priority)
        {
            ticket.dueDate = CalculateDueDate(
                ticket.priority,
                ticket.createdDate
            );
        }

        if (oldStatus != Status.Resolved &&
            ticket.status == Status.Resolved)
        {
            ticket.resolvedDate = DateTime.UtcNow;
        }

        if (oldStatus != Status.Closed &&
            ticket.status == Status.Closed)
        {
            ticket.closedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        var result = await _context.Tickets
            .Include(t => t.assignedAgent)
            .FirstAsync(t => t.id == ticket.id);

        return Ok(ToTicketDto(result));
    }

    [HttpPut("{id}/assignment")]
    public async Task<IActionResult> AssignAgent(
        int id,
        AssignAgentDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.status == Status.Closed)
        {
            return BadRequest(new
            {
                message = "Nje tiket e mbyllur eshte `read-only`."
            });
        }

        if (dto.assignedAgentId.HasValue)
        {
            var agent = await _context.Agents
                .FindAsync(dto.assignedAgentId.Value);

            if (agent == null)
            {
                return BadRequest(new
                {
                    message = "Agjendi selektuar nuk ekziston."
                });
            }

            if (!agent.active)
            {
                return BadRequest(new
                {
                    message = "Nje agjend jo aktiv nuk mund te merr tiket."
                });
            }
        }

        ticket.assignedAgentId = dto.assignedAgentId;
        ticket.lastModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ToTicketDto(ticket));
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(
        int id,
        ChangeStatusDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.status == Status.Closed)
        {
            return BadRequest(new
            {
                message = "Nje tiket e mbyllur eshte `read-only`."
            });
        }

        if (!Enum.IsDefined(typeof(Status), dto.status))
        {
            return BadRequest(new
            {
                message = "Statusi i zgjedhur nuk ekziston."
            });
        }

        if (ticket.status != dto.status)
        {
            if (!IsValidStatusTransition(ticket.status, dto.status))
            {
                return BadRequest(new
                {
                    message =
                        $"Status nuk mund te ndrroj nga {ticket.status} ne {dto.status}."
                });
            }
        }

        if (dto.status == Status.InProgress)
        {
            if (!ticket.assignedAgentId.HasValue)
            {
                return BadRequest(new
                {
                    message =
                        "Nje tiket duhet te kete nje agjend para se te shkoje ne `In Progress`."
                });
            }

            var agent = await _context.Agents
                .FindAsync(ticket.assignedAgentId.Value);

            if (agent == null || !agent.active)
            {
                return BadRequest(new
                {
                    message =
                        "Nje tiket nuk mund te kaloj ne `In Progress` nese agjendi saj eshte jo aktiv."
                });
            }
        }

        var oldStatus = ticket.status;

        ticket.status = dto.status;
        ticket.lastModifiedDate = DateTime.UtcNow;

        if (oldStatus != Status.Resolved &&
            ticket.status == Status.Resolved)
        {
            ticket.resolvedDate = DateTime.UtcNow;
        }

        if (oldStatus != Status.Closed &&
            ticket.status == Status.Closed)
        {
            ticket.closedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return Ok(ToTicketDto(ticket));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.status == Status.Closed)
        {
            return BadRequest(new
            {
                message = "Nje tiket e mbyllur eshte `read-only` nuk mund te fshihet."
            });
        }

        _context.Tickets.Remove(ticket);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static bool IsValidStatusTransition(
        Status current,
        Status requested)
    {
        return (current, requested) switch
        {
            (Status.New, Status.InProgress) => true,

            (Status.InProgress, Status.Resolved) => true,

            (Status.Resolved, Status.Closed) => true,

            (Status.Resolved, Status.InProgress) => true,

            _ => false
        };
    }

    private static DateTime CalculateDueDate(
        Priority priority,
        DateTime createdDate)
    {
        return priority switch
        {
            Priority.Critical => createdDate.AddHours(4),
            Priority.High => createdDate.AddDays(1),
            Priority.Normal => createdDate.AddDays(3),
            Priority.Low => createdDate.AddDays(7),

            _ => throw new ArgumentOutOfRangeException(
                nameof(priority),
                priority,
                "Prioritet Gabim.")
        };
    }

    private static TicketDto ToTicketDto(Ticket ticket)
    {
        return new TicketDto
        {
            id          = ticket.id,
            referenceId = ticket.referenceId,

            title       = ticket.title,
            description = ticket.description,

            customerName = ticket.customerName,
            customerEmail = ticket.customerEmail,

            priority = ticket.priority,
            status   = ticket.status,

            assignedAgentId = ticket.assignedAgentId,

            assignedAgent = ticket.assignedAgent == null
                ? null
                : new AgentDto
                {
                    id = ticket.assignedAgent.id,
                    fullName = ticket.assignedAgent.fullName,
                    email = ticket.assignedAgent.email,
                    department = ticket.assignedAgent.department,
                    active = ticket.assignedAgent.active
                },

            createdDate      = ticket.createdDate,
            lastModifiedDate = ticket.lastModifiedDate,
            resolvedDate     = ticket.resolvedDate,
            closedDate       = ticket.closedDate,
            dueDate          = ticket.dueDate,

            isOverdue =
                ticket.dueDate < DateTime.UtcNow &&
                ticket.status != Status.Resolved &&
                ticket.status != Status.Closed,

            comments = ticket.comments
                .Select(comment => new CommentDto
                {
                    id = comment.id,
                    ticketId = comment.ticketId,
                    authorName = comment.authorName,
                    body = comment.body,
                    createdDate = comment.createdDate
                })
                .ToList()
        };
    }

    // Komentet, po i qes ktu endpoint-et, pasi qe pershkak te kohes se shkurte po mundohem pjesen kryesore ta perfundoj sa me shpejt, dhe se komentet varen nga tiketat
    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetComments(int id)
    {
        var ticketExists = await _context.Tickets
            .AnyAsync(t => t.id == id);

        if (!ticketExists)
        {
            return NotFound();
        }

        var comments = await _context.Comments
            .Where(c => c.ticketId == id)
            .OrderBy(c => c.createdDate)
            .Select(c => new CommentDto
            {
                id = c.id,
                ticketId = c.ticketId,
                authorName = c.authorName,
                body = c.body,
                createdDate = c.createdDate
            })
            .ToListAsync();

        return Ok(comments);
    }

    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(
        int id,
        CreateCommentDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.status == Status.Closed)
        {
            return BadRequest(new
            {
                message = "Nje tiket e mbyllur nuk mund t'merr komente te reja."
            });
        }

        var comment = new Comment
        {
            ticketId = id,
            authorName = dto.authorName,
            body = dto.body,
            createdDate = DateTime.UtcNow
        };

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        var result = new CommentDto
        {
            id = comment.id,
            ticketId = comment.ticketId,
            authorName = comment.authorName,
            body = comment.body,
            createdDate = comment.createdDate
        };

        return CreatedAtAction(
            nameof(GetComments),
            new { id },
            result
        );
    }
}