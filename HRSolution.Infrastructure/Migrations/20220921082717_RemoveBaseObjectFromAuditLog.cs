using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSolution.Infrastructure.Migrations
{
    public partial class RemoveBaseObjectFromAuditLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AuditLog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AuditLog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditLog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "AuditLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "AuditLog",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
