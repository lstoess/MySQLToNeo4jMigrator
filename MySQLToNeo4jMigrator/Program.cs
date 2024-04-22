/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-11
 * Kurzbeschreibung: Start der Datenmigration von MySQL zu Neo4j
 */

using SQLtoNeo4j.Services;
using Serilog;

namespace SQLtoNeo4j
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("../../../logs/log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            try
            {
                var dataMigrationService = new DataMigrationService();
                await dataMigrationService.MigrateDataAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fehler bei der Datenmigration.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
