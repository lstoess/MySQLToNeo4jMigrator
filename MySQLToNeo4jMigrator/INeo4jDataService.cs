/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-02
 * Kurzbeschreibung: Interface für den Service, der mit Neo4j-Datenbankoperationen interagiert.
 */

using SQLtoNeo4j.Models;

namespace SQLtoNeo4j.Interfaces
{
    public interface INeo4jDataService
    {
        // Definiert eine Methode zum Speichern von Materialflüssen in Neo4j.
        // Die Methode nimmt eine Sammlung von MaterialFlow-Objekten entgegen und speichert sie asynchron.
        Task SaveMaterialFlowsAsync(IEnumerable<MaterialFlow> materialFlows);
    }
}
