using System.Collections.Generic;

namespace IgameToolsWinForms.Modelos;

public class Estadisticas
{
    public int TotalJuegos { get; set; }
    public int JuegosUnicos { get; set; }
    public int Duplicados { get; set; }
    public int Desconocidos { get; set; }
    public int Actualizados { get; set; }
    public Dictionary<string, int> DistribucionGeneros { get; set; } = new();
    public double TasaActualizacion { get; set; }
    public double PorcentajeDuplicados { get; set; }
    public double PorcentajeDesconocidos { get; set; }
    public double PorcentajeActualizados { get; set; }
    public int LongitudPromedioNombres { get; set; }
    public int GenerosUnicos { get; set; }
    public string GeneroMasComun { get; set; } = string.Empty;
    public int JuegosDelGeneroMasComun { get; set; }
}
