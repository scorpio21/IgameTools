using IgameToolsWinForms;
using IgameToolsWinForms.Interfaces;

namespace IgameToolsWinForms.Servicios;

public class FiltrosJuegos
{
    public bool VerDuplicados { get; set; } = false;
    public bool VerDesconocidos { get; set; } = false;
    public string TextoBusqueda { get; set; } = string.Empty;
}

public class OrdenamientoJuegos
{
    public int Columna { get; set; } = 0;
    public bool Ascendente { get; set; } = true;
}

public class ServicioJuegos : IServicioJuegos
{
    public IEnumerable<Juego> AplicarFiltros(List<Juego> juegos, FiltrosJuegos filtros, HashSet<string> nombresDuplicados)
    {
        IEnumerable<Juego> query = juegos;

        // Filtrar por texto de búsqueda
        if (!string.IsNullOrWhiteSpace(filtros.TextoBusqueda))
        {
            var busqueda = filtros.TextoBusqueda.ToLowerInvariant();
            query = query.Where(j => 
                j.Nombre.ToLowerInvariant().Contains(busqueda) ||
                (!string.IsNullOrWhiteSpace(j.NombreCorto) && j.NombreCorto.ToLowerInvariant().Contains(busqueda)) ||
                j.Genero.ToLowerInvariant().Contains(busqueda));
        }

        // Filtrar duplicados
        if (!filtros.VerDuplicados)
        {
            query = query.Where(j => !nombresDuplicados.Contains(j.Nombre));
        }

        // Filtrar desconocidos
        if (!filtros.VerDesconocidos)
        {
            query = query.Where(j => !j.EsDesconocido);
        }

        return query;
    }

    public List<Juego> OrdenarJuegos(IEnumerable<Juego> juegos, OrdenamientoJuegos ordenamiento)
    {
        return ordenamiento.Columna switch
        {
            0 => ordenamiento.Ascendente 
                ? juegos.OrderBy(j => j.Nombre, StringComparer.CurrentCultureIgnoreCase).ToList()
                : juegos.OrderByDescending(j => j.Nombre, StringComparer.CurrentCultureIgnoreCase).ToList(),
            1 => ordenamiento.Ascendente 
                ? juegos.OrderBy(j => j.Genero, StringComparer.CurrentCultureIgnoreCase).ToList()
                : juegos.OrderByDescending(j => j.Genero, StringComparer.CurrentCultureIgnoreCase).ToList(),
            2 => ordenamiento.Ascendente 
                ? juegos.OrderBy(j => j.Path, StringComparer.CurrentCultureIgnoreCase).ToList()
                : juegos.OrderByDescending(j => j.Path, StringComparer.CurrentCultureIgnoreCase).ToList(),
            3 => ordenamiento.Ascendente 
                ? juegos.OrderBy(j => j.Slave, StringComparer.CurrentCultureIgnoreCase).ToList()
                : juegos.OrderByDescending(j => j.Slave, StringComparer.CurrentCultureIgnoreCase).ToList(),
            _ => juegos.ToList()
        };
    }

    public HashSet<string> CalcularNombresDuplicados(List<Juego> juegos)
    {
        return juegos
            .GroupBy(j => j.Nombre, StringComparer.CurrentCultureIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet(StringComparer.CurrentCultureIgnoreCase);
    }

    public List<string> ObtenerGenerosUnicos(List<Juego> juegos)
    {
        var generos = juegos
            .Where(j => !string.IsNullOrWhiteSpace(j.Genero) && 
                       !j.Genero.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
            .Select(j => j.Genero)
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .OrderBy(g => g, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        // Asegurar que "Unknown" esté al final si existe
        if (juegos.Any(j => j.Genero.Equals("Unknown", StringComparison.OrdinalIgnoreCase)))
        {
            generos.Add("Unknown");
        }

        return generos;
    }

    public List<Juego> AplicarQuickTag(List<Juego> juegos, List<int> indices, string tag)
    {
        var resultado = new List<Juego>(juegos);

        foreach (var indice in indices)
        {
            if (indice >= 0 && indice < resultado.Count)
            {
                var juego = resultado[indice];
                var juegoActualizado = new Juego
                {
                    Nombre = juego.Nombre + tag,
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
                resultado[indice] = juegoActualizado;
            }
        }

        return resultado;
    }

    public List<Juego> LimpiarLista()
    {
        return new List<Juego>();
    }

    public (List<Juego> juegos, List<int> indices) BuscarIndicesPorTexto(List<Juego> juegos, string textoBusqueda)
    {
        var indices = new List<int>();
        var juegosFiltrados = new List<Juego>();

        if (string.IsNullOrWhiteSpace(textoBusqueda))
        {
            return (juegos, Enumerable.Range(0, juegos.Count).ToList());
        }

        var busqueda = textoBusqueda.ToLowerInvariant();

        for (int i = 0; i < juegos.Count; i++)
        {
            var juego = juegos[i];
            if (juego.Nombre.ToLowerInvariant().Contains(busqueda) ||
                (!string.IsNullOrWhiteSpace(juego.NombreCorto) && juego.NombreCorto.ToLowerInvariant().Contains(busqueda)) ||
                juego.Genero.ToLowerInvariant().Contains(busqueda))
            {
                indices.Add(i);
                juegosFiltrados.Add(juego);
            }
        }

        return (juegosFiltrados, indices);
    }

    public Juego EditarJuego(Juego juegoOriginal, string nombre, string genero, string path, string slave, 
                           string nombreCorto, string dato1, string dato2, string dato3, string dato4)
    {
        return new Juego
        {
            Nombre = nombre,
            Genero = genero,
            Path = path,
            Slave = slave,
            NombreCorto = nombreCorto,
            Dato1 = dato1,
            Dato2 = dato2,
            Dato3 = dato3,
            Dato4 = dato4,
            EsDesconocido = string.IsNullOrWhiteSpace(genero) || genero.Equals("Unknown", StringComparison.OrdinalIgnoreCase)
        };
    }

    public Juego CopiarJuego(Juego juego)
    {
        return new Juego
        {
            Nombre = juego.Nombre,
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

    public (int total, int mostrados, int duplicados, int desconocidos) ObtenerEstadisticas(List<Juego> juegos, HashSet<string> nombresDuplicados)
    {
        var total = juegos.Count;
        var duplicados = juegos.Count(j => nombresDuplicados.Contains(j.Nombre));
        var desconocidos = juegos.Count(j => j.EsDesconocido);
        var mostrados = total - duplicados - desconocidos;

        return (total, mostrados, duplicados, desconocidos);
    }

    public void ActualizarNombresCortos(List<Juego> juegos, int maxLongitud = 31)
    {
        for (int i = 0; i < juegos.Count; i++)
        {
            var juego = juegos[i];
            if (string.IsNullOrWhiteSpace(juego.Nombre))
            {
                juegos[i] = juego with { NombreCorto = string.Empty };
                continue;
            }

            var nombreNormalizado = juego.Nombre.Trim();
            
            // Si ya es corto, mantener tal cual
            string nombreCorto;
            if (nombreNormalizado.Length <= maxLongitud)
            {
                nombreCorto = nombreNormalizado;
            }
            else if (maxLongitud < nombreNormalizado.Length && nombreNormalizado[maxLongitud] == ' ')
            {
                nombreCorto = nombreNormalizado.Substring(0, maxLongitud).TrimEnd();
            }
            else
            {
                var ultimoEspacio = nombreNormalizado.LastIndexOf(' ', maxLongitud - 1);
                nombreCorto = ultimoEspacio > 0
                    ? nombreNormalizado.Substring(0, ultimoEspacio)
                    : nombreNormalizado.Substring(0, maxLongitud).TrimEnd();
            }
            
            juegos[i] = juego with { NombreCorto = nombreCorto };
        }
    }

    public List<Juego> AplicarTitleCase(List<Juego> juegos, int titleCaseIndex)
    {
        return juegos.Select(juego =>
        {
            var nombreJuego = AplicarTitleCaseTexto(juego.Nombre, titleCaseIndex);
            var generoJuego = AplicarTitleCaseTexto(juego.Genero, titleCaseIndex);

            return new Juego
            {
                Nombre = nombreJuego,
                Genero = generoJuego,
                Path = juego.Path,
                Slave = juego.Slave,
                NombreCorto = AplicarTitleCaseTexto(juego.NombreCorto, titleCaseIndex),
                Dato1 = juego.Dato1,
                Dato2 = juego.Dato2,
                Dato3 = juego.Dato3,
                Dato4 = juego.Dato4,
                EsDesconocido = juego.EsDesconocido
            };
        }).ToList();
    }

    private static string AplicarTitleCaseTexto(string texto, int titleCaseIndex)
    {
        return titleCaseIndex switch
        {
            1 => texto.ToLowerInvariant(),
            2 => texto.ToUpperInvariant(),
            _ => texto
        };
    }

    // Métodos para la interfaz
    public List<Juego> FiltrarJuegos(List<Juego> juegos, string texto, bool mostrarDuplicados, bool mostrarDesconocidos)
    {
        var filtros = new FiltrosJuegos
        {
            TextoBusqueda = texto,
            VerDuplicados = mostrarDuplicados,
            VerDesconocidos = mostrarDesconocidos
        };

        var nombresDuplicados = juegos
            .GroupBy(j => j.Nombre.ToLowerInvariant())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        return AplicarFiltros(juegos, filtros, nombresDuplicados).ToList();
    }

    public List<Juego> OrdenarJuegos(List<Juego> juegos, int columna, bool ascendente)
    {
        var ordenamiento = new OrdenamientoJuegos
        {
            Columna = columna,
            Ascendente = ascendente
        };

        return AplicarOrdenamiento(juegos, ordenamiento).ToList();
    }

    public void QuickTag(List<Juego> juegos, string tag)
    {
        var indices = Enumerable.Range(0, juegos.Count).ToList();
        AplicarQuickTag(juegos, indices, tag);
    }

    // Método privado para ordenamiento
    private IEnumerable<Juego> AplicarOrdenamiento(List<Juego> juegos, OrdenamientoJuegos ordenamiento)
    {
        return ordenamiento.Columna switch
        {
            0 => ordenamiento.Ascendente ? juegos.OrderBy(j => j.Nombre) : juegos.OrderByDescending(j => j.Nombre),
            1 => ordenamiento.Ascendente ? juegos.OrderBy(j => j.Genero) : juegos.OrderByDescending(j => j.Genero),
            2 => ordenamiento.Ascendente ? juegos.OrderBy(j => j.Slave) : juegos.OrderByDescending(j => j.Slave),
            3 => ordenamiento.Ascendente ? juegos.OrderBy(j => j.Path) : juegos.OrderByDescending(j => j.Path),
            4 => ordenamiento.Ascendente ? juegos.OrderBy(j => j.NombreCorto) : juegos.OrderByDescending(j => j.NombreCorto),
            _ => juegos
        };
    }
}
