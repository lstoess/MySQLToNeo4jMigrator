/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-02
 * Kurzbeschreibung: Interface für den Service, der mit SQL-Datenbankoperationen interagiert.
 */

using SQLtoNeo4j.Models;

namespace SQLtoNeo4j.Interfaces
{
    public interface ISqlDataService
    {
        // Asynchrone Methode, die eine Liste von `MaterialFlow`-Objekten zurückgibt.
        // Diese Methode ist verantwortlich für das Abrufen von Materialflussdaten aus einer SQL-Datenbank.
        Task<IEnumerable<MaterialFlow>> GetMaterialFlowsAsync();
    }
}
