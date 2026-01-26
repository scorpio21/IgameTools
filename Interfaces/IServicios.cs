using IgameToolsWinForms.Servicios;

namespace IgameToolsWinForms.Interfaces;

public interface IServicioCsv
{
    Task<(List<Juego> juegos, ValidationResult validacion)> CargarCsvAsync(string ruta, IProgress<string>? progreso = null);
    Task<(bool exito, string mensaje)> GuardarCsvAsync(string ruta, List<Juego> juegos, bool mantenerDatos, bool titleCase, bool nombresCortos, IProgress<string>? progreso = null);
}

public interface IServicioFixList
{
    Task<(bool exito, string mensaje, EstadisticasFixList estadisticas)> ActualizarDesdeFixListAsync(List<Juego> juegos, IProgress<string>? progreso = null);
}

public interface IServicioJuegos
{
    List<Juego> FiltrarJuegos(List<Juego> juegos, string texto, bool mostrarDuplicados, bool mostrarDesconocidos);
    List<Juego> OrdenarJuegos(List<Juego> juegos, int columna, bool ascendente);
    void QuickTag(List<Juego> juegos, string tag);
}

public interface IServicioEstadisticas
{
    Estadisticas CalcularEstadisticas(List<Juego> juegos);
}

public interface IServicioEstadisticasFixList
{
    EstadisticasFixList CalcularEstadisticasFixList(List<Juego> juegos, List<Juego> juegosActualizados, TimeSpan duracion);
}

public interface IServicioBusquedaAvanzada
{
    List<Juego> BuscarJuegos(List<Juego> juegos, string nombre, string genero, string slave, string ruta, bool busquedaExacta);
}
