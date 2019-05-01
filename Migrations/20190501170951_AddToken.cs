using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class AddToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TokenId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccessToken",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TokenString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessToken", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TokenId",
                table: "Users",
                column: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AccessToken_TokenId",
                table: "Users",
                column: "TokenId",
                principalTable: "AccessToken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AccessToken_TokenId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccessToken");

            migrationBuilder.DropIndex(
                name: "IX_Users_TokenId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenId",
                table: "Users");
        }
    }
}
