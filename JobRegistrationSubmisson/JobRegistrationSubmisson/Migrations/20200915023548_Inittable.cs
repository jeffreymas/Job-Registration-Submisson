using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobRegistrationSubmisson.Migrations
{
    public partial class Inittable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Account",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Roles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Employees",
                columns: table => new
                {
                    EmpId = table.Column<string>(nullable: false),
                    Adress = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(nullable: false),
                    DeleteTime = table.Column<DateTimeOffset>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_Tb_Employees_Tb_Account_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Tb_Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_AccRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    RolesId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_AccRoles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Tb_AccRoles_Tb_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Tb_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tb_AccRoles_Tb_Account_UserId",
                        column: x => x.UserId,
                        principalTable: "Tb_Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_AccRoles_RolesId",
                table: "Tb_AccRoles",
                column: "RolesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_AccRoles");

            migrationBuilder.DropTable(
                name: "Tb_Employees");

            migrationBuilder.DropTable(
                name: "Tb_Roles");

            migrationBuilder.DropTable(
                name: "Tb_Account");
        }
    }
}
