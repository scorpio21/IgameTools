namespace IgameToolsWinForms.Modelos;

public class EstadisticasFixList
{
    public int TotalJuegosProcesados { get; set; }
    public int JuegosActualizados { get; set; }
    public int GenerosCorregidos { get; set; }
    public int PathsEncontrados { get; set; }
    public int PathsCorregidos { get; set; }
    public int SlavesEncontrados { get; set; }
    public int ErroresCorregidos { get; set; }
    public int JuegosConDatosExtra { get; set; }
    public int JuegosSinCambios { get; set; }
    public double PorcentajeActualizados { get; set; }
    public double PorcentajeCorregidos { get; set; }
    public string DirectorioTrabajo { get; set; } = string.Empty;
    public DateTime FechaEjecucion { get; set; }
    public TimeSpan DuracionProceso { get; set; }
}
