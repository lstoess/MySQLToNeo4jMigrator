/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-02
 * Kurzbeschreibung: Implementierung des INeo4jDataService für den Zugriff auf Neo4j-Datenbanken.
 */

using Neo4jClient;
using SQLtoNeo4j.Interfaces;
using SQLtoNeo4j.Models;
using Serilog;
using System;

public class Neo4jDataService : INeo4jDataService   // Tragen Sie an den Stellen mit *** Ihre Neo4j-Verbindungsdaten ein
{
    private string _url = "***";
    private string _user = "***";
    private string _password = "***";

    public async Task SaveMaterialFlowsAsync(IEnumerable<MaterialFlow> materialFlows)
    {
        var client = new BoltGraphClient(new Uri(_url), _user, _password);  // Verbindung zu Neo4j herstellen
        try
        {
            await client.ConnectAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Es konnte keine Verbindung zu Neo4j hergestellt werden.");
            throw; 
        }

        Log.Information("Verbindung zu Neo4j hergestellt.");

        foreach (var flow in materialFlows)
        {
            try
            {
                await client.Cypher
                    .Merge("(start:Material {Name: $startMaterial})")
                    .OnCreate()
                    .Set("start = {Name: $startMaterial}")
                    .WithParams(new { startMaterial = flow.StartMaterial })
                    .Merge("(ziel:Material {Name: $zielMaterial})")
                    .OnCreate()
                    .Set("ziel = {Name: $zielMaterial}")
                    .WithParams(new { zielMaterial = flow.ZielMaterial })
                    .Merge("(start)-[r:FLIESST_ZU]->(ziel)")
                    .OnCreate()
                    .Set("r = {dosMenge: $dosMenge, dosTimestamp: $dosTimestamp, rohstoffChargenId: $rohstoffChargenId}")
                    .OnMatch()
                    .Set("r.dosMenge = $dosMenge, r.dosTimestamp = $dosTimestamp, r.rohstoffChargenId = $rohstoffChargenId")
                    .WithParams(new { 
                        dosMenge = flow.DosMenge, 
                        dosTimestamp = flow.DosTimestamp, 
                        rohstoffChargenId = flow.RohstoffChargenId })
                    .ExecuteWithoutResultsAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Fehler beim Speichern des Materialflusses von {flow.StartMaterial} zu {flow.ZielMaterial}.");
            }
        }

        Log.Information("Materialfluesse erfolgreich in Neo4j gespeichert.");
    }
}
