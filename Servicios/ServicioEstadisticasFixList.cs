using System;
using System.Collections.Generic;
using System.Linq;
using IgameToolsWinForms.Modelos;

namespace IgameToolsWinForms.Servicios;

public class ServicioEstadisticasFixList
{
    public EstadisticasFixList CalcularEstadisticasFixList(
        List<Juego> juegosEntrada, 
        List<Juego> juegosSalida, 
        string directorioTrabajo,
        TimeSpan duracion)
    {
        if (juegosEntrada == null || juegosSalida == null)
            return new EstadisticasFixList();

        var estadisticas = new EstadisticasFixList
        {
            TotalJuegosProcesados = juegosEntrada.Count,
            DirectorioTrabajo = directorioTrabajo ?? string.Empty,
            FechaEjecucion = DateTime.Now,
            DuracionProceso = duracion
        };

        // Comparar juegos entrada vs salida para detectar cambios
        for (int i = 0; i < Math.Min(juegosEntrada.Count, juegosSalida.Count); i++)
        {
            var juegoEntrada = juegosEntrada[i];
            var juegoSalida = juegosSalida[i];

            // Detectar si el juego fue actualizado
            if (juegoSalida.Nombre != juegoEntrada.Nombre ||
                juegoSalida.Genero != juegoEntrada.Genero ||
                juegoSalida.Path != juegoEntrada.Path ||
                juegoSalida.Slave != juegoEntrada.Slave)
            {
                estadisticas.JuegosActualizados++;
            }

            // Detectar correcciones de género
            if (juegoSalida.Genero != juegoEntrada.Genero && 
                !string.IsNullOrWhiteSpace(juegoSalida.Genero))
            {
                estadisticas.GenerosCorregidos++;
            }

            // Detectar paths encontrados/corregidos
            if (!string.IsNullOrWhiteSpace(juegoSalida.Path) && 
                string.IsNullOrWhiteSpace(juegoEntrada.Path))
            {
                estadisticas.PathsEncontrados++;
            }
            else if (juegoSalida.Path != juegoEntrada.Path)
            {
                estadisticas.PathsCorregidos++;
            }

            // Detectar slaves encontrados
            if (!string.IsNullOrWhiteSpace(juegoSalida.Slave) && 
                string.IsNullOrWhiteSpace(juegoEntrada.Slave))
            {
                estadisticas.SlavesEncontrados++;
            }

            // Detectar datos extra (Dato1-Dato4)
            if (!string.IsNullOrWhiteSpace(juegoSalida.Dato1) ||
                !string.IsNullOrWhiteSpace(juegoSalida.Dato2) ||
                !string.IsNullOrWhiteSpace(juegoSalida.Dato3) ||
                !string.IsNullOrWhiteSpace(juegoSalida.Dato4))
            {
                estadisticas.JuegosConDatosExtra++;
            }
        }

        // Calcular juegos sin cambios
        estadisticas.JuegosSinCambios = estadisticas.TotalJuegosProcesados - estadisticas.JuegosActualizados;

        // Calcular porcentajes
        if (estadisticas.TotalJuegosProcesados > 0)
        {
            estadisticas.PorcentajeActualizados = (double)estadisticas.JuegosActualizados / estadisticas.TotalJuegosProcesados * 100;
            estadisticas.PorcentajeCorregidos = (double)(estadisticas.GenerosCorregidos + estadisticas.PathsCorregidos) / estadisticas.TotalJuegosProcesados * 100;
        }

        // Contar errores corregidos (juegos que tenían datos inválidos y fueron arreglados)
        estadisticas.ErroresCorregidos = juegosSalida.Count(j => 
            string.IsNullOrWhiteSpace(j.Nombre) || 
            string.IsNullOrWhiteSpace(j.Genero));

        return estadisticas;
    }

    public string FormatearEstadisticasFixListTexto(EstadisticasFixList estadisticas)
    {
        if (estadisticas == null)
            return "No hay estadísticas disponibles.";

        var texto = $"📊 ESTADÍSTICAS FIX LIST\n";
        texto += $"═════════════════════════════\n\n";
        
        texto += $"🎮 JUEGOS PROCESADOS\n";
        texto += $"• Total procesados: {estadisticas.TotalJuegosProcesados:N0}\n";
        texto += $"• Actualizados: {estadisticas.JuegosActualizados:N0} ({estadisticas.PorcentajeActualizados:F1}%)\n";
        texto += $"• Sin cambios: {estadisticas.JuegosSinCambios:N0}\n\n";

        texto += $"🔧 CORRECCIONES REALIZADAS\n";
        texto += $"• Géneros corregidos: {estadisticas.GenerosCorregidos:N0}\n";
        texto += $"• Paths encontrados: {estadisticas.PathsEncontrados:N0}\n";
        texto += $"• Paths corregidos: {estadisticas.PathsCorregidos:N0}\n";
        texto += $"• Slaves encontrados: {estadisticas.SlavesEncontrados:N0}\n";
        texto += $"• Errores corregidos: {estadisticas.ErroresCorregidos:N0}\n\n";

        texto += $"📋 DATOS ADICIONALES\n";
        texto += $"• Con datos extra: {estadisticas.JuegosConDatosExtra:N0}\n";
        texto += $"• Total corregidos: {estadisticas.PorcentajeCorregidos:F1}%\n\n";

        texto += $"⏱️ INFORMACIÓN DEL PROCESO\n";
        texto += $"• Fecha: {estadisticas.FechaEjecucion:yyyy-MM-dd HH:mm:ss}\n";
        texto += $"• Duración: {estadisticas.DuracionProceso:mm\\:ss}\n";
        texto += $"• Directorio: {estadisticas.DirectorioTrabajo}";

        return texto;
    }
}
