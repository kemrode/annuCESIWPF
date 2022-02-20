using Microsoft.EntityFrameworkCore.Migrations;

namespace annuaire.Core.Employees.Infrastructures.Migrations
{
    public partial class addMail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Employee");
        }
    }
}
