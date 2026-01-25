namespace IgameToolsWinForms;

public record struct Juego
{
    public required string Nombre { get; init; }
    public required string Genero { get; set; }
    public required string Slave { get; init; }
    public required string Path { get; init; }
    public string Ruta { get; set; } = string.Empty; // Para compatibilidad con código existente
    public string NombreCorto { get; set; } = string.Empty;
    public bool EsDesconocido { get; set; }

    public string Dato1 { get; init; } = "0";
    public string Dato2 { get; init; } = "0";
    public string Dato3 { get; init; } = "0";
    public string Dato4 { get; init; } = "0";

    public Juego() { } // Constructor por defecto para record struct
}
