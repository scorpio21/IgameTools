using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using IgameToolsWinForms;
using System.IO.Compression;

namespace IgameToolsWinForms.Servicios;

public class EstadoFix
{
    public string Titulo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;

    public EstadoFix() { }
    public EstadoFix(string titulo, string detalle)
    {
        Titulo = titulo;
        Detalle = detalle;
    }
}

public class ServicioFixList
{
    private readonly string _ftpHost = "ftp.grandis.nu";
    private readonly string _ftpPath = "/~Uploads/mrv2k/";
    private readonly string _ftpUser = "ftp";
    private readonly string _ftpPass = "amiga";

    public async Task<List<Juego>> EjecutarFixListAsync(string directorioTrabajo, List<Juego> juegosEntrada, IProgress<EstadoFix> progreso)
    {
        var totalInicial = juegosEntrada.Count;
        progreso.Report(new EstadoFix("Checking database...", ""));
        
        var rutaDb = await DescargarBaseDatosSiHaceFaltaAsync(directorioTrabajo, progreso);
        if (string.IsNullOrEmpty(rutaDb))
        {
            return juegosEntrada; // Retornar original si no se puede descargar DB
        }

        progreso.Report(new EstadoFix("Processing genres...", ""));
        await DescargarArchivoGenresSiHaceFaltaAsync(directorioTrabajo, progreso);

        progreso.Report(new EstadoFix("Loading game data...", ""));
        var mapa = await CargarMapaJuegosAsync(rutaDb, progreso);

        progreso.Report(new EstadoFix("Updating games...", ""));
        var salida = await Task.Run(() => ProcesarJuegos(juegosEntrada, mapa, progreso));

        progreso.Report(new EstadoFix("Done!", $"Updated {salida.Count} games"));
        
        // Mostrar ventana de resumen
       // await Task.Run(() => MostrarResumen(totalInicial, salida, mapa.Count));
        
        return salida;
    }

   /* private void MostrarResumen(int totalInicial, List<Juego> juegosSalida, int totalMapa)
    {
        try
        {
            var actualizados = juegosSalida.Count(j => j.Genero != "Unknown" && !string.IsNullOrWhiteSpace(j.Genero));
            var sinCambios = totalInicial - actualizados;
            var tasaActualizacion = totalInicial > 0 ? (actualizados * 100.0 / totalInicial) : 0;

            var resumen = $"=== RESUMEN FIX LIST ===\n\n" +
                         $"Total juegos procesados: {totalInicial:N0}\n" +
                         $"Juegos actualizados: {actualizados:N0}\n" +
                         $"Juegos sin cambios: {sinCambios:N0}\n" +
                         $"Tasa de actualización: {tasaActualizacion:F1}%\n" +
                         $"Base de datos usada: {totalMapa:N0} juegos\n\n" +
                         $"=== Fecha: ===\n" +
                         $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            System.Windows.Forms.MessageBox.Show(resumen, "Fix List - Resumen de Actualización", 
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);//TODO: Quitar
        }
        catch (Exception ex)
        {
            // Si falla la ventana, continuar sin mostrarla
            System.Diagnostics.Debug.WriteLine($"Error mostrando resumen: {ex.Message}");
        }
    }*/

    public async Task<string?> DescargarBaseDatosSiHaceFaltaAsync(string directorioTrabajo, IProgress<EstadoFix>? progreso = null)
    {
        try
        {
            progreso?.Report(new EstadoFix("Checking database...", "Verificando archivos locales"));
            
            // Buscar archivos IG_Data locales
            var archivosDb = Directory
                .EnumerateFiles(directorioTrabajo, "IG_Data*", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList();

            var dbLocal = archivosDb.FirstOrDefault();
            var nombreDbRemoto = await ObtenerNombreBaseDatosRemotaAsync(progreso);

            if (string.IsNullOrEmpty(nombreDbRemoto))
            {
                progreso?.Report(new EstadoFix("Error", "No se pudo obtener nombre de DB remota"));
                return dbLocal;
            }

            var rutaNueva = Path.Combine(directorioTrabajo, nombreDbRemoto);
            var rutaLocal = Path.Combine(directorioTrabajo, "IG_Data.dat");

            // Si ya existe IG_Data.dat, comparar con el remoto
            if (File.Exists(rutaLocal))
            {
                progreso?.Report(new EstadoFix("Comparing files...", "Verificando si IG_Data.dat está actualizado"));
                
                try
                {
                    // Obtener tamaño del archivo remoto
                    var request = (FtpWebRequest)WebRequest.Create($"ftp://{_ftpHost}{_ftpPath}{nombreDbRemoto}");
                    request.Method = WebRequestMethods.Ftp.GetFileSize;
                    request.Credentials = new NetworkCredential(_ftpUser, _ftpPass);
                    request.Timeout = 10000;
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.KeepAlive = false;

                    using var response = (FtpWebResponse)await request.GetResponseAsync();
                    long tamanoRemoto = response.ContentLength;
                    long tamanoLocal = new FileInfo(rutaLocal).Length;

                    if (tamanoLocal == tamanoRemoto)
                    {
                        progreso?.Report(new EstadoFix("Database up to date", "IG_Data.dat está actualizado"));
                        return rutaLocal;
                    }
                    else
                    {
                        progreso?.Report(new EstadoFix("Database outdated", $"IG_Data.dat local ({tamanoLocal:N0} bytes) vs remoto ({tamanoRemoto:N0} bytes)"));
                    }
                }
                catch (Exception ex)
                {
                    progreso?.Report(new EstadoFix("Size check failed", $"No se pudo comparar tamaños: {ex.Message}"));
                }
            }

            // Descargar el archivo si es necesario
            if (File.Exists(rutaLocal))
            {
                File.Delete(rutaLocal);
            }

            progreso?.Report(new EstadoFix("Downloading database...", nombreDbRemoto));
            var rutaRemota = nombreDbRemoto; // El archivo está directamente en ~Uploads/mrv2k/

            try
            {
                await DescargarFtpAsync(rutaRemota, rutaNueva, progreso);
                
                // Si descargó un archivo con extensión .dat, renombrar a IG_Data.dat
                if (Path.GetExtension(rutaNueva).ToLower() == ".dat" && Path.GetFileName(rutaNueva) != "IG_Data.dat")
                {
                    if (File.Exists(rutaLocal))
                        File.Delete(rutaLocal);
                    File.Move(rutaNueva, rutaLocal);
                    return rutaLocal;
                }
                
                return rutaNueva;
            }
            catch (Exception ex)
            {
                progreso?.Report(new EstadoFix("Download failed", ex.Message));
                return dbLocal;
            }
        }
        catch (Exception ex)
        {
            progreso?.Report(new EstadoFix("Error checking database", ex.Message));
            return null;
        }
    }

    public async Task DescargarArchivoGenresSiHaceFaltaAsync(string directorioTrabajo, IProgress<EstadoFix>? progreso = null)
    {
        var ruta = Path.Combine(directorioTrabajo, "genres");
        
        // Si ya existe genres local, comparar con el remoto
        if (File.Exists(ruta))
        {
            progreso?.Report(new EstadoFix("Comparing genres...", "Verificando si genres está actualizado"));
            
            try
            {
                // Obtener tamaño del archivo remoto
                var request = (FtpWebRequest)WebRequest.Create($"ftp://{_ftpHost}{_ftpPath}genres");
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.Credentials = new NetworkCredential(_ftpUser, _ftpPass);
                request.Timeout = 10000;
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                using var response = (FtpWebResponse)await request.GetResponseAsync();
                long tamanoRemoto = response.ContentLength;
                long tamanoLocal = new FileInfo(ruta).Length;

                if (tamanoLocal == tamanoRemoto)
                {
                    progreso?.Report(new EstadoFix("Genres file up to date", "genres está actualizado"));
                    return;
                }
                else
                {
                    progreso?.Report(new EstadoFix("Genres outdated", $"genres local ({tamanoLocal:N0} bytes) vs remoto ({tamanoRemoto:N0} bytes)"));
                }
            }
            catch (Exception ex)
            {
                progreso?.Report(new EstadoFix("Genres size check failed", $"No se pudo comparar tamaños: {ex.Message}"));
            }
        }
        else
        {
            progreso?.Report(new EstadoFix("Downloading genres...", "genres no existe localmente"));
        }

        // Descargar si es necesario
        try
        {
            await DescargarFtpAsync("genres", ruta, progreso); // genres está en ~Uploads/mrv2k/
        }
        catch (Exception ex)
        {
            progreso?.Report(new EstadoFix("Genres download failed", ex.Message));
        }
    }

    public void CrearArchivoGenresDesdeIGData(string rutaIGData, string rutaGenres)
    {
        try
        {
            using var archivo = ZipFile.OpenRead(rutaIGData);
            var entrada = archivo.Entries.FirstOrDefault(e => 
                e.FullName.Equals("IG_Data/genres", StringComparison.OrdinalIgnoreCase));

            if (entrada != null)
            {
                using var stream = entrada.Open();
                using var writer = new StreamWriter(rutaGenres, false, Encoding.UTF8);
                using var reader = new StreamReader(stream);

                string? linea;
                while ((linea = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(linea))
                    {
                        writer.WriteLine(linea);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al procesar genres desde IG_Data: {ex.Message}", ex);
        }
    }

    public async Task<bool> ProbarConexionFtpAsync(IProgress<EstadoFix>? progreso = null)
    {
        try
        {
            progreso?.Report(new EstadoFix("Probando conexión FTP...", "Conectando a ftp.grandis.nu"));
            
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{_ftpHost}{_ftpPath}");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_ftpUser, _ftpPass);
            request.Timeout = 15000; // 15 segundos para prueba
            request.ReadWriteTimeout = 15000;
            request.UsePassive = true; // Importar para firewalls
            request.UseBinary = true;
            request.KeepAlive = false; // Como en el original

            using var response = (FtpWebResponse)await request.GetResponseAsync();
            progreso?.Report(new EstadoFix("Conexión FTP exitosa", $"Servidor responde: {response.StatusCode}"));
            return true;
        }
        catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
        {
            progreso?.Report(new EstadoFix("Error de Timeout", "La conexión FTP excedió el tiempo de espera (15s). Firewall o proxy bloqueando?"));
            return false;
        }
        catch (WebException ex) when (ex.Response is FtpWebResponse ftpResponse)
        {
            progreso?.Report(new EstadoFix("Error FTP", $"Código: {ftpResponse.StatusCode}, Mensaje: {ftpResponse.StatusDescription}"));
            return false;
        }
        catch (WebException ex)
        {
            progreso?.Report(new EstadoFix("Error de Red", ex.Message));
            return false;
        }
        catch (Exception ex)
        {
            progreso?.Report(new EstadoFix("Error Inesperado", ex.Message));
            return false;
        }
    }

    public async Task<List<string>> ListarFtpAsync(string rutaRelativa, IProgress<EstadoFix>? progreso = null)
    {
        try
        {
            progreso?.Report(new EstadoFix("Conectando al FTP...", "Listando archivos"));
            
            var request = (FtpWebRequest)WebRequest.Create($"ftp://{_ftpHost}{_ftpPath}{rutaRelativa}");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_ftpUser, _ftpPass);
            request.Timeout = 10000; // 10 segundos timeout
            request.ReadWriteTimeout = 10000;
            request.UsePassive = true; // Importar para firewalls
            request.UseBinary = true;
            request.KeepAlive = false; // Como en el original

            using var response = (FtpWebResponse)await request.GetResponseAsync();
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);

            var archivos = new List<string>();
            string? linea;
            while ((linea = await reader.ReadLineAsync()) != null)
            {
                archivos.Add(linea);
            }

            progreso?.Report(new EstadoFix("Conexión FTP completada", $"Se encontraron {archivos.Count} archivos"));
            return archivos;
        }
        catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
        {
            throw new TimeoutException("La conexión FTP ha excedido el tiempo de espera (10 segundos). Verifique la conexión a internet.", ex);
        }
        catch (WebException ex) when (ex.Response is FtpWebResponse ftpResponse && ftpResponse.StatusCode == FtpStatusCode.NotLoggedIn)
        {
            throw new UnauthorizedAccessException("Credenciales FTP incorrectas. Verifique usuario y contraseña.", ex);
        }
        catch (WebException ex)
        {
            throw new InvalidOperationException($"Error de conexión FTP: {ex.Message}. Verifique que el servidor FTP esté accesible.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error inesperado al listar FTP: {ex.Message}", ex);
        }
    }

    // Métodos privados
    private async Task<string?> ObtenerNombreBaseDatosRemotaAsync(IProgress<EstadoFix>? progreso = null)
    {
        try
        {
            progreso?.Report(new EstadoFix("Checking database...", "Buscando archivos remotos"));
            
            var archivos = await ListarFtpAsync("", progreso);
            
            var dbFiles = archivos
                .Where(f => f.Contains("IG_Data", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(f => f)
                .ToList();

            return dbFiles.FirstOrDefault();
        }
        catch (Exception ex)
        {
            progreso?.Report(new EstadoFix("Error getting remote DB name", ex.Message));
            return null;
        }
    }

    private async Task DescargarFtpAsync(string rutaRemota, string rutaLocal, IProgress<EstadoFix>? progreso = null)
    {
        var request = (FtpWebRequest)WebRequest.Create($"ftp://{_ftpHost}{_ftpPath}{rutaRemota}");
        request.Method = WebRequestMethods.Ftp.DownloadFile;
        request.Credentials = new NetworkCredential(_ftpUser, _ftpPass);
        request.Timeout = 30000; // 30 segundos para descargas
        request.ReadWriteTimeout = 30000;
        request.UsePassive = true; // Importar para firewalls
        request.UseBinary = true;
        request.KeepAlive = false; // Como en el original

        using var response = (FtpWebResponse)await request.GetResponseAsync();
        using var stream = response.GetResponseStream();
        using var fileStream = File.Create(rutaLocal);

        var buffer = new byte[8192];
        int bytesRead;
        long totalBytes = response.ContentLength;
        long bytesDownloaded = 0;

        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await fileStream.WriteAsync(buffer, 0, bytesRead);
            bytesDownloaded += bytesRead;

            if (totalBytes > 0)
            {
                var porcentaje = (int)((bytesDownloaded * 100) / totalBytes);
                progreso?.Report(new EstadoFix($"Downloading... {porcentaje}%", $"{bytesDownloaded:N0} / {totalBytes:N0} bytes"));
            }
        }
    }

    private async Task<Dictionary<string, string>> CargarMapaJuegosAsync(string rutaDb, IProgress<EstadoFix> progreso)
    {
        var mapa = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            // Si es un archivo .dat, procesar como CSV
            if (Path.GetExtension(rutaDb).ToLower() == ".dat")
            {
                progreso.Report(new EstadoFix("Processing IG_Data.dat...", "Leyendo archivo de datos"));
                
                var lineas = await File.ReadAllLinesAsync(rutaDb, Encoding.UTF8);
                
                for (int i = 0; i < lineas.Length; i++)
                {
                    var linea = lineas[i].Trim();
                    if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("Slave")) // Saltar cabecera
                        continue;

                    var campos = linea.Split(';');

                    // IG_Data.dat tiene 6 campos: Slave;Nombre;Género;NombreCompleto;NombreCorto;Tipo
                    if (campos.Length >= 3)
                    {
                        var slave = campos[0]?.Trim() ?? string.Empty;
                        var genero = campos[2]?.Trim() ?? string.Empty;

                        if (!string.IsNullOrWhiteSpace(slave) && !string.IsNullOrWhiteSpace(genero))
                        {
                            mapa[slave] = genero;
                        }
                    }
                }
            }
            else
            {
                // Código original para archivos ZIP
                using var archivo = ZipFile.OpenRead(rutaDb);
                var entrada = archivo.Entries.FirstOrDefault(e => 
                    e.FullName.Equals("IG_Data/whdload_db.xml", StringComparison.OrdinalIgnoreCase));

                if (entrada == null)
                {
                    progreso.Report(new EstadoFix("Error", "whdload_db.xml no encontrado en el archivo IG_Data"));
                    return mapa;
                }

                using var stream = entrada.Open();
                using var reader = new StreamReader(stream);

                string? linea;
                while ((linea = await reader.ReadLineAsync()) != null)
                {
                    var match = Regex.Match(linea, @"<slave name=""([^""]+)"".*?<genre>([^<]+)</genre>");
                    if (match.Success)
                    {
                        var slave = match.Groups[1].Value.Trim();
                        var genero = match.Groups[2].Value.Trim();
                        
                        if (!string.IsNullOrWhiteSpace(slave) && !string.IsNullOrWhiteSpace(genero))
                        {
                            mapa[slave] = genero;
                        }
                    }
                }
            }

            progreso.Report(new EstadoFix("Database loaded", $"Loaded {mapa.Count:N0} game entries"));
            return mapa;
        }
        catch (Exception ex)
        {
            progreso.Report(new EstadoFix("Error loading database", ex.Message));
            return mapa;
        }
    }

    private List<Juego> ProcesarJuegos(List<Juego> juegosEntrada, Dictionary<string, string> mapa, IProgress<EstadoFix> progreso)
    {
        var salida = new List<Juego>();
        var actualizados = 0;
        var total = juegosEntrada.Count;

        for (var i = 0; i < juegosEntrada.Count; i++)
        {
            var juego = juegosEntrada[i];
            var key = juego.Slave.ToLowerInvariant();
            
            // Extraer slave del path si es necesario
            var slaveFromPath = ExtractSlaveFromPath(juego.Path);
            var keyFromPath = slaveFromPath.ToLowerInvariant();

            string? generoEncontrado = null;
            bool encontrado = false;

            // Intentar con slave directo
            if (mapa.TryGetValue(key, out var genero))
            {
                generoEncontrado = genero;
                encontrado = true;
            }
            // Intentar con slave del path
            else if (mapa.TryGetValue(keyFromPath, out genero))
            {
                generoEncontrado = genero;
                encontrado = true;
            }

            if (encontrado && !string.IsNullOrWhiteSpace(generoEncontrado))
            {
                var juegoActualizado = new Juego
                {
                    Nombre = juego.Nombre,
                    Genero = generoEncontrado,
                    Path = juego.Path,
                    Slave = juego.Slave,
                    NombreCorto = juego.NombreCorto,
                    Dato1 = juego.Dato1,
                    Dato2 = juego.Dato2,
                    Dato3 = juego.Dato3,
                    Dato4 = juego.Dato4,
                    EsDesconocido = false
                };
                salida.Add(juegoActualizado);
                actualizados++;
            }
            else
            {
                salida.Add(juego);
            }

            if (i % 100 == 0)
            {
                var porcentaje = (int)((i * 100) / total);
                progreso.Report(new EstadoFix($"Processing... {porcentaje}%", $"{i:N0} / {total:N0} games processed"));
            }
        }

        progreso.Report(new EstadoFix("Processing complete", $"Updated {actualizados:N0} of {total:N0} games"));
        return salida;
    }

    private string ExtractSlaveFromPath(string path)
    {
        try
        {
            // Path format: Games:0/1000Miglia/1000Miglia.Slave
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            var parts = path.Split('/');
            if (parts.Length >= 3)
            {
                var slaveWithExt = parts.Last();
                // Remove .Slave extension if present
                if (slaveWithExt.EndsWith(".Slave", StringComparison.OrdinalIgnoreCase))
                {
                    return slaveWithExt;
                }
                else if (slaveWithExt.EndsWith(".slave", StringComparison.OrdinalIgnoreCase))
                {
                    return slaveWithExt;
                }
                return slaveWithExt;
            }
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
}
