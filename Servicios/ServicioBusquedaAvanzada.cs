using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IgameToolsWinForms.Interfaces;

namespace IgameToolsWinForms.Servicios;

public class ServicioBusquedaAvanzada : IServicioBusquedaAvanzada
{
    public List<Juego> Buscar(List<Juego> juegos, string campo, string tipo, string termino, bool mayusculas, bool usarRegex)
    {
        if (juegos == null || juegos.Count == 0 || string.IsNullOrWhiteSpace(termino))
        {
            return new List<Juego>();
        }

        try
        {
            var resultados = new List<Juego>();
            var terminoBusqueda = mayusculas ? termino : termino.ToLowerInvariant();

            foreach (var juego in juegos)
            {
                if (CumpleCriterio(juego, campo, tipo, terminoBusqueda, mayusculas, usarRegex))
                {
                    resultados.Add(juego);
                }
            }

            return resultados;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al realizar búsqueda: {ex.Message}");
        }
    }

    private bool CumpleCriterio(Juego juego, string campo, string tipo, string termino, bool mayusculas, bool usarRegex)
    {
        var valores = ObtenerValoresCampo(juego, campo, mayusculas);

        foreach (var valor in valores)
        {
            if (string.IsNullOrWhiteSpace(valor))
                continue;

            if (usarRegex && tipo == "Regex")
            {
                try
                {
                    if (Regex.IsMatch(valor, termino, mayusculas ? RegexOptions.None : RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                }
                catch
                {
                    // Si hay error en el regex, continuar con búsqueda normal
                    continue;
                }
            }

            switch (tipo)
            {
                case "Contiene":
                    if (valor.Contains(termino))
                        return true;
                    break;

                case "Exacto":
                    if (valor.Equals(termino, mayusculas ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                        return true;
                    break;

                case "Comienza con":
                    if (valor.StartsWith(termino, mayusculas ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                        return true;
                    break;

                case "Termina con":
                    if (valor.EndsWith(termino, mayusculas ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                        return true;
                    break;

                case "No contiene":
                    if (!valor.Contains(termino))
                        return true;
                    break;
            }
        }

        return false;
    }

    private List<string> ObtenerValoresCampo(Juego juego, string campo, bool mayusculas)
    {
        var valores = new List<string>();

        switch (campo)
        {
            case "Nombre":
                if (!string.IsNullOrWhiteSpace(juego.Nombre))
                    valores.Add(mayusculas ? juego.Nombre : juego.Nombre.ToLowerInvariant());
                break;

            case "Nombre Corto":
                if (!string.IsNullOrWhiteSpace(juego.NombreCorto))
                    valores.Add(mayusculas ? juego.NombreCorto : juego.NombreCorto.ToLowerInvariant());
                break;

            case "Género":
                if (!string.IsNullOrWhiteSpace(juego.Genero))
                    valores.Add(mayusculas ? juego.Genero : juego.Genero.ToLowerInvariant());
                break;

            case "Slave":
                if (!string.IsNullOrWhiteSpace(juego.Slave))
                    valores.Add(mayusculas ? juego.Slave : juego.Slave.ToLowerInvariant());
                break;

            case "Ruta":
                if (!string.IsNullOrWhiteSpace(juego.Path))
                    valores.Add(mayusculas ? juego.Path : juego.Path.ToLowerInvariant());
                break;

            case "Todos":
                // Buscar en todos los campos
                if (!string.IsNullOrWhiteSpace(juego.Nombre))
                    valores.Add(mayusculas ? juego.Nombre : juego.Nombre.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(juego.NombreCorto))
                    valores.Add(mayusculas ? juego.NombreCorto : juego.NombreCorto.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(juego.Genero))
                    valores.Add(mayusculas ? juego.Genero : juego.Genero.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(juego.Slave))
                    valores.Add(mayusculas ? juego.Slave : juego.Slave.ToLowerInvariant());
                if (!string.IsNullOrWhiteSpace(juego.Path))
                    valores.Add(mayusculas ? juego.Path : juego.Path.ToLowerInvariant());
                break;
        }

        return valores;
    }

    public List<string> SugerenciasTerminos(List<Juego> juegos, string campo, string partialTerm, int maxSuggestions = 10)
    {
        if (juegos == null || juegos.Count == 0 || string.IsNullOrWhiteSpace(partialTerm))
        {
            return new List<string>();
        }

        var sugerencias = new HashSet<string>();
        var partialLower = partialTerm.ToLowerInvariant();

        foreach (var juego in juegos)
        {
            var valores = ObtenerValoresCampo(juego, campo, false);

            foreach (var valor in valores)
            {
                if (string.IsNullOrWhiteSpace(valor))
                    continue;

                // Buscar términos que contienen el texto parcial
                if (valor.ToLowerInvariant().Contains(partialLower))
                {
                    // Extraer palabras completas que contienen el término parcial
                    var palabras = Regex.Split(valor, @"\s+")
                        .Where(p => p.ToLowerInvariant().Contains(partialLower))
                        .Take(maxSuggestions);

                    foreach (var palabra in palabras)
                    {
                        sugerencias.Add(palabra);
                    }
                }
            }
        }

        return sugerencias
            .Take(maxSuggestions)
            .OrderBy(s => s.Length)
            .ThenBy(s => s)
            .ToList();
    }

    public EstadisticasBusqueda ObtenerEstadisticasBusqueda(List<Juego> juegos, List<Juego> resultados)
    {
        var totalJuegos = juegos?.Count ?? 0;
        var totalResultados = resultados?.Count ?? 0;

        return new EstadisticasBusqueda
        {
            TotalJuegos = totalJuegos,
            TotalResultados = totalResultados,
            PorcentajeEncontrados = totalJuegos > 0 ? (double)totalResultados / totalJuegos * 100 : 0,
            TiempoBusqueda = DateTime.Now
        };
    }

    // Método para la interfaz
    public List<Juego> BuscarJuegos(List<Juego> juegos, string nombre, string genero, string slave, string ruta, bool busquedaExacta)
    {
        if (juegos == null || juegos.Count == 0)
            return new List<Juego>();

        IEnumerable<Juego> resultados = juegos;

        // Filtrar por nombre
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            if (busquedaExacta)
                resultados = resultados.Where(j => j.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            else
                resultados = resultados.Where(j => j.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
        }

        // Filtrar por género
        if (!string.IsNullOrWhiteSpace(genero))
        {
            if (busquedaExacta)
                resultados = resultados.Where(j => j.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));
            else
                resultados = resultados.Where(j => j.Genero.Contains(genero, StringComparison.OrdinalIgnoreCase));
        }

        // Filtrar por slave
        if (!string.IsNullOrWhiteSpace(slave))
        {
            if (busquedaExacta)
                resultados = resultados.Where(j => j.Slave.Equals(slave, StringComparison.OrdinalIgnoreCase));
            else
                resultados = resultados.Where(j => j.Slave.Contains(slave, StringComparison.OrdinalIgnoreCase));
        }

        // Filtrar por ruta
        if (!string.IsNullOrWhiteSpace(ruta))
        {
            if (busquedaExacta)
                resultados = resultados.Where(j => j.Path.Equals(ruta, StringComparison.OrdinalIgnoreCase));
            else
                resultados = resultados.Where(j => j.Path.Contains(ruta, StringComparison.OrdinalIgnoreCase));
        }

        return resultados.ToList();
    }
}

public class EstadisticasBusqueda
{
    public int TotalJuegos { get; set; }
    public int TotalResultados { get; set; }
    public double PorcentajeEncontrados { get; set; }
    public DateTime TiempoBusqueda { get; set; }

    public string FormatearResultado()
    {
        return $"Encontrados {TotalResultados:N0} de {TotalJuegos:N0} juegos ({PorcentajeEncontrados:F1}%)";
    }
}
