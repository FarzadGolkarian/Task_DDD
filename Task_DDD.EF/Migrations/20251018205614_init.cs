using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_DDD.EF.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientUserTypeEnum = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminUserType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TicketStatusTypeEnum = table.Column<int>(type: "int", nullable: false),
                    TicketPriorityTypeEnum = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Employee_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ticket_User_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AssignedToUserId",
                table: "Ticket",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CreatedByUserId",
                table: "Ticket",
                column: "CreatedByUserId");
            migrationBuilder.InsertData(table: "User", columns: new[] {
                                           "Id",
                                           "FullName",
                                           "Email",
                                           "Password",
                                           "AdminUserType",
                                           "CreatedAt",
                                           "CreatedBy",
                                           "IsActive",
                                           "IsDeleted"
                                       }, values: new object[] {
                                           "bf5c9b0b-fa95-46ca-932a-78304220fae9",
                                           "admin",
                                           "admin@admin.com",
                                           "58QQ84GtUlB/QZch4y8NvqDb3UdPlZgp7NuKBPun0mc=",
                                           "10",
                                           DateTime.UtcNow,
                                           "admin",
                                           true,
                                           false
                                       });
            migrationBuilder.InsertData(table: "Employee", columns: new[] {
                                           "Id",
                                           "FullName",
                                           "Email",
                                           "Password",
                                           "ClientUserTypeEnum",
                                           "CreatedAt",
                                           "CreatedBy",
                                           "IsActive"
                                       }, values: new object[] {
                                           "2d3fb4cb-d243-419e-b487-834de5e423ab",
                                           "user",
                                           "user@user.com",
                                           "vjJ+DNmN1GnodCbHpEn7Zeayqi2mc0ONvqVXRJx/NWY=",
                                          // "user",
                                           "20",
                                           DateTime.UtcNow,
                                           "admin",
                                           true
                                       });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
