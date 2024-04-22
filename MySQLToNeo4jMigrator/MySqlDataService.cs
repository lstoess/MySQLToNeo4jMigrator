/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-02
 * Kurzbeschreibung: Implementierung des ISqlDataService für den Zugriff auf MySQL-Datenbanken.
 */

using MySql.Data.MySqlClient;
using SQLtoNeo4j.Interfaces;
using SQLtoNeo4j.Models;
using System.Data;
using Serilog;
using System;

public class MySqlDataService : ISqlDataService
{
    private string _connectionString;

    public MySqlDataService()   // Tragen Sie an den Stellen mit *** Ihre MySQL-Verbindungsdaten ein
    {
        _connectionString = "server=***;user=***;database=***;port=***;password=***";
    }

    public async Task<IEnumerable<MaterialFlow>> GetMaterialFlowsAsync()
    {
        var materialFlows = new List<MaterialFlow>();

        try
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Verbindung zu MySQL konnte nicht hergestellt werden.");
                    throw;
                }
                
                Log.Information("Verbindung zu MySQL hergestellt.");

                var query = @"
                SELECT 
                    m1.Name AS start_material, 
                    m2.Name AS ziel_material,
                    SUM(mf.dos_Menge) AS dosierte_menge, 
                    mf.dosierung_timestamp,
                    mf.rohstoff_chargen_id
                FROM 
                    materialfluss mf
                JOIN 
                    material m1 ON mf.start_mID = m1.Code
                JOIN 
                    material m2 ON mf.ziel_mID = m2.Code
                GROUP BY 
                    m1.Name, 
                    m2.Name, 
                    mf.dosierung_timestamp,
                    mf.rohstoff_chargen_id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var flow = new MaterialFlow
                            {
                                StartMaterial = reader.GetString("start_material"),
                                ZielMaterial = reader.GetString("ziel_material"),
                                DosMenge = reader.GetDecimal("dosierte_menge"),
                                DosTimestamp = reader.GetString("dosierung_timestamp"),
                                RohstoffChargenId = reader.GetString("rohstoff_chargen_id")
                            };
                            materialFlows.Add(flow);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Abruf von Materialfluessen aus MySQL fehlgeschlagen.");
            throw;
        }

        return materialFlows;
    }
}
