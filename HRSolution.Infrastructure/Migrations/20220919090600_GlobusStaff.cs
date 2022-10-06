using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRSolution.Infrastructure.Migrations
{
    public partial class GlobusStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stringType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stringId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deletionTimestamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountEnabled = table.Column<bool>(type: "bit", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    creationType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    displayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    givenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    immutableId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isCompromised = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    jobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastDirSyncTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mailNickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    preferredLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    streetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    telephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usageLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
