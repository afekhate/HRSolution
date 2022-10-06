using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSolution.Infrastructure.Migrations
{
    public partial class AddDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSpecifications",
                table: "JobSpecification");

            migrationBuilder.RenameTable(
                name: "JobSpecification",
                newName: "JobSpecification");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "JobSpecification",
                newName: "Id");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "JobSpecification",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "JobSpecification",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSpecifications",
                table: "JobSpecification",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HOD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSpecifications",
                table: "JobSpecification");

            migrationBuilder.RenameTable(
                name: "JobSpecification",
                newName: "JobSpecification");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JobSpecification",
                newName: "ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "JobSpecification",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "JobSpecification",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSpecifications",
                table: "JobSpecification",
                column: "ID");
        }
    }
}
