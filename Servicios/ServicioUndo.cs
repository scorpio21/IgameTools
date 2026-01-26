using IgameToolsWinForms;
using IgameToolsWinForms.Interfaces;

namespace IgameToolsWinForms.Servicios;

// Comando para edición individual de juegos
public class EditarJuegoCommand : ICommand
{
    private readonly List<Juego> _juegos;
    private readonly int _indice;
    private readonly Juego _juegoOriginal;
    private readonly Juego _juegoNuevo;

    public EditarJuegoCommand(List<Juego> juegos, int indice, Juego juegoOriginal, Juego juegoNuevo)
    {
        _juegos = juegos;
        _indice = indice;
        _juegoOriginal = juegoOriginal;
        _juegoNuevo = juegoNuevo;
    }

    public string Description => $"Editar juego: {_juegoNuevo.Nombre}";

    public void Execute()
    {
        if (_indice >= 0 && _indice < _juegos.Count)
        {
            _juegos[_indice] = _juegoNuevo;
        }
    }

    public void Undo()
    {
        if (_indice >= 0 && _indice < _juegos.Count)
        {
            _juegos[_indice] = _juegoOriginal;
        }
    }
}

// Comando para etiquetado rápido
public class QuickTagCommand : ICommand
{
    private readonly List<Juego> _juegos;
    private readonly List<int> _indices;
    private readonly string _tag;
    private readonly List<Juego> _juegosOriginales;

    public QuickTagCommand(List<Juego> juegos, List<int> indices, string tag)
    {
        _juegos = juegos;
        _indices = indices;
        _tag = tag;
        _juegosOriginales = new List<Juego>();

        foreach (var indice in _indices)
        {
            if (indice >= 0 && indice < juegos.Count)
            {
                _juegosOriginales.Add(CopiarJuego(juegos[indice]));
            }
        }
    }

    public string Description => $"Etiquetar {_indices.Count} juegos con '{_tag}'";

    public void Execute()
    {
        foreach (var indice in _indices)
        {
            if (indice >= 0 && indice < _juegos.Count)
            {
                var juego = _juegos[indice];
                _juegos[indice] = new Juego
                {
                    Nombre = juego.Nombre + _tag,
                    Genero = juego.Genero,
                    Path = juego.Path,
                    Slave = juego.Slave,
                    NombreCorto = juego.NombreCorto,
                    Dato1 = juego.Dato1,
                    Dato2 = juego.Dato2,
                    Dato3 = juego.Dato3,
                    Dato4 = juego.Dato4,
                    EsDesconocido = juego.EsDesconocido
                };
            }
        }
    }

    public void Undo()
    {
        for (var i = 0; i < _indices.Count && i < _juegosOriginales.Count; i++)
        {
            var indice = _indices[i];
            if (indice >= 0 && indice < _juegos.Count)
            {
                _juegos[indice] = _juegosOriginales[i];
            }
        }
    }

    private static Juego CopiarJuego(Juego j)
    {
        return new Juego
        {
            Nombre = j.Nombre,
            Genero = j.Genero,
            Path = j.Path,
            Slave = j.Slave,
            NombreCorto = j.NombreCorto,
            Dato1 = j.Dato1,
            Dato2 = j.Dato2,
            Dato3 = j.Dato3,
            Dato4 = j.Dato4,
            EsDesconocido = j.EsDesconocido
        };
    }
}

// Comando para Fix List
public class FixListCommand : ICommand
{
    private readonly List<Juego> _juegos;
    private readonly List<Juego> _juegosOriginales;
    private readonly List<Juego> _juegosNuevos;

    public FixListCommand(List<Juego> juegos, List<Juego> juegosNuevos)
    {
        _juegos = juegos;
        _juegosOriginales = new List<Juego>(juegos);
        _juegosNuevos = new List<Juego>(juegosNuevos);
    }

    public string Description => $"Fix List: {_juegosNuevos.Count} juegos procesados";

    public void Execute()
    {
        _juegos.Clear();
        _juegos.AddRange(_juegosNuevos);
    }

    public void Undo()
    {
        _juegos.Clear();
        _juegos.AddRange(_juegosOriginales);
    }
}

// Comando para limpiar lista
public class LimpiarListaCommand : ICommand
{
    private readonly List<Juego> _juegos;
    private readonly List<Juego> _juegosOriginales;

    public LimpiarListaCommand(List<Juego> juegos)
    {
        _juegos = juegos;
        _juegosOriginales = new List<Juego>(juegos);
    }

    public string Description => $"Limpiar lista: {_juegosOriginales.Count} juegos eliminados";

    public void Execute()
    {
        _juegos.Clear();
    }

    public void Undo()
    {
        _juegos.Clear();
        _juegos.AddRange(_juegosOriginales);
    }
}

// Gestor de Undo/Redo
public class ServicioUndo : IServicioUndo
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();
    private readonly int _maxHistorySize;

    public ServicioUndo(int maxHistorySize = 50)
    {
        _maxHistorySize = maxHistorySize;
    }

    public bool PuedeDeshacer => _undoStack.Count > 0;
    public bool PuedeRehacer => _redoStack.Count > 0;
    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
    public string UndoDescription => CanUndo ? _undoStack.Peek().Description : "Deshacer";
    public string RedoDescription => CanRedo ? _redoStack.Peek().Description : "Rehacer";

    public void EjecutarComando(ICommand command)
    {
        try
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();

            // Limitar el tamaño del historial
            while (_undoStack.Count > _maxHistorySize)
            {
                _undoStack.Pop();
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al ejecutar comando '{command.Description}': {ex.Message}", ex);
        }
    }

    public void ExecuteCommand(ICommand command)
    {
        EjecutarComando(command);
    }

    public void Undo()
    {
        if (!CanUndo)
            throw new InvalidOperationException("No hay acciones para deshacer");

        var command = _undoStack.Pop();
        try
        {
            command.Undo();
            _redoStack.Push(command);
        }
        catch (Exception ex)
        {
            // Si falla el undo, devolver el comando al stack de undo
            _undoStack.Push(command);
            throw new InvalidOperationException($"Error al deshacer '{command.Description}': {ex.Message}", ex);
        }
    }

    public void Redo()
    {
        if (!CanRedo)
            throw new InvalidOperationException("No hay acciones para rehacer");

        var command = _redoStack.Pop();
        try
        {
            command.Execute();
            _undoStack.Push(command);
        }
        catch (Exception ex)
        {
            // Si falla el redo, devolver el comando al stack de redo
            _redoStack.Push(command);
            throw new InvalidOperationException($"Error al rehacer '{command.Description}': {ex.Message}", ex);
        }
    }

    public void Deshacer()
    {
        Undo();
    }

    public void Rehacer()
    {
        Redo();
    }

    public List<string> GetHistorial()
    {
        var historial = new List<string>();
        
        // Agregar comandos de undo (en orden inverso)
        foreach (var command in _undoStack.Reverse())
        {
            historial.Add($"↶ {command.Description}");
        }
        
        // Agregar separador
        if (_undoStack.Count > 0 && _redoStack.Count > 0)
        {
            historial.Add("---");
        }
        
        // Agregar comandos de redo
        foreach (var command in _redoStack)
        {
            historial.Add($"↷ {command.Description}");
        }
        
        return historial;
    }

    public void LimpiarHistorial()
    {
        ClearHistory();
    }

    public void ClearHistory()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }

    public int GetUndoCount() => _undoStack.Count;
    public int GetRedoCount() => _redoStack.Count;
}
