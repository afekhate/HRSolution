using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSolution.Infrastructure.Migrations
{
    public partial class ChangeIdfromDepartmentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Department_DepartmentId",
                table: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Unit_DepartmentId",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Unit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Unit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Unit_DepartmentId",
                table: "Unit",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Department_DepartmentId",
                table: "Unit",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
