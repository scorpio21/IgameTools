using IgameToolsWinForms.Servicios;
using IgameToolsWinForms.Modelos;

namespace IgameToolsWinForms.Interfaces;

public interface IServicioCsv
{
    Task<(List<Juego> juegos, ValidationResult validacion)> CargarCsvAsync(string ruta, IProgress<string>? progreso = null);
    Task<(bool exito, string mensaje)> GuardarCsvAsync(string ruta, List<Juego> juegos, bool mantenerDatos, bool titleCase, bool nombresCortos, IProgress<string>? progreso = null);
    
    // Métodos síncronos para compatibilidad temporal
    (List<Juego> juegos, ValidationResult validacion) CargarCsv(string ruta);
    (bool exito, string mensaje) GuardarCsv(string ruta, List<Juego> juegos, bool mantenerDatos, bool titleCase, bool nombresCortos);
}

public interface IServicioFixList
{
    Task<(bool exito, string mensaje, EstadisticasFixList estadisticas)> ActualizarDesdeFixListAsync(List<Juego> juegos, IProgress<string>? progreso = null);
    
    // Métodos síncronos para compatibilidad temporal
    (bool exito, string mensaje) ProbarConexionFtp();
    (bool exito, string mensaje, EstadisticasFixList estadisticas) EjecutarFixList(List<Juego> juegos);
    
    // Métodos asíncronos adicionales para compatibilidad
    Task<(bool exito, string mensaje)> ProbarConexionFtpAsync();
    Task<(bool exito, string mensaje, EstadisticasFixList estadisticas)> EjecutarFixListAsync(List<Juego> juegos);
}

public interface IServicioJuegos
{
    List<Juego> FiltrarJuegos(List<Juego> juegos, string texto, bool mostrarDuplicados, bool mostrarDesconocidos);
    List<Juego> OrdenarJuegos(List<Juego> juegos, int columna, bool ascendente);
    void QuickTag(List<Juego> juegos, string tag);
    
    // Métodos adicionales para compatibilidad temporal
    Juego CopiarJuego(Juego juego);
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

// Interfaz para el sistema de Undo/Redo
public interface IServicioUndo
{
    void EjecutarComando(ICommand comando);
    void ExecuteCommand(ICommand comando);
    void Deshacer();
    void Rehacer();
    void Undo();
    void Redo();
    bool PuedeDeshacer { get; }
    bool PuedeRehacer { get; }
    bool CanUndo { get; }
    bool CanRedo { get; }
    string UndoDescription { get; }
    string RedoDescription { get; }
    List<string> GetHistorial();
    void LimpiarHistorial();
}

public interface ICommand
{
    string Description { get; }
    void Execute();
    void Undo();
}
