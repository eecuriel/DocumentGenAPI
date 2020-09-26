using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyExpManAPI.Migrations
{
    public partial class ExpenseLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseLogs",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventArgument = table.Column<string>(nullable: true),
                    EventContext = table.Column<string>(nullable: true),
                    EventDateGeneration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseLogs", x => x.EventID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseLogs");
        }
    }
}
