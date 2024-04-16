using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Migrations
{
    /// <inheritdoc />
    public partial class mssqllocal_migration_764 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoginDataId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LoginDataId",
                table: "Users",
                column: "LoginDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LoginData_LoginDataId",
                table: "Users",
                column: "LoginDataId",
                principalTable: "LoginData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoginData_LoginDataId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "LoginData");

            migrationBuilder.DropIndex(
                name: "IX_Users_LoginDataId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoginDataId",
                table: "Users");
        }
    }
}
