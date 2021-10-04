using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment4.Entities.Migrations
{
    public partial class InitialDreng : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    AssignedToEmail = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Title);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_AssignedToEmail",
                        column: x => x.AssignedToEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagTask",
                columns: table => new
                {
                    Tagsname = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TasksTitle = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTask", x => new { x.Tagsname, x.TasksTitle });
                    table.ForeignKey(
                        name: "FK_TagTask_Tags_Tagsname",
                        column: x => x.Tagsname,
                        principalTable: "Tags",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTask_Tasks_TasksTitle",
                        column: x => x.TasksTitle,
                        principalTable: "Tasks",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagTask_TasksTitle",
                table: "TagTask",
                column: "TasksTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToEmail",
                table: "Tasks",
                column: "AssignedToEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagTask");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
