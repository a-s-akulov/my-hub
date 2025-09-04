using Microsoft.EntityFrameworkCore.Migrations;
using TicketsGeneratorServices.Db.Migrations.Base;

#nullable disable

namespace TicketsGeneratorServices.Db.Migrations
{
    /// <inheritdoc />
    public partial class QuartzInitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Создание схемы для Quartz \|/
            QuartzPostgresInitialMigration.Up(migrationBuilder, skipQuartzUserCreation: true, skipQuartzSchemaCreation: true, defaultParentSchema: "public");
            // Создание схемы для Quartz /|\
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Откат схемы для Quartz \|/
            QuartzPostgresInitialMigration.Down(migrationBuilder);
            // Откат схемы для Quartz /|\
        }
    }
}
