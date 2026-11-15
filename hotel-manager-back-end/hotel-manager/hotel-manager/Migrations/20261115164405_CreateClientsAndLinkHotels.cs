using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel_manager.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientsAndLinkHotels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                schema: "dbo",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ClientId",
                schema: "dbo",
                table: "Hotels",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_Clients_ClientId",
                schema: "dbo",
                table: "Hotels",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_Clients_ClientId",
                schema: "dbo",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ClientId",
                schema: "dbo",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ClientId",
                schema: "dbo",
                table: "Hotels");
        }
    }
}
