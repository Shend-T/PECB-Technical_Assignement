using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    referenceId = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    customerName = table.Column<string>(type: "text", nullable: false),
                    customerEmail = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    assignedAgentId = table.Column<int>(type: "integer", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    resolvedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tickets_Agents_assignedAgentId",
                        column: x => x.assignedAgentId,
                        principalTable: "Agents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_assignedAgentId",
                table: "Tickets",
                column: "assignedAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_referenceId",
                table: "Tickets",
                column: "referenceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
