using backend.Controllers;
using backend.Data;
using backend.DTOs.Ticket;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

public class TicketsControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static Agent CreateActiveAgent()
    {
        return new Agent
        {
            id = 8,
            fullName = "Test Agent",
            email = "test@agent.com",
            department = Department.General,
            active = true
        };
    }

    private static Ticket CreateTicket(
        Status status = Status.New,
        int? assignedAgentId = null)
    {
        var createdDate = DateTime.UtcNow;

        return new Ticket
        {
            id = 5,
            referenceId = "TCK-2026-0005",
            title = "Test Ticket",
            description = "Test description",
            customerName = "Test Customer",
            customerEmail = "customer@customer.com",
            priority = Priority.Normal,
            status = status,
            assignedAgentId = assignedAgentId,
            createdDate = createdDate,
            lastModifiedDate = createdDate,
        };
    }

    [Fact]
    public async Task NewTicket_CanMoveToInProgress_WhenActiveAgentIsAssigned()
    {
        await using var context = CreateContext();

        var agent = CreateActiveAgent();
        var ticket = CreateTicket();

        context.Agents.Add(agent);
        context.Tickets.Add(ticket);

        await context.SaveChangesAsync();

        var controller = new TicketsController(context);

        var dto = new ChangeStatusDto
        {
            status = Status.InProgress
        };

        ticket.assignedAgentId = agent.id;
        await context.SaveChangesAsync();

        var result = await controller.ChangeStatus(ticket.id, dto);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var updatedTicket = await context.Tickets
            .FirstAsync(t => t.id == ticket.id);

        Assert.Equal(Status.InProgress, updatedTicket.status);
    }

    [Fact]
    public async Task NewTicket_CannotMoveDirectlyToResolved()
    {
        await using var context = CreateContext();

        var ticket = CreateTicket(Status.New);

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        var controller = new TicketsController(context);

        var dto = new ChangeStatusDto
        {
            status = Status.Resolved
        };

        var result = await controller.ChangeStatus(ticket.id, dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        var updatedTicket = await context.Tickets
            .FirstAsync(t => t.id == ticket.id);

        Assert.Equal(Status.New, updatedTicket.status);
    }

    [Fact]
    public async Task NewTicket_CannotMoveToInProgress_WithoutAssignedAgent()
    {
        await using var context = CreateContext();

        var ticket = CreateTicket(Status.New);

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        var controller = new TicketsController(context);

        var dto = new ChangeStatusDto
        {
            status = Status.InProgress
        };

        var result = await controller.ChangeStatus(ticket.id, dto);

        Assert.IsType<BadRequestObjectResult>(result);

        var updatedTicket = await context.Tickets
            .FirstAsync(t => t.id == ticket.id);

        Assert.Equal(Status.New, updatedTicket.status);
    }

    [Fact]
    public async Task CriticalTicket_HasDueDateFourHoursAfterCreation()
    {
        await using var context = CreateContext();

        var controller = new TicketsController(context);

        var dto = new CreateTicketDto
        {
            title = "Critical Ticket",
            description = "Critical issue",
            customerName = "Test Customer",
            customerEmail = "customer@customer.com",
            priority = Priority.Critical,
            assignedAgentId = null
        };

        var result = await controller.CreateTicket(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        var createdTicket = Assert.IsType<backend.DTOs.Ticket.TicketDto>(
            createdResult.Value);

        var expectedDueDate = createdTicket.createdDate.AddHours(4);

        Assert.Equal(
            expectedDueDate,
            createdTicket.dueDate);
    }
}