using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobRegistrationSubmisson.Migrations
{
    public partial class InitJoblist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_M_Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_User",
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
                    table.PrimaryKey("PK_TB_M_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_Trans_Joblist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreateData = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    DeleteData = table.Column<DateTimeOffset>(nullable: false),
                    isDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Trans_Joblist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Employees",
                columns: table => new
                {
                    EmpId = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(nullable: false),
                    DeleteTime = table.Column<DateTimeOffset>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Employees", x => x.EmpId);
                    table.ForeignKey(
                        name: "FK_Tb_Employees_TB_M_User_EmpId",
                        column: x => x.EmpId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_UserRole",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_UserRole", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Tb_M_UserRole_Tb_M_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Tb_M_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tb_M_UserRole_TB_M_User_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_M_JobSeeker",
                columns: table => new
                {
                    JobSId = table.Column<string>(nullable: false),
                    JoblistId = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Birth_Date = table.Column<DateTime>(nullable: false),
                    Nationality = table.Column<string>(nullable: true),
                    Marital_Status = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    Last_Education = table.Column<string>(nullable: true),
                    GPA = table.Column<string>(nullable: true),
                    Technical_Skill = table.Column<string>(nullable: true),
                    Experience = table.Column<string>(nullable: true),
                    Achievement = table.Column<string>(nullable: true),
                    RegistDate = table.Column<DateTimeOffset>(nullable: false),
                    UpdateDate = table.Column<DateTimeOffset>(nullable: false),
                    RejectDate = table.Column<DateTimeOffset>(nullable: false),
                    Approve = table.Column<bool>(nullable: false),
                    Reject = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_M_JobSeeker", x => x.JoblistId);
                    table.UniqueConstraint("AK_TB_M_JobSeeker_JobSId", x => x.JobSId);
                    table.ForeignKey(
                        name: "FK_TB_M_JobSeeker_TB_M_User_JobSId",
                        column: x => x.JobSId,
                        principalTable: "TB_M_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_M_JobSeeker_TB_Trans_Joblist_JoblistId",
                        column: x => x.JoblistId,
                        principalTable: "TB_Trans_Joblist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_M_UserRole_RoleId",
                table: "Tb_M_UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Employees");

            migrationBuilder.DropTable(
                name: "TB_M_JobSeeker");

            migrationBuilder.DropTable(
                name: "Tb_M_UserRole");

            migrationBuilder.DropTable(
                name: "TB_Trans_Joblist");

            migrationBuilder.DropTable(
                name: "Tb_M_Role");

            migrationBuilder.DropTable(
                name: "TB_M_User");
        }
    }
}
