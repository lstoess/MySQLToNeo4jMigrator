/*
 * Autor: Laura Stößer
 * Erstellt am: 2024-03-27
 * Letzte Änderung: 2024-04-02
 * Kurzbeschreibung: Modellklasse für die Materialfluss-Daten.
 */

namespace SQLtoNeo4j.Models
{
    public class MaterialFlow
    {
        public string StartMaterial { get; set; }   // Startmaterial im Materialfluss
        public string ZielMaterial { get; set; }    // Zielmaterial im Materialfluss
        public decimal DosMenge { get; set; }       // Dosierte Menge des Materials, welches vom Start zum Ziel fließt
        public string DosTimestamp { get; set; }    // Timestamp der Dosierung   
        public string RohstoffChargenId { get; set; } // ID der Rohstoffcharge
    }
}
