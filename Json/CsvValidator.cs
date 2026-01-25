using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IgameToolsWinForms;

public class ValidationResult
{
    public bool IsValid { get; init; }
    public List<string> Errors { get; init; } = new();
    public List<string> Warnings { get; init; } = new();
    public int TotalLines { get; init; }
    public int ValidLines { get; init; }
    public int InvalidLines { get; init; }
}

public static class CsvValidator
{
    public static ValidationResult Validate(string filePath)
    {
        var errors = new List<string>();
        var warnings = new List<string>();
        var totalLines = 0;
        var validLines = 0;
        var invalidLines = 0;

        try
        {
            // Verificar que el archivo existe
            if (!File.Exists(filePath))
            {
                errors.Add("El archivo no existe.");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            // Verificar extensión
            if (!Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
                warnings.Add("El archivo no tiene extensión .csv. Podría no ser un archivo CSV válido.");
            }

            // Verificar tamaño (evitar archivos vacíos o excesivamente grandes)
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0)
            {
                errors.Add("El archivo está vacío.");
                return new ValidationResult { IsValid = false, Errors = errors };
            }

            if (fileInfo.Length > 50 * 1024 * 1024) // 50MB
            {
                warnings.Add("El archivo es muy grande (>50MB) y podría tardar en cargarse.");
            }

            // Analizar contenido
            var lineasConProblemas = new List<int>();
            var lineasConCamposInsuficientes = new List<int>();
            var lineasConCamposExcesivos = new List<int>();
            var maxCampos = 0;
            var minCampos = int.MaxValue;

            using var reader = new StreamReader(filePath, Encoding.UTF8, true);
            string? line;
            int lineNumber = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                totalLines++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue; // Ignorar líneas vacías
                }

                try
                {
                    var campos = ParsearLineaCsv(line, ';');
                    
                    if (campos.Count < 4)
                    {
                        lineasConCamposInsuficientes.Add(lineNumber);
                        invalidLines++;
                        continue;
                    }

                    if (campos.Count > 8)
                    {
                        lineasConCamposExcesivos.Add(lineNumber);
                    }

                    maxCampos = Math.Max(maxCampos, campos.Count);
                    minCampos = Math.Min(minCampos, campos.Count);

                    // Validaciones básicas de campos obligatorios
                    var nombre = campos.Count > 1 ? campos[1] : string.Empty;
                    var genero = campos.Count > 2 ? campos[2] : string.Empty;
                    var ruta = campos.Count > 3 ? campos[3] : string.Empty;

                    if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(ruta))
                    {
                        lineasConProblemas.Add(lineNumber);
                        invalidLines++;
                        continue;
                    }

                    validLines++;
                }
                catch
                {
                    lineasConProblemas.Add(lineNumber);
                    invalidLines++;
                }
            }

            // Generar mensajes específicos
            if (lineasConCamposInsuficientes.Any())
            {
                var muestra = string.Join(", ", lineasConCamposInsuficientes.Take(5));
                var resto = lineasConCamposInsuficientes.Count - 5;
                errors.Add($"Líneas con menos de 4 campos (mínimo requerido): {muestra}{(resto > 0 ? $" y {resto} más..." : "")}");
            }

            if (lineasConCamposExcesivos.Any())
            {
                var muestra = string.Join(", ", lineasConCamposExcesivos.Take(3));
                var resto = lineasConCamposExcesivos.Count - 3;
                warnings.Add($"Líneas con más de 8 campos: {muestra}{(resto > 0 ? $" y {resto} más..." : "")}");
            }

            if (lineasConProblemas.Any())
            {
                var muestra = string.Join(", ", lineasConProblemas.Take(3));
                var resto = lineasConProblemas.Count - 3;
                errors.Add($"Líneas con errores de formato: {muestra}{(resto > 0 ? $" y {resto} más..." : "")}");
            }

            if (validLines == 0)
            {
                errors.Add("No se encontraron líneas válidas en el archivo.");
            }

            if (minCampos != int.MaxValue && maxCampos > 0)
            {
                if (minCampos != maxCampos)
                {
                    warnings.Add($"El archivo tiene un número inconsistente de columnas: entre {minCampos} y {maxCampos} columnas.");
                }
                else if (maxCampos != 8)
                {
                    warnings.Add($"El archivo tiene {maxCampos} columnas. El formato esperado es 8 columnas.");
                }
            }

            var isValid = !errors.Any() && validLines > 0;
            return new ValidationResult
            {
                IsValid = isValid,
                Errors = errors,
                Warnings = warnings,
                TotalLines = totalLines,
                ValidLines = validLines,
                InvalidLines = invalidLines
            };
        }
        catch (UnauthorizedAccessException)
        {
            errors.Add("No tienes permisos para leer este archivo.");
        }
        catch (IOException ex)
        {
            errors.Add($"Error al leer el archivo: {ex.Message}");
        }
        catch (Exception ex)
        {
            errors.Add($"Error inesperado: {ex.Message}");
        }

        return new ValidationResult { IsValid = false, Errors = errors };
    }

    private static List<string> ParsearLineaCsv(string linea, char separador)
    {
        var resultado = new List<string>();
        var sb = new StringBuilder();
        var dentroDeComillas = false;

        for (var i = 0; i < linea.Length; i++)
        {
            var c = linea[i];

            if (c == '"')
            {
                if (dentroDeComillas && i + 1 < linea.Length && linea[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                    continue;
                }

                dentroDeComillas = !dentroDeComillas;
                continue;
            }

            if (c == separador && !dentroDeComillas)
            {
                resultado.Add(sb.ToString());
                sb.Clear();
                continue;
            }

            sb.Append(c);
        }

        resultado.Add(sb.ToString());
        return resultado;
    }
}
