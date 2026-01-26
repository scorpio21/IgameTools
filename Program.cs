using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IgameToolsWinForms.Interfaces;
using IgameToolsWinForms.Servicios;

namespace IgameToolsWinForms;

internal static class Program
{
    [STAThread]
    private static async Task Main()
    {
        // Configurar el host con Dependency Injection
        var host = CreateHostBuilder().Build();

        // Configurar logging
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Iniciando IgameToolsWinForms v0.2.0");

        try
        {
            // Configurar aplicación WinForms
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Crear y ejecutar el formulario principal con DI
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var mainForm = new FormPrincipal(services);
            
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fatal al iniciar la aplicación");
            MessageBox.Show($"Error fatal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            logger.LogInformation("Cerrando IgameToolsWinForms");
        }
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", 
                    optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // Configurar logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                });

                // Registrar configuración
                services.Configure<ApplicationSettings>(context.Configuration.GetSection("Application"));
                services.Configure<CsvSettings>(context.Configuration.GetSection("Csv"));
                services.Configure<FtpSettings>(context.Configuration.GetSection("Ftp"));
                services.Configure<UISettings>(context.Configuration.GetSection("UI"));

                // Registrar servicios
                services.AddSingleton<IServicioCsv, ServicioCsv>();
                services.AddSingleton<IServicioFixList, ServicioFixList>();
                services.AddSingleton<IServicioJuegos, ServicioJuegos>();
                services.AddSingleton<IServicioEstadisticas, ServicioEstadisticas>();
                services.AddSingleton<IServicioEstadisticasFixList, ServicioEstadisticasFixList>();
                services.AddSingleton<IServicioBusquedaAvanzada, ServicioBusquedaAvanzada>();

                // Registrar servicios de UI (ViewModels)
                services.AddTransient<FormPrincipal>();
                services.AddTransient<FormEditarJuego>();
                services.AddTransient<FormBusquedaAvanzada>();
                services.AddTransient<FormAyuda>();
            });
    }
}

// Clases de configuración
public class ApplicationSettings
{
    public string Name { get; set; } = "IgameToolsWinForms";
    public string Version { get; set; } = "0.2.0";
    public int MaxUndoSteps { get; set; } = 50;
    public string DefaultCulture { get; set; } = "es-ES";
}

public class CsvSettings
{
    public string DefaultEncoding { get; set; } = "UTF-8";
    public string Delimiter { get; set; } = ";";
    public bool BackupEnabled { get; set; } = true;
    public int MaxBackupFiles { get; set; } = 5;
}

public class FtpSettings
{
    public string Server { get; set; } = "ftp.grandis.nu";
    public string Username { get; set; } = "ftp";
    public string Password { get; set; } = "amiga";
    public int Timeout { get; set; } = 30000;
    public int RetryCount { get; set; } = 3;
}

public class UISettings
{
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "es-ES";
    public int AutoSaveInterval { get; set; } = 300;
}
