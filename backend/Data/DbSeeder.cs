using backend.Models;

namespace backend.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Agents.Any() || context.Tickets.Any())
        {
            return;
        }

        var now = DateTime.UtcNow;

        // ===== AGENTS =====
        var agents = new List<Agent>
        {
            new Agent
            {
                fullName = "Test 100",
                email = "test100@gmail.com",
                department = Department.Technical,
                active = true
            },
            new Agent
            {
                fullName = "Test 200",
                email = "test200@gmail.com",
                department = Department.Billing,
                active = true
            },
            new Agent
            {
                fullName = "Shend Tytynxhiu",
                email = "shend.tytynxhiu@pecb.com",
                department = Department.Technical,
                active = true
            },
            new Agent
            {
                fullName = "User 1",
                email = "user1@gmail.com",
                department = Department.General,
                active = true
            },
            new Agent
            {
                fullName = "User 2",
                email = "user2@gmail.com",
                department = Department.General,
                active = false
            }
        };

        context.Agents.AddRange(agents);
        context.SaveChanges();

        // ===== TICKETS =====

        var tickets = new List<Ticket>
        {
            CreateTicket(
                "lorem ipsum",
                "lorem ipsum lorem ipsum lorem ipsum lorem ipsum.",
                "User User1",
                "user@user.com",
                Priority.High,
                Status.New,
                null,
                now.AddHours(-2),
                now.AddDays(1)
            ),

            CreateTicket(
                "User",
                "User problem.",
                "User User",
                "user@hotmail.com",
                Priority.Low,
                Status.Resolved,
                agents[3].id,
                now.AddDays(-5),
                now.AddDays(2)
            ),

            CreateTicket(
                "Error",
                "Error.",
                "Error",
                "error@error.com",
                Priority.High,
                Status.Resolved,
                agents[2].id,
                now.AddDays(-2),
                now.AddDays(-1)
            ),

            CreateTicket(
                "Dummy text",
                "Dummy text.",
                "User Users",
                "user@users.com",
                Priority.Normal,
                Status.Resolved,
                agents[0].id,
                now.AddDays(-4),
                now.AddDays(-1)
            ),

            CreateTicket(
                "lorem ipsumlorem ipsum",
                "lorem ipsumlorem ipsumlorem ipsumlorem ipsum.",
                "Robert Taylor",
                "robert@example.com",
                Priority.Normal,
                Status.New,
                null,
                now.AddHours(-5),
                now.AddDays(3)
            ),

            CreateTicket(
                "Qasja ne account",
                "Kam harruar `password`-in.",
                "Test User 5",
                "testuser5@gmail.com",
                Priority.Low,
                Status.New,
                null,
                now.AddDays(-1),
                now.AddDays(6)
            ),

            CreateTicket(
                "Problem kritik",
                "Nje problem shum kritik",
                "Daniel Anderson",
                "daniel@example.com",
                Priority.Critical,
                Status.New,
                null,
                now.AddHours(-10),
                now.AddHours(-6)
            ),

            CreateTicket(
                "error",
                "Error 500.",
                "User Error",
                "user500@gmail.com",
                Priority.High,
                Status.InProgress,
                agents[0].id,
                now.AddHours(-6),
                now.AddHours(18)
            ),

            CreateTicket(
                "Test Tiket",
                "Test tiket.",
                "Tiket User",
                "tiketuser@gmail.com",
                Priority.High,
                Status.InProgress,
                agents[2].id,
                now.AddDays(-3),
                now.AddDays(-2)
            ),

            CreateTicket(
                "Error",
                "Error me tiketa.",
                "User",
                "user@user.com",
                Priority.Normal,
                Status.Resolved,
                agents[1].id,
                now.AddDays(-4),
                now.AddDays(-1)
            ),

            CreateTicket(
                "Error 404",
                "Error 404.",
                "Test User",
                "test@gmail.com",
                Priority.Normal,
                Status.Closed,
                agents[1].id,
                now.AddDays(-10),
                now.AddDays(-7)
            ),

            CreateTicket(
                "Lorem ipsum",
                "Lorem ipsum.",
                "Lorem ipsum",
                "test@gmail.com",
                Priority.Low,
                Status.New,
                null,
                now.AddDays(-10),
                now.AddDays(-3)
            ),

            CreateTicket(
                "Error 409",
                "Error 409.",
                "Error 409",
                "e409@gmail.com",
                Priority.Normal,
                Status.InProgress,
                agents[2].id,
                now.AddDays(-6),
                now.AddDays(-3)
            ),

            CreateTicket(
                "Error 304",
                "Not modified error.",
                "e304",
                "e304@error.com",
                Priority.Critical,
                Status.New,
                null,
                now.AddHours(-1),
                now.AddHours(3)
            ),

            CreateTicket(
                "Error 401",
                "Nuk jam autorizuar per tu pergjigjur komenteve.",
                "Test User",
                "test@gmail.com",
                Priority.Normal,
                Status.New,
                null,
                now.AddHours(-3),
                now.AddDays(3)
            ),

            CreateTicket(
                "Two-factor authentication",
                "Nuk mund te kyqem ne web faqe.",
                "Anonymous",
                "anon@gmail.com",
                Priority.High,
                Status.Closed,
                agents[0].id,
                now.AddDays(-8),
                now.AddDays(-7)
            ),

            CreateTicket(
                "Request",
                "Request.",
                "Request",
                "user@request.com",
                Priority.Normal,
                Status.Closed,
                agents[1].id,
                now.AddDays(-10),
                now.AddDays(-7)
            ),

            CreateTicket(
                "Error 502",
                "502.",
                "e502",
                "e502@error.com",
                Priority.Critical,
                Status.Closed,
                agents[2].id,
                now.AddDays(-6),
                now.AddDays(-6).AddHours(4)
            ),

            CreateTicket(
                "Error 503",
                "503.",
                "503",
                "error@error.com",
                Priority.Low,
                Status.Closed,
                agents[3].id,
                now.AddDays(-12),
                now.AddDays(-5)
            ),

            CreateTicket(
                "Lorem Ipsum",
                "Lorem Ipsum.",
                "User User",
                "user@gmail.com",
                Priority.High,
                Status.Resolved,
                agents[3].id,
                now.AddDays(-3),
                now.AddDays(-2)
            )
        };

        for (int i = 0; i < tickets.Count; i++)
        {
            tickets[i].referenceId = $"TCK-2026-{i + 1:D4}";
        }

        context.Tickets.AddRange(tickets);
        context.SaveChanges();

        // ===== COMMENTS ======
        var comments = new List<Comment>
        {
            new Comment
            {
                ticketId = tickets[0].id,
                authorName = "Test 1",
                body = "Koment nga Test 1.",
                createdDate = now.AddHours(-1)
            },

            new Comment
            {
                ticketId = tickets[4].id,
                authorName = "Komenti 2",
                body = "Koment per testim.",
                createdDate = now.AddHours(-2)
            },

            new Comment
            {
                ticketId = tickets[7].id,
                authorName = "Komenti perfundimtar",
                body = "Vetem per ta bere seed databazen.",
                createdDate = now.AddHours(-3)
            },
        };

        context.Comments.AddRange(comments);
        context.SaveChanges();
    }

    private static Ticket CreateTicket(
        string title,
        string description,
        string customerName,
        string customerEmail,
        Priority priority,
        Status status,
        int? assignedAgentId,
        DateTime createdDate,
        DateTime dueDate)
    {
        return new Ticket
        {
            title = title,
            description = description,
            customerName = customerName,
            customerEmail = customerEmail,
            priority = priority,
            status = status,
            assignedAgentId = assignedAgentId,
            createdDate = createdDate,
            lastModifiedDate = createdDate,
            dueDate = dueDate,

            resolvedDate = status == Status.Resolved || status == Status.Closed
                ? createdDate.AddHours(2)
                : null,

            closedDate = status == Status.Closed
                ? createdDate.AddHours(4)
                : null
        };
    }
}