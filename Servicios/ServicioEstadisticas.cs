using System.Collections.Generic;
using System.Linq;
using IgameToolsWinForms.Modelos;
using IgameToolsWinForms.Interfaces;

namespace IgameToolsWinForms.Servicios;

public class ServicioEstadisticas : IServicioEstadisticas
{
    public Estadisticas CalcularEstadisticas(List<Juego> juegos)
    {
        if (juegos == null || juegos.Count == 0)
        {
            return new Estadisticas();
        }

        var estadisticas = new Estadisticas
        {
            TotalJuegos = juegos.Count
        };

        // Calcular juegos únicos y duplicados
        var nombresAgrupados = juegos
            .GroupBy(j => j.Nombre)
            .ToDictionary(g => g.Key, g => g.Count());

        estadisticas.JuegosUnicos = nombresAgrupados.Count;
        estadisticas.Duplicados = nombresAgrupados.Values.Count(c => c > 1);
        estadisticas.PorcentajeDuplicados = estadisticas.TotalJuegos > 0 
            ? (double)estadisticas.Duplicados / estadisticas.TotalJuegos * 100 
            : 0;

        // Calcular desconocidos
        estadisticas.Desconocidos = juegos.Count(j => j.EsDesconocido);
        estadisticas.PorcentajeDesconocidos = estadisticas.TotalJuegos > 0 
            ? (double)estadisticas.Desconocidos / estadisticas.TotalJuegos * 100 
            : 0;

        // Calcular actualizados (juegos que no son desconocidos y tienen datos válidos)
        estadisticas.Actualizados = juegos.Count(j => 
            !j.EsDesconocido && 
            !string.IsNullOrWhiteSpace(j.Genero) && 
            j.Genero != "Unknown");
        estadisticas.PorcentajeActualizados = estadisticas.TotalJuegos > 0 
            ? (double)estadisticas.Actualizados / estadisticas.TotalJuegos * 100 
            : 0;

        // Distribución por género
        estadisticas.DistribucionGeneros = juegos
            .Where(j => !string.IsNullOrWhiteSpace(j.Genero))
            .GroupBy(j => j.Genero)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .ToDictionary(g => g.Key, g => g.Count());

        estadisticas.GenerosUnicos = estadisticas.DistribucionGeneros.Count;

        if (estadisticas.DistribucionGeneros.Any())
        {
            var generoTop = estadisticas.DistribucionGeneros.First();
            estadisticas.GeneroMasComun = generoTop.Key;
            estadisticas.JuegosDelGeneroMasComun = generoTop.Value;
        }

        // Longitud promedio de nombres
        estadisticas.LongitudPromedioNombres = juegos.Any() 
            ? (int)juegos.Average(j => j.Nombre?.Length ?? 0) 
            : 0;

        // Tasa de actualización (qué tan completa está la información)
        var camposCompletos = juegos.Count(j => 
            !string.IsNullOrWhiteSpace(j.Nombre) &&
            !string.IsNullOrWhiteSpace(j.Genero) &&
            !string.IsNullOrWhiteSpace(j.Slave) &&
            !string.IsNullOrWhiteSpace(j.Path));

        estadisticas.TasaActualizacion = estadisticas.TotalJuegos > 0 
            ? (double)camposCompletos / estadisticas.TotalJuegos * 100 
            : 0;

        return estadisticas;
    }

    public string FormatearEstadisticasTexto(Estadisticas estadisticas)
    {
        if (estadisticas.TotalJuegos == 0)
        {
            return "No hay juegos para analizar.";
        }

        var texto = new System.Text.StringBuilder();
        texto.AppendLine("=== ESTADÍSTICAS DE LA LISTA DE JUEGOS ===");
        texto.AppendLine();
        
        texto.AppendLine("📊 ESTADÍSTICAS BÁSICAS:");
        texto.AppendLine($"• Total de juegos: {estadisticas.TotalJuegos:N0}");
        texto.AppendLine($"• Juegos únicos: {estadisticas.JuegosUnicos:N0}");
        texto.AppendLine($"• Duplicados: {estadisticas.Duplicados:N0} ({estadisticas.PorcentajeDuplicados:F1}%)");
        texto.AppendLine($"• Desconocidos: {estadisticas.Desconocidos:N0} ({estadisticas.PorcentajeDesconocidos:F1}%)");
        texto.AppendLine($"• Actualizados: {estadisticas.Actualizados:N0} ({estadisticas.PorcentajeActualizados:F1}%)");
        texto.AppendLine();

        texto.AppendLine("📈 ESTADÍSTICAS AVANZADAS:");
        texto.AppendLine($"• Longitud promedio de nombres: {estadisticas.LongitudPromedioNombres} caracteres");
        texto.AppendLine($"• Géneros únicos: {estadisticas.GenerosUnicos:N0}");
        texto.AppendLine($"• Tasa de actualización: {estadisticas.TasaActualizacion:F1}%");
        
        if (!string.IsNullOrWhiteSpace(estadisticas.GeneroMasComun))
        {
            texto.AppendLine($"• Género más común: {estadisticas.GeneroMasComun} ({estadisticas.JuegosDelGeneroMasComun:N0} juegos)");
        }
        
        texto.AppendLine();

        if (estadisticas.DistribucionGeneros.Any())
        {
            texto.AppendLine("🎮 DISTRIBUCIÓN POR GÉNERO (Top 10):");
            foreach (var genero in estadisticas.DistribucionGeneros)
            {
                var porcentaje = estadisticas.TotalJuegos > 0 
                    ? (double)genero.Value / estadisticas.TotalJuegos * 100 
                    : 0;
                texto.AppendLine($"  • {genero.Key}: {genero.Value:N0} ({porcentaje:F1}%)");
            }
        }

        texto.AppendLine();
        texto.AppendLine($"Generado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

        return texto.ToString();
    }
}
