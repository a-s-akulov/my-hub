using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketsGeneratorServices.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "my_awesome_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    product_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_my_awesome_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_my_awesome_products",
                columns: table => new
                {
                    log_id = table.Column<Guid>(type: "uuid", nullable: false),
                    changed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    changed_operation = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    product_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_my_awesome_products", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_log_my_awesome_products_my_awesome_products_id",
                        column: x => x.id,
                        principalTable: "my_awesome_products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_log_my_awesome_products_id",
                table: "log_my_awesome_products",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_my_awesome_products");

            migrationBuilder.DropTable(
                name: "my_awesome_products");
        }
    }
}
