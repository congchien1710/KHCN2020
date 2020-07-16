using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KHCN.Data.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApi",
                table: "CMS_Function");

            migrationBuilder.CreateTable(
                name: "CMS_Api",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IdParent = table.Column<int>(nullable: true),
                    IdModule = table.Column<int>(nullable: false),
                    Controller = table.Column<string>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    ControllerAction = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_Api", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CMS_RoleApi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IdApi = table.Column<int>(nullable: false),
                    IdRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMS_RoleApi", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMS_Api");

            migrationBuilder.DropTable(
                name: "CMS_RoleApi");

            migrationBuilder.AddColumn<bool>(
                name: "IsApi",
                table: "CMS_Function",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
