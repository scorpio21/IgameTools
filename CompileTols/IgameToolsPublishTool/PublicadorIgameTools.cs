using System.Diagnostics;
using System.IO.Compression;
using System.Text;

namespace IgameToolsPublishTool;

internal sealed class PublicadorIgameTools
{
    public string RutaProyecto { get; }

    private string RutaCsproj => Path.Combine(RutaProyecto, "IgameToolsWinForms.csproj");

    public PublicadorIgameTools(string rutaProyecto)
    {
        RutaProyecto = rutaProyecto;
    }

    public static string? DetectarRutaIscc()
    {
        var rutas = new[]
        {
            @"C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
            @"C:\Program Files\Inno Setup 6\ISCC.exe",
        };

        return rutas.FirstOrDefault(File.Exists);
    }

    public async Task EjecutarBuildAsync(Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        await EjecutarProcesoAsync(
            archivo: "dotnet",
            argumentos: $"build \"{RutaCsproj}\"",
            escribirLogAsync,
            cancellationToken);
    }

    public Task LimpiarPublishAsync(Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LimpiarCarpetaPublish();
        return escribirLogAsync("publish limpiado");
    }

    public async Task EjecutarPublishSingleFileAsync(bool limpiarAntes, Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        if (limpiarAntes)
        {
            LimpiarCarpetaPublish();
            await escribirLogAsync("publish limpiado");
        }

        var argumentos = $"publish \"{RutaCsproj}\" -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=false -o \"{Path.Combine(RutaProyecto, "publish", "win-x64-singlefile")}\"";
        await EjecutarProcesoAsync(
            archivo: "dotnet",
            argumentos,
            escribirLogAsync,
            cancellationToken);
    }

    public Task CopiarRecursosAsync(Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var destino = Path.Combine(RutaProyecto, "publish", "win-x64-singlefile");
        if (!Directory.Exists(destino))
        {
            throw new InvalidOperationException($"No existe: {destino}");
        }

        var img = Path.Combine(RutaProyecto, "img");
        if (Directory.Exists(img))
        {
            CopiarDirectorio(img, Path.Combine(destino, "img"));
        }

        var csv = Path.Combine(RutaProyecto, "csv");
        if (Directory.Exists(csv))
        {
            CopiarDirectorio(csv, Path.Combine(destino, "csv"));
        }

        return escribirLogAsync("Recursos copiados (img/csv)");
    }

    public Task CrearZipPortableAsync(string version, Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        return CrearZipAsync($"publish/IgameTools_{version}_Portable.zip", escribirLogAsync, cancellationToken);
    }

    public Task CrearZipSingleFileAsync(string version, Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        return CrearZipAsync($"publish/IgameTools_{version}_win-x64_singlefile.zip", escribirLogAsync, cancellationToken);
    }

    public async Task CrearInstaladorAsync(string rutaIscc, string version, Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        if (!File.Exists(rutaIscc))
        {
            throw new InvalidOperationException($"No existe ISCC.exe: {rutaIscc}");
        }

        var rutaInstallerIss = Path.Combine(RutaProyecto, "installer.iss");
        if (!File.Exists(rutaInstallerIss))
        {
            throw new InvalidOperationException($"No existe: {rutaInstallerIss}");
        }

        var carpetaSalida = Path.Combine(RutaProyecto, "publish");
        Directory.CreateDirectory(carpetaSalida);

        var argumentos = $"/Sspawn=0 /V=1 /O\"{carpetaSalida}\" /DMyAppVersion={version} \"{rutaInstallerIss}\"";
        await EjecutarProcesoAsync(rutaIscc, argumentos, escribirLogAsync, cancellationToken);

        await escribirLogAsync($"Instalador generado en: {carpetaSalida}");
    }

    private void LimpiarCarpetaPublish()
    {
        var publish = Path.Combine(RutaProyecto, "publish");
        if (!Directory.Exists(publish))
        {
            return;
        }

        foreach (var dir in Directory.GetDirectories(publish))
        {
            Directory.Delete(dir, true);
        }

        foreach (var file in Directory.GetFiles(publish))
        {
            File.Delete(file);
        }
    }

    private Task CrearZipAsync(string rutaRelativaSalida, Func<string, Task> escribirLogAsync, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var origen = Path.Combine(RutaProyecto, "publish", "win-x64-singlefile");
        if (!Directory.Exists(origen))
        {
            throw new InvalidOperationException($"No existe: {origen}");
        }

        var salida = Path.Combine(RutaProyecto, rutaRelativaSalida.Replace('/', Path.DirectorySeparatorChar));
        var carpetaSalida = Path.GetDirectoryName(salida);
        if (!string.IsNullOrWhiteSpace(carpetaSalida))
        {
            Directory.CreateDirectory(carpetaSalida);
        }

        if (File.Exists(salida))
        {
            File.Delete(salida);
        }

        ZipFile.CreateFromDirectory(origen, salida, CompressionLevel.Optimal, includeBaseDirectory: false);
        return escribirLogAsync($"ZIP creado: {salida}");
    }

    private static void CopiarDirectorio(string origen, string destino)
    {
        Directory.CreateDirectory(destino);

        foreach (var archivo in Directory.GetFiles(origen))
        {
            var nombre = Path.GetFileName(archivo);
            File.Copy(archivo, Path.Combine(destino, nombre), overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(origen))
        {
            var nombre = Path.GetFileName(dir);
            CopiarDirectorio(dir, Path.Combine(destino, nombre));
        }
    }

    private async Task EjecutarProcesoAsync(
        string archivo,
        string argumentos,
        Func<string, Task> escribirLogAsync,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var psi = new ProcessStartInfo
        {
            FileName = archivo,
            Arguments = argumentos,
            WorkingDirectory = RutaProyecto,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        psi.StandardOutputEncoding = Encoding.UTF8;
        psi.StandardErrorEncoding = Encoding.UTF8;

        using var proceso = new Process { StartInfo = psi, EnableRaisingEvents = true };

        await escribirLogAsync($"> {archivo} {argumentos}");

        proceso.OutputDataReceived += async (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                await escribirLogAsync(e.Data);
            }
        };

        proceso.ErrorDataReceived += async (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                await escribirLogAsync(e.Data);
            }
        };

        if (!proceso.Start())
        {
            throw new InvalidOperationException("No se pudo iniciar el proceso.");
        }

        proceso.BeginOutputReadLine();
        proceso.BeginErrorReadLine();

        try
        {
            await proceso.WaitForExitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            try
            {
                if (!proceso.HasExited)
                {
                    proceso.Kill(entireProcessTree: true);
                }
            }
            catch
            {
            }

            throw;
        }

        if (proceso.ExitCode != 0)
        {
            throw new InvalidOperationException($"El proceso falló con ExitCode={proceso.ExitCode}");
        }
    }
}
