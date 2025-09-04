using Microsoft.EntityFrameworkCore.Migrations;


namespace TicketsGeneratorServices.Db.Migrations.Base
{
    public class QuartzPostgresInitialMigration
    {

        public static void Up(MigrationBuilder migrationBuilder, string defaultParentSchema = "public", bool skipQuartzUserCreation = false, string quartzUserName = "quartz", bool skipQuartzSchemaCreation = false, string quartzSchema = "qrtz", string initialScriptPath = "QuartsPostgresInitialScript.sql")
        {
            // Создание схемы и пользователя для Quartz \|/
            if (!skipQuartzSchemaCreation)
                migrationBuilder.Sql($"CREATE SCHEMA {quartzSchema};");

            if (!skipQuartzUserCreation)
            {
                migrationBuilder.Sql($"CREATE USER {quartzUserName};");
                migrationBuilder.Sql($"GRANT USAGE ON SCHEMA {quartzSchema} TO {quartzUserName};");
                migrationBuilder.Sql($"GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA {quartzSchema} TO {quartzUserName};");
                migrationBuilder.Sql($"GRANT USAGE, SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA {quartzSchema} TO {quartzUserName};");
                migrationBuilder.Sql($"GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA {quartzSchema} TO {quartzUserName};");
                migrationBuilder.Sql($"ALTER USER {quartzUserName} SET SEARCH_PATH TO '{quartzSchema}';");
            }
            // Создание схемы и пользователя для Quartz /|\

            var initialScriptPathFull = Path.Combine("..", "TicketsGeneratorServices.Db", "Migrations", "Base", initialScriptPath);
            var initialScript = File.ReadAllText(initialScriptPathFull);

            //migrationBuilder.Sql($"");
            initialScript = $"SET SEARCH_PATH TO '{quartzSchema}';\n"
                + initialScript
                + $"\nSET SEARCH_PATH TO '{defaultParentSchema}';";

            migrationBuilder.Sql(initialScript);
        }


        public static void Down(MigrationBuilder migrationBuilder, string quartzUserName = "quartz", string quartzSchema = "qrtz")
        {
            migrationBuilder.Sql($"DROP SCHEMA {quartzSchema} CASCADE;");
        }
    }
}
