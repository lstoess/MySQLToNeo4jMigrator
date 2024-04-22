/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-02
 * Kurzbeschreibung: Service für die Migration von Daten von MySQL zu Neo4j.
 */

using SQLtoNeo4j.Interfaces;
using SQLtoNeo4j.Models;
using Serilog;

namespace SQLtoNeo4j.Services
{
    public class DataMigrationService
    {
        private readonly ISqlDataService _sqlDataService;
        private readonly INeo4jDataService _neo4jDataService;

        public DataMigrationService()
        {
            _sqlDataService = new MySqlDataService();
            _neo4jDataService = new Neo4jDataService();
        }

        public async Task MigrateDataAsync()
        {
            try
            {
                Log.Information("Starte Datenmigration von MySQL zu Neo4j.");
                IEnumerable<MaterialFlow> materialFlows = await _sqlDataService.GetMaterialFlowsAsync();
                await _neo4jDataService.SaveMaterialFlowsAsync(materialFlows);
                Log.Information("Datenmigration erfolgreich abgeschlossen.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Fehler bei der Datenmigration.");
                throw;
            }
        }
    }
}
