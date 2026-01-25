using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace IgameToolsWinForms;

public class AppSettings
{
    public string? LastCsvFile { get; set; }
    public List<string> RecentFiles { get; set; } = new();

    // Preferencias de usuario
    public bool MantenerDatos { get; set; } = true;
    public bool NombresCortos { get; set; } = false;
    public bool VerDuplicados { get; set; } = false;
    public bool VerDesconocidos { get; set; } = false;
    public int TitleCaseIndex { get; set; } = 0;

    // Ajustes de UI - Columnas escalables
    public Dictionary<string, int>? AnchoColumnas { get; set; }

    // Estado de la ventana (opcional)
    public int WindowWidth { get; set; } = 1500;
    public int WindowHeight { get; set; } = 1000;
    public int WindowLeft { get; set; } = -1; // -1 = no guardado (usar por defecto)
    public int WindowTop { get; set; } = -1;

    private static readonly string SettingsPath = Path.Combine("Json", "settings.json");

    public static AppSettings Load()
    {
        if (!File.Exists(SettingsPath))
            return new AppSettings();

        try
        {
            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, json);
        }
        catch
        {
            // Ignorar errores al guardar settings
        }
    }

    public void AddRecentFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        // Eliminar si ya existe
        RecentFiles.Remove(filePath);

        // Añadir al principio
        RecentFiles.Insert(0, filePath);

        // Mantener solo los últimos 5
        while (RecentFiles.Count > 5)
        {
            RecentFiles.RemoveAt(RecentFiles.Count - 1);
        }
    }
}
