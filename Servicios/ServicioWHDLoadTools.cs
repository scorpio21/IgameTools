using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Windows.Forms;
using IgameToolsWinForms.Modelos;
using Microsoft.Extensions.Logging;

namespace IgameToolsWinForms.Servicios
{
    public class ServicioWHDLoadTools
    {
        private readonly ILogger<ServicioWHDLoadTools> _logger;
        private readonly WhdLoadSettings _settings;
        private List<GameData> _gameList;
        private List<int> _filteredList;
        private FilterData _filter;

        public ServicioWHDLoadTools(ILogger<ServicioWHDLoadTools> logger)
        {
            _logger = logger;
            _settings = new WhdLoadSettings();
            _gameList = new List<GameData>();
            _filteredList = new List<int>();
            _filter = new FilterData();
            
            // Establecer directorio base
            _settings.WhdFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "WHDLoad");
        }

        public WhdLoadSettings Settings => _settings;
        public List<GameData> GameList => _gameList;
        public List<int> FilteredList => _filteredList;
        public FilterData Filter => _filter;

        private sealed class SesionConsolaDescarga : IDisposable
        {
            private readonly BlockingCollection<(string Texto, Color Color)> _cola;
            private readonly ManualResetEventSlim _lista;
            private readonly Thread _hilo;
            private FormConsolaDescarga? _form;

            public SesionConsolaDescarga(string titulo)
            {
                _cola = new BlockingCollection<(string, Color)>();
                _lista = new ManualResetEventSlim(false);
                _hilo = new Thread(() => HiloSta(titulo))
                {
                    IsBackground = true
                };
                _hilo.SetApartmentState(ApartmentState.STA);
                _hilo.Start();
                _lista.Wait(TimeSpan.FromSeconds(5));
            }

            private void HiloSta(string titulo)
            {
                _form = new IgameToolsWinForms.FormConsolaDescarga(titulo);
                _form.Shown += (_, _) => _lista.Set();
                _form.FormClosed += (_, _) =>
                {
                    try { _cola.CompleteAdding(); } catch { }
                };

                Task.Run(() =>
                {
                    foreach (var item in _cola.GetConsumingEnumerable())
                    {
                        _form?.EscribirLinea(item.Texto, item.Color);
                    }
                });

                Application.Run(_form);
            }

            public void Escribir(string? texto, Color color)
            {
                if (_cola.IsAddingCompleted)
                    return;

                _cola.Add((texto ?? string.Empty, color));
            }

            public void Cerrar()
            {
                try { _cola.CompleteAdding(); } catch { }

                try
                {
                    if (_form != null && !_form.IsDisposed)
                    {
                        _form.BeginInvoke(new Action(() => _form.Close()));
                    }
                }
                catch
                {
                }
            }

            public void Dispose()
            {
                Cerrar();
            }
        }

        public void DefaultSettings()
        {
            _settings.PrefsName = "default.prefs";
            _settings.DownloadType = 1; // HTTP por defecto
            _settings.FtpFolder = "Retroplay WHDLoad Packs";
            _settings.FtpServer = "ftp2.grandis.nu";
            _settings.FtpUser = "ftp";
            _settings.FtpPass = "amiga";
            _settings.FtpPassive = true;
            _settings.FtpPort = 21;
            _settings.HttpServer = "http://ftp2.grandis.nu/turran/FTP/Retroplay%20WHDLoad%20Packs";
            
            // Establecer carpeta principal por defecto
            _settings.WhdFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");
            
            _settings.FtpGameFolder = "Commodore_Amiga_-_WHDLoad_-_Games";
            _settings.WhdGameFolder = "Games";
            _settings.FtpDemoFolder = "Commodore_Amiga_-_WHDLoad_-_Demos";
            _settings.WhdDemoFolder = "Demos";
            _settings.FtpBetaGameFolder = "Commodore_Amiga_-_WHDLoad_-_Games_-_Beta_&_Unofficial";
            _settings.WhdBetaGameFolder = "Beta-Game";
            _settings.FtpBetaDemoFolder = "Commodore_Amiga_-_WHDLoad_-_Demos_-_Beta_&_Unofficial";
            _settings.WhdBetaDemoFolder = "Beta-Demo";
            _settings.FtpMagsFolder = "Commodore_Amiga_-_WHDLoad_-_Magazines";
            _settings.WhdMagsFolder = "Magazines";
            
            _settings.LangBool = true;
            _settings.SortType = 1;
            _settings.SplitLanguages = 0;
            _settings.A500Mini = false;
        }

        public async Task<bool> ScanHttpAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando escaneo HTTP");
                
                // Limpiar lista actual
                _gameList.Clear();
                _filteredList.Clear();
                
                var datList = new List<string>();
                var xmlList = new List<string>();
                
                // Escanear archivos DAT en el servidor HTTP
                var baseUrl = _settings.HttpServer;
                var datFiles = await ScanHttpDirectoryAsync(baseUrl);
                
                foreach (var datFile in datFiles)
                {
                    if (datFile.EndsWith(".zip"))
                    {
                        datList.Add(datFile);
                        // Extraer archivos XML del ZIP
                        var xmlFiles = await ExtractXmlFromZipAsync(datFile);
                        xmlList.AddRange(xmlFiles);
                    }
                }
                
                // Procesar archivos XML
                foreach (var xmlFile in xmlList)
                {
                    await ProcessXmlFileAsync(xmlFile);
                }
                
                _logger.LogInformation($"Escaneo completado. {_gameList.Count} juegos encontrados.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al escanear HTTP");
                return false;
            }
        }

        public bool ScanFtp()
        {
            try
            {
                _logger.LogInformation("Iniciando escaneo FTP");
                
                // Limpiar lista actual
                _gameList.Clear();
                _filteredList.Clear();
                
                // Simular el procesamiento de archivos XML como en el original
                // En el original se descargan ZIP con XML y se procesan con FillTree()
                ProcessXmlFiles("FTP");
                
                _logger.LogInformation($"Escaneo FTP completado. {_gameList.Count} archivos encontrados");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al escanear FTP");
                return false;
            }
        }

        private List<string> ScanFtpDirectory()
        {
            var files = new List<string>();
            
            try
            {
                // Implementación básica de escaneo FTP
                // En el original se conecta al servidor FTP y busca archivos .dat
                // Por ahora devolvemos archivos de ejemplo
                
                var ftpServer = _settings.FtpServer;
                var ftpUser = _settings.FtpUser;
                var ftpPass = _settings.FtpPass;
                var ftpFolder = _settings.FtpFolder;
                
                // Simular archivos .dat encontrados en FTP
                files.Add("WHDLoad_Amiga1200.dat");
                files.Add("WHDLoad_Amiga600.dat");
                files.Add("WHDLoad_CD32.dat");
                files.Add("WHDLoad_CDTV.dat");
                files.Add("WHDLoad_AGA.dat");
                files.Add("WHDLoad_ECS.dat");
                files.Add("WHDLoad_NTSC.dat");
                files.Add("WHDLoad_PAL.dat");
                
                // Agregar juegos de ejemplo
                var gameNames = new[]
                {
                    "Leisure Suit Larry", "Monkey Island", "Day of the Tentacle",
                    "Indiana Jones", "Sim City", "Civilization",
                    "Worms", "Sensible Soccer", "Speedball 2",
                    "Another World", "Turrican", "Shadow Warrior"
                };
                
                for (int i = 0; i < gameNames.Length; i++)
                {
                    files.Add($"{gameNames[i]}.dat");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error escaneando directorio FTP: {ex.Message}");
            }
            
            return files;
        }

        public bool ScanHttp()
        {
            try
            {
                _logger.LogInformation("Iniciando escaneo HTTP");
                
                // Limpiar lista actual
                _gameList.Clear();
                _filteredList.Clear();
                
                _logger.LogInformation("Llamando a ScanHttpDirectoryAsync...");
                
                // Usar el nuevo método con consola
                var datFiles = Task.Run(async () => await ScanHttpDirectoryAsync("")).GetAwaiter().GetResult();
                
                _logger.LogInformation($"ScanHttpDirectoryAsync devolvió {datFiles.Count} archivos");
                
                // Procesar los archivos .dat encontrados
                ProcessDatFiles(datFiles);
                
                _logger.LogInformation($"Escaneo HTTP completado. {_gameList.Count} archivos encontrados");
                
                // IMPORTANTE: Escanear archivos existentes para marcar Available/Missing
                RescanFiles();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error escaneando HTTP: {ex.Message}");
                return false;
            }
        }

        private List<string> ScanHttpDirectory()
        {
            var files = new List<string>();
            
            try
            {
                // Implementación básica de escaneo HTTP
                // En el original se conecta al servidor HTTP y busca archivos .dat
                // Por ahora devolvemos archivos de ejemplo
                
                var httpServer = _settings.HttpServer;
                
                // Simular archivos .dat encontrados en HTTP
                files.Add("WHDLoad_Amiga1200.dat");
                files.Add("WHDLoad_Amiga600.dat");
                files.Add("WHDLoad_CD32.dat");
                files.Add("WHDLoad_CDTV.dat");
                files.Add("WHDLoad_AGA.dat");
                files.Add("WHDLoad_ECS.dat");
                files.Add("WHDLoad_NTSC.dat");
                files.Add("WHDLoad_PAL.dat");
                
                // Agregar juegos de ejemplo diferentes a los de FTP
                var gameNames = new[]
                {
                    "Prince of Persia", "Out Run", "Lemmings",
                    "The Secret of Monkey Island", "Sam & Max Hit the Road",
                    "Cannon Fodder", "Gods", "Populous",
                    "Dune II", "The Settlers", "Railroad Tycoon",
                    "Theme Park", "Roller Coaster", "Championship Manager"
                };
                
                for (int i = 0; i < gameNames.Length; i++)
                {
                    files.Add($"{gameNames[i]}.dat");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error escaneando directorio HTTP: {ex.Message}");
            }
            
            return files;
        }

        private async Task<List<string>> ScanHttpDirectoryAsync(string url)
        {
            var files = new List<string>();
            
            try
            {
                _logger.LogInformation("Iniciando ScanHttpDirectoryAsync");
                
                // Iniciar proceso de consola separado como en el original
                var consoleProcess = StartConsoleProcess("HTTP Download");
                
                _logger.LogInformation("Iniciando descarga de archivos ZIP del FTP...");
                
                // Crear carpeta Dats si no existe
                var datsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dats");
                _logger.LogInformation($"Ruta de carpeta Dats: {datsPath}");
                
                if (!Directory.Exists(datsPath))
                {
                    _logger.LogInformation("La carpeta Dats no existe, creándola...");
                    Directory.CreateDirectory(datsPath);
                    _logger.LogInformation($"Carpeta Dats creada: {datsPath}");
                }
                else
                {
                    _logger.LogInformation("La carpeta Dats ya existe");
                }
                
                // Lista de archivos ZIP a descargar (como en el original)
                var zipFiles = new[]
                {
                    "Commodore Amiga - WHDLoad - Magazines (2025-07-24).zip",
                    "Commodore Amiga - WHDLoad - Games - Beta & Unofficial (2026-01-12).zip",
                    "Commodore Amiga - WHDLoad - Games (2026-01-24).zip",
                    "Commodore Amiga - WHDLoad - Demos - Beta & Unofficial (2025-07-24).zip",
                    "Commodore Amiga - WHDLoad - Demos (2025-09-20).zip"
                };
                
                var downloadedFiles = new List<string>();
                var fileCount = 0;
                
                // Enviar mensaje de conexión como el original
                SendToConsoleColored(consoleProcess, "Reading ftp2.grandis.nu", ConsoleColor.Green);
                await SendToConsoleAsync(consoleProcess, "");
                
                foreach (var zipFile in zipFiles)
                {
                    fileCount++;
                    try
                    {
                        var localPath = Path.Combine(datsPath, zipFile);
                        
                        // Mensaje como el original: "Downloading : filename.zip"
                        await SendToConsoleAsync(consoleProcess, $"Downloading : {zipFile}");
                        
                        var success = await DownloadAndExtractZipAsync(zipFile, localPath);
                        
                        if (success)
                        {
                            // Mensaje como el original: "Success"
                            await SendToConsoleAsync(consoleProcess, "Success");
                            downloadedFiles.Add(zipFile);
                            _logger.LogInformation($"Archivo descargado y extraído: {zipFile}");
                        }
                        else
                        {
                            // Mensaje de error como el original
                            await SendToConsoleAsync(consoleProcess, "Error - Cannot find HTTP folder!");
                            _logger.LogWarning($"No se pudo descargar: {zipFile}");
                        }
                    }
                    catch (Exception ex)
                    {
                        await SendToConsoleAsync(consoleProcess, "Error - Cannot find HTTP folder!");
                        _logger.LogError(ex, $"Error procesando archivo {zipFile}");
                    }
                }
                
                // Buscar archivos .dat en la carpeta Dats
                await SendToConsoleAsync(consoleProcess, "");
                SendToConsoleColored(consoleProcess, "Processing dat files...", ConsoleColor.Cyan);
                
                files = Directory.GetFiles(datsPath, "*.dat", SearchOption.AllDirectories).ToList();
                _logger.LogInformation($"Se encontraron {files.Count} archivos .dat");
                
                foreach (var datFile in files)
                {
                    _logger.LogInformation($"Archivo .dat encontrado: {datFile}");
                }
                
                // Cerrar consola
                await CloseConsoleProcess(consoleProcess);
                
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error escaneando directorio {url}");
                return new List<string>();
            }
        }

        private Task SendToConsoleAsync(SesionConsolaDescarga? consola, string message)
        {
            try
            {
                consola?.Escribir(message ?? string.Empty, Color.White);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando mensaje a consola: {message}");
                return Task.CompletedTask;
            }
        }

        private SesionConsolaDescarga StartConsoleProcess(string title)
        {
            var sesion = new SesionConsolaDescarga(title);
            sesion.Escribir("Checking for update...", Color.Cyan);
            sesion.Escribir(string.Empty, Color.White);
            return sesion;
        }

        private void CenterConsoleWindow(Process process)
        {
            try
            {
                // Esperar un momento para que la ventana se cree
                System.Threading.Thread.Sleep(500);
                
                // Obtener el handle de la ventana de la consola
                var consoleHandle = process.MainWindowHandle;
                if (consoleHandle != IntPtr.Zero)
                {
                    // Obtener dimensiones de la pantalla principal
                    var mainScreen = System.Windows.Forms.Screen.PrimaryScreen;
                    var screenWidth = mainScreen.WorkingArea.Width;
                    var screenHeight = mainScreen.WorkingArea.Height;
                    
                    // Centrar y dar tamaño a la consola (como en el original)
                    var consoleWidth = (int)(screenWidth / 1.25);
                    var consoleHeight = (int)(screenHeight / 1.25);
                    var consoleX = (screenWidth - consoleWidth) / 2;
                    var consoleY = (screenHeight - consoleHeight) / 2;
                    
                    // Mover y redimensionar la ventana de la consola
                    MoveWindow(consoleHandle, consoleX, consoleY, consoleWidth, consoleHeight, true);
                    
                    // Eliminar el botón de cerrar de la consola (como en el original)
                    RemoveConsoleCloseButton(consoleHandle);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error centrando ventana de consola");
            }
        }

        private Task CloseConsoleProcess(SesionConsolaDescarga? consola)
        {
            try
            {
                consola?.Cerrar();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cerrando proceso de consola");
                return Task.CompletedTask;
            }
        }

        private void OpenDownloadConsole(string title)
        {
            try
            {
                // Asignar una consola al proceso actual
                if (AllocConsole())
                {
                    // Redirigir salida estándar a la consola
                    var stdoutHandle = GetStdHandle(STD_OUTPUT_HANDLE);
                    var stderrHandle = GetStdHandle(STD_ERROR_HANDLE);
                    
                    var consoleOutput = new StreamWriter(Console.OpenStandardOutput());
                    var consoleError = new StreamWriter(Console.OpenStandardError());
                    
                    consoleOutput.AutoFlush = true;
                    consoleError.AutoFlush = true;
                    
                    Console.SetOut(consoleOutput);
                    Console.SetError(consoleError);
                    
                    // Configurar título y propiedades
                    Console.Title = title + " (Press 'Esc' to cancel download.)";
                    Console.Clear();
                    
                    // Centrar y redimensionar la consola
                    var consoleWindow = GetConsoleWindow();
                    if (consoleWindow != IntPtr.Zero)
                    {
                        // Obtener dimensiones de la pantalla principal
                        var mainScreen = System.Windows.Forms.Screen.PrimaryScreen;
                        var screenWidth = mainScreen.WorkingArea.Width;
                        var screenHeight = mainScreen.WorkingArea.Height;
                        
                        // Centrar y dar tamaño a la consola (como en el original)
                        var consoleWidth = (int)(screenWidth / 1.25);
                        var consoleHeight = (int)(screenHeight / 1.25);
                        var consoleX = (screenWidth - consoleWidth) / 2;
                        var consoleY = (screenHeight - consoleHeight) / 2;
                        
                        // Mover y redimensionar la ventana de la consola
                        MoveWindow(consoleWindow, consoleX, consoleY, consoleWidth, consoleHeight, true);
                    }
                    
                    // Eliminar el botón de cerrar de la consola (como en el original)
                    RemoveConsoleCloseButton(consoleWindow);
                    
                    Console.WriteLine("===============================================");
                    Console.WriteLine($"    {title}");
                    Console.WriteLine("===============================================");
                    Console.WriteLine();
                    Console.WriteLine("Starting download process...");
                    Console.WriteLine("Press 'Esc' to cancel download.");
                    Console.WriteLine();
                    
                    // Iniciar hilo para detectar la tecla Escape
                    Task.Run(() => MonitorEscapeKey());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error abriendo consola de descarga");
            }
        }

        private void CloseDownloadConsole()
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("===============================================");
                Console.WriteLine("    Download completed!");
                Console.WriteLine("===============================================");
                Console.WriteLine();
                Console.WriteLine("Please donate to the Turran Server.");
                Console.WriteLine("The link is on the 'About' window.");
                Console.WriteLine();
                
                // Esperar 3 segundos como en el original
                System.Threading.Thread.Sleep(3000);
                
                // Liberar la consola
                FreeConsole();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cerrando consola de descarga");
            }
        }

        private void MonitorEscapeKey()
        {
            try
            {
                while (Console.Title.Contains("Download"))
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Download cancelled by user.");
                            Console.WriteLine("Closing console...");
                            System.Threading.Thread.Sleep(1000);
                            
                            // Cambiar el título para salir del bucle
                            Console.Title = "Cancelled";
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoreando tecla Escape");
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;

        private void RemoveConsoleCloseButton(IntPtr windowHandle)
        {
            try
            {
                if (windowHandle != IntPtr.Zero)
                {
                    var systemMenu = GetSystemMenu(windowHandle, false);
                    // Eliminar el botón de cerrar (posición 6 en el menú del sistema)
                    DeleteMenu(systemMenu, 6, 0x400); // MF_BYPOSITION
                    SendMessage(windowHandle, 0x00A1, (IntPtr)0, (IntPtr)0); // WM_NCPAINT
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando botón de cerrar de la consola");
            }
        }

        private async Task<bool> DownloadAndExtractZipAsync(string zipFileName, string localPath)
        {
            try
            {
                _logger.LogInformation($"Descargando {zipFileName} desde FTP...");
                
                // Descargar el archivo ZIP real desde el FTP
                var ftpUrl = $"ftp://{_settings.FtpServer}/{_settings.FtpFolder}/{zipFileName}";
                
                using var client = new WebClient();
                client.Credentials = new System.Net.NetworkCredential(_settings.FtpUser, _settings.FtpPass);
                
                await client.DownloadFileTaskAsync(new Uri(ftpUrl), localPath);
                _logger.LogInformation($"Archivo descargado: {zipFileName}");
                
                // Extraer el ZIP
                _logger.LogInformation($"Extrayendo {zipFileName}...");
                await ExtractZipFileAsync(localPath);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error descargando y extrayendo {zipFileName}");

                // Importante: no generar datos simulados en producción.
                return false;
            }
        }

        private async Task CreateSimulatedZipFileAsync(string zipPath, string zipFileName)
        {
            // Crear archivo ZIP simulado con archivos .dat dentro
            await Task.Run(() =>
            {
                using var archive = System.IO.Compression.ZipFile.Open(zipPath, System.IO.Compression.ZipArchiveMode.Create);
                
                // Agregar archivos .dat simulados según el tipo de ZIP
                var datFiles = GetDatFilesForZip(zipFileName);
                
                foreach (var datFile in datFiles)
                {
                    var entry = archive.CreateEntry(datFile);
                    using var writer = new StreamWriter(entry.Open());
                    
                    // Escribir contenido .dat simulado
                    WriteSimulatedDatContent(writer, datFile, zipFileName);
                }
            });
        }

        private List<string> GetDatFilesForZip(string zipFileName)
        {
            var files = new List<string>();
            
            if (zipFileName.Contains("Games"))
            {
                if (zipFileName.Contains("Beta"))
                {
                    files.Add("WHDLoad_Games_Beta.dat");
                }
                else
                {
                    files.Add("WHDLoad_Games.dat");
                }
            }
            else if (zipFileName.Contains("Demos"))
            {
                if (zipFileName.Contains("Beta"))
                {
                    files.Add("WHDLoad_Demos_Beta.dat");
                }
                else
                {
                    files.Add("WHDLoad_Demos.dat");
                }
            }
            else if (zipFileName.Contains("Magazines"))
            {
                files.Add("WHDLoad_Magazines.dat");
            }
            
            return files;
        }

        private void WriteSimulatedDatContent(StreamWriter writer, string datFile, string zipFileName)
        {
            // Escribir encabezado simulado de archivo .dat
            writer.WriteLine("; WHDLoad DAT File");
            writer.WriteLine($"; Generated from: {zipFileName}");
            writer.WriteLine("; Date: " + DateTime.Now.ToString("yyyy-MM-dd"));
            writer.WriteLine();
            
            // Generar entradas de juegos simuladas
            var gameCount = Random.Shared.Next(100, 500);
            var gameType = GetGameTypeFromFileName(datFile);
            
            for (int i = 0; i < gameCount; i++)
            {
                var gameName = GenerateGameName(gameType);
                var slave = GenerateSlaveName();
                var genre = GenerateGenre();
                var size = Random.Shared.Next(100000, 5000000);
                
                writer.WriteLine($"{gameName}\t{slave}\t{genre}\t{size}");
            }
        }

        private string GetGameTypeFromFileName(string datFile)
        {
            if (datFile.Contains("Games")) return "Game";
            if (datFile.Contains("Demos")) return "Demo";
            if (datFile.Contains("Magazines")) return "Magazine";
            return "Unknown";
        }

        private string GenerateGameName(string type)
        {
            var prefixes = type == "Game" ? new[] { "Super", "Ultimate", "Amazing", "Epic", "Mega" } :
                           type == "Demo" ? new[] { "Demo", "Preview", "Beta", "Alpha", "Test" } :
                           new[] { "Mag", "Issue", "Volume", "Edition", "Release" };
            
            var suffixes = new[] { "Adventure", "Quest", "Challenge", "Journey", "Saga", "Wars", "Legend", "Mystery" };
            
            var prefix = prefixes[Random.Shared.Next(prefixes.Length)];
            var suffix = suffixes[Random.Shared.Next(suffixes.Length)];
            var number = Random.Shared.Next(1, 999);
            
            return $"{prefix} {suffix} {number}";
        }

        private string GenerateSlaveName()
        {
            var slaves = new[] { "game.slave", "main.slave", "startup.slave", "loader.slave", "exec.slave" };
            return slaves[Random.Shared.Next(slaves.Length)];
        }

        private string GenerateGenre()
        {
            var genres = new[] { "Action", "Adventure", "Platform", "Puzzle", "RPG", "Strategy", "Simulation", "Sports", "Racing", "Shooter" };
            return genres[Random.Shared.Next(genres.Length)];
        }

        private async Task ExtractZipFileAsync(string zipPath)
        {
            await Task.Run(() =>
            {
                var extractPath = Path.GetDirectoryName(zipPath);
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath, true);
                
                // Eliminar el ZIP después de extraer
                File.Delete(zipPath);
            });
        }

        private async Task<List<string>> ExtractXmlFromZipAsync(string zipFile)
        {
            var xmlFiles = new List<string>();
            
            try
            {
                // Implementación simplificada
                xmlFiles.Add("games.xml");
                xmlFiles.Add("demos.xml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error extrayendo XML de {zipFile}");
            }
            
            return xmlFiles;
        }

        private async Task ProcessXmlFileAsync(string xmlFile)
        {
            try
            {
                // Implementación simplificada para procesar XML
                // En el original usa XML parsing
                
                _logger.LogWarning($"ProcessXmlFileAsync está en modo simplificado y no procesa XML real. Archivo: {xmlFile}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error procesando archivo XML {xmlFile}");
            }
        }

        public void FilterList()
        {
            try
            {
                _filteredList.Clear();
                
                // Por ahora, incluir todos los juegos (filtros básicos)
                for (int i = 0; i < _gameList.Count; i++)
                {
                    var game = _gameList[i];
                    
                    // Filtro simple: solo incluir tipos básicos
                    if (ShouldIncludeInFilter(game))
                    {
                        _filteredList.Add(i);
                        game.FileFiltered = true;
                    }
                    else
                    {
                        game.FileFiltered = false;
                    }
                }

                // Aplicar ordenación como en el original
                if (_settings.SortType != 0 && _filteredList.Count > 1)
                {
                    _filteredList = _settings.SortType switch
                    {
                        1 => _filteredList
                            .OrderBy(i => _gameList[i].FileName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                            .ToList(),
                        2 => _filteredList
                            .OrderBy(i => _gameList[i].FileType ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                            .ThenBy(i => _gameList[i].FileName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                            .ToList(),
                        3 => _filteredList
                            .OrderBy(i => _gameList[i].FileType ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                            .ThenBy(i => _gameList[i].FileSubFolder ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                            .ThenBy(i => _gameList[i].FileName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                            .ToList(),
                        _ => _filteredList
                    };
                }
                
                _logger.LogInformation($"Filtro aplicado: {_filteredList.Count} de {_gameList.Count} juegos visibles");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FilterList");
                // En caso de error, mostrar todos los juegos
                _filteredList.Clear();
                for (int i = 0; i < _gameList.Count; i++)
                {
                    _filteredList.Add(i);
                    _gameList[i].FileFiltered = true;
                }
            }
        }

        private bool ShouldIncludeInFilter(GameData game)
        {
            try
            {
                _logger.LogDebug($"Evaluando filtro para juego: {game.FileName}, Tipo: {game.FileType}, Idioma: {game.FileLanguage}");
                
                // Content types - Si el filtro está desactivado, ocultar el tipo correspondiente
                if (!_filter.FGames && game.FileType == "Game") 
                {
                    _logger.LogDebug($"Filtrado: Games desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FDemos && game.FileType == "Demo") 
                {
                    _logger.LogDebug($"Filtrado: Demos desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FMags && game.FileType == "Magazine") 
                {
                    _logger.LogDebug($"Filtrado: Magazines desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.FBetaGame && game.FileBetaGame)
                {
                    _logger.LogDebug($"Filtrado: BetaGame desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FBetaDemo && game.FileBetaDemo)
                {
                    _logger.LogDebug($"Filtrado: BetaDemo desactivado para {game.FileName}");
                    return false;
                }
                
                // System types - Solo filtrar si el filtro está desactivado
                if (!_filter.FAGA && game.FileAga) 
                {
                    _logger.LogDebug($"Filtrado: AGA desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FECS && !game.FileAga) 
                {
                    _logger.LogDebug($"Filtrado: ECS desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FNTSC && game.FileNtsc) 
                {
                    _logger.LogDebug($"Filtrado: NTSC desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FPAL && !game.FileNtsc) 
                {
                    _logger.LogDebug($"Filtrado: PAL desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.FAmiga && game.FileAmiga)
                {
                    _logger.LogDebug($"Filtrado: Amiga desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FArcadia && game.FileArcadia)
                {
                    _logger.LogDebug($"Filtrado: Arcadia desactivado para {game.FileName}");
                    return false;
                }
                
                // Languages - Solo filtrar si el idioma está definido y el filtro está desactivado
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FEnglish && game.FileLanguage == "English") 
                {
                    _logger.LogDebug($"Filtrado: English desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FSpanish && game.FileLanguage == "Spanish") 
                {
                    _logger.LogDebug($"Filtrado: Spanish desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FFrench && game.FileLanguage == "French") 
                {
                    _logger.LogDebug($"Filtrado: French desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FGerman && game.FileLanguage == "German") 
                {
                    _logger.LogDebug($"Filtrado: German desactivado para {game.FileName}");
                    return false;
                }

                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FCroatian && game.FileLanguage == "Croatian")
                {
                    _logger.LogDebug($"Filtrado: Croatian desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FCzech && game.FileLanguage == "Czech")
                {
                    _logger.LogDebug($"Filtrado: Czech desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FDanish && game.FileLanguage == "Danish")
                {
                    _logger.LogDebug($"Filtrado: Danish desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FDutch && game.FileLanguage == "Dutch")
                {
                    _logger.LogDebug($"Filtrado: Dutch desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FFinnish && game.FileLanguage == "Finnish")
                {
                    _logger.LogDebug($"Filtrado: Finnish desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FGreek && game.FileLanguage == "Greek")
                {
                    _logger.LogDebug($"Filtrado: Greek desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FItalian && game.FileLanguage == "Italian")
                {
                    _logger.LogDebug($"Filtrado: Italian desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FMulti && game.FileLanguage == "Multi")
                {
                    _logger.LogDebug($"Filtrado: Multi desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FPolish && game.FileLanguage == "Polish")
                {
                    _logger.LogDebug($"Filtrado: Polish desactivado para {game.FileName}");
                    return false;
                }
                if (!string.IsNullOrEmpty(game.FileLanguage) && !_filter.FSwedish && game.FileLanguage == "Swedish")
                {
                    _logger.LogDebug($"Filtrado: Swedish desactivado para {game.FileName}");
                    return false;
                }
                
                // Hardware types - Solo filtrar si el filtro está desactivado
                if (!_filter.FFiles && game.FileFiles)
                {
                    _logger.LogDebug($"Filtrado: Files desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FImage && game.FileImage)
                {
                    _logger.LogDebug($"Filtrado: Image desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.FCD32 && game.FileCd32) 
                {
                    _logger.LogDebug($"Filtrado: CD32 desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FCDTV && game.FileCdtv) 
                {
                    _logger.LogDebug($"Filtrado: CDTV desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FCDROM && game.FileCdrom) 
                {
                    _logger.LogDebug($"Filtrado: CDROM desactivado para {game.FileName}");
                    return false;
                }
                
                // Memory types - Solo filtrar si el filtro está desactivado
                if (!_filter.FChip && game.FileChip) 
                {
                    _logger.LogDebug($"Filtrado: Chip desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FFast && game.FileFast) 
                {
                    _logger.LogDebug($"Filtrado: Fast desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.F512K && game.File512K)
                {
                    _logger.LogDebug($"Filtrado: 512K desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F512KB && game.File512KB)
                {
                    _logger.LogDebug($"Filtrado: 512KB desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F1MB && game.File1MB)
                {
                    _logger.LogDebug($"Filtrado: 1MB desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F1_5MB && game.File1_5MB)
                {
                    _logger.LogDebug($"Filtrado: 1_5MB desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F1MBCHIP && game.File1MBCHIP)
                {
                    _logger.LogDebug($"Filtrado: 1MBCHIP desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F2MB && game.File2MB)
                {
                    _logger.LogDebug($"Filtrado: 2MB desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F8MB && game.File8MB)
                {
                    _logger.LogDebug($"Filtrado: 8MB desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F12MB && game.File12MB)
                {
                    _logger.LogDebug($"Filtrado: 12MB desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FLowMem && game.FileLowMem)
                {
                    _logger.LogDebug($"Filtrado: LowMem desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FSlowMem && game.FileSlowMem)
                {
                    _logger.LogDebug($"Filtrado: SlowMem desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.FNoIntro && game.FileNoIntro)
                {
                    _logger.LogDebug($"Filtrado: NoIntro desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FMT32 && game.FileMT32)
                {
                    _logger.LogDebug($"Filtrado: MT32 desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FNoVoice && game.FileNoVoice)
                {
                    _logger.LogDebug($"Filtrado: NoVoice desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FNoSpeech && game.FileNoSpeech)
                {
                    _logger.LogDebug($"Filtrado: NoSpeech desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FNoMusic && game.FileNoMusic)
                {
                    _logger.LogDebug($"Filtrado: NoMusic desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FNoMovie && game.FileNoMovie)
                {
                    _logger.LogDebug($"Filtrado: NoMovie desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.F1Disk && game.File1Disk)
                {
                    _logger.LogDebug($"Filtrado: 1Disk desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F2Disk && game.File2Disk)
                {
                    _logger.LogDebug($"Filtrado: 2Disk desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F3Disk && game.File3Disk)
                {
                    _logger.LogDebug($"Filtrado: 3Disk desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.F4Disk && game.File4Disk)
                {
                    _logger.LogDebug($"Filtrado: 4Disk desactivado para {game.FileName}");
                    return false;
                }

                if (!_filter.FHiRes && game.FileHiRes)
                {
                    _logger.LogDebug($"Filtrado: HiRes desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FLoRes && game.FileLoRes)
                {
                    _logger.LogDebug($"Filtrado: LoRes desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FGameDemo && game.FileGameDemo)
                {
                    _logger.LogDebug($"Filtrado: GameDemo desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FPreview && game.FilePreview)
                {
                    _logger.LogDebug($"Filtrado: Preview desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FPreRelease && game.FilePreRelease)
                {
                    _logger.LogDebug($"Filtrado: PreRelease desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FEnhanced && game.FileEnhanced)
                {
                    _logger.LogDebug($"Filtrado: Enhanced desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FCensored && game.FileCensored)
                {
                    _logger.LogDebug($"Filtrado: Censored desactivado para {game.FileName}");
                    return false;
                }
                if (!_filter.FUnCensored && game.FileUnCensored)
                {
                    _logger.LogDebug($"Filtrado: UnCensored desactivado para {game.FileName}");
                    return false;
                }
                
                // Si pasa todos los filtros, incluir el juego
                _logger.LogDebug($"Juego incluido: {game.FileName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ShouldIncludeInFilter para juego: {game.FileName}");
                return true; // Incluir por defecto si hay error
            }
        }

        public void RescanFiles()
        {
            try
            {
                _logger.LogInformation("Iniciando RescanFiles para verificar archivos existentes");
                
                // Marcar todos los juegos como no disponibles inicialmente
                foreach (var game in _gameList)
                {
                    game.FileAvailable = false;
                }
                
                // Escanear todos los archivos en la carpeta WHD
                var whdFolder = _settings.WhdFolder;
                if (!Directory.Exists(whdFolder))
                {
                    _logger.LogWarning($"La carpeta WHD no existe: {whdFolder}");
                    return;
                }
                
                var existingFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                
                // Escanear recursivamente todos los archivos
                ScanFilesRecursive(whdFolder, existingFiles);
                
                _logger.LogInformation($"Se encontraron {existingFiles.Count} archivos en el disco");
                
                // Marcar juegos como disponibles si existen
                var availableCount = 0;
                foreach (var game in _gameList)
                {
                    if (existingFiles.ContainsKey(game.FileName))
                    {
                        game.FileAvailable = true;
                        game.FilePath = existingFiles[game.FileName];
                        availableCount++;
                    }
                }
                
                _logger.LogInformation($"Se marcaron {availableCount} juegos como disponibles de {_gameList.Count} totales");
                _logger.LogInformation($"{_gameList.Count - availableCount} juegos están marcados como Missing");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en RescanFiles");
            }
        }
        
        private void ScanFilesRecursive(string folder, Dictionary<string, string> fileMap)
        {
            try
            {
                // Escaninar archivos LHA y LZX en la carpeta actual
                foreach (var file in Directory.GetFiles(folder, "*.lha"))
                {
                    var fileName = Path.GetFileName(file);
                    fileMap[fileName] = file;
                }
                
                foreach (var file in Directory.GetFiles(folder, "*.lzx"))
                {
                    var fileName = Path.GetFileName(file);
                    fileMap[fileName] = file;
                }
                
                // Escanear subcarpetas recursivamente
                foreach (var subfolder in Directory.GetDirectories(folder))
                {
                    ScanFilesRecursive(subfolder, fileMap);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error escaneando carpeta: {folder}");
            }
        }

        public void UpdateGenre()
        {
            try
            {
                foreach (var game in _gameList)
                {
                    // Asignar género básico basado en el nombre del archivo
                    if (game.FileType == "Game" && !game.FileBetaGame)
                    {
                        // Igual que el original:
                        // - Ignore Languages: el género depende del sistema/chipset.
                        // - Split Languages: si el idioma no es English, el género pasa a ser el idioma.
                        var split = _settings.SplitLanguages == 1;
                        if (split && !string.Equals(game.FileLanguage, "English", StringComparison.OrdinalIgnoreCase))
                        {
                            game.FileGenre = string.IsNullOrWhiteSpace(game.FileLanguage) ? "English" : game.FileLanguage;
                        }
                        else
                        {
                            // Determinar género basado en el nombre del archivo
                            var fileName = (game.FileName ?? string.Empty).ToLowerInvariant();

                            if (fileName.Contains("aga") || fileName.Contains("1200"))
                                game.FileGenre = "AGA";
                            else if (fileName.Contains("ecs") || fileName.Contains("500") || fileName.Contains("600"))
                                game.FileGenre = "ECS-OCS";
                            else if (fileName.Contains("cd32"))
                                game.FileGenre = "CD32";
                            else if (fileName.Contains("cdtv"))
                                game.FileGenre = "CDTV";
                            else if (fileName.Contains("cdrom"))
                                game.FileGenre = "CDROM";
                            else if (fileName.Contains("ntsc"))
                                game.FileGenre = "NTSC";
                            else if (fileName.Contains("arcadia"))
                                game.FileGenre = "Arcadia";
                            else
                                game.FileGenre = "Unknown";
                        }
                    }
                    else if (game.FileType == "Demo" && !game.FileBetaDemo)
                    {
                        game.FileGenre = "Demo";
                    }
                    else if (game.FileBetaGame)
                    {
                        game.FileGenre = "Beta Game";
                    }
                    else if (game.FileBetaDemo)
                    {
                        game.FileGenre = "Beta Demo";
                    }
                    else
                    {
                        game.FileGenre = "Other";
                    }
                }
                
                _logger.LogInformation($"Géneros actualizados para {_gameList.Count} juegos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en UpdateGenre");
            }
        }

        public async Task<bool> DownloadFileAsync(string fileName, string destinationPath)
        {
            try
            {
                _logger.LogInformation($"Creando archivo simulado: {fileName} -> {destinationPath}");
                
                // Crear directorio si no existe
                var directory = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                // Generar archivo simulado con tamaño real basado en el tipo
                var fileSize = GetSimulatedFileSize(fileName);
                
                // Crear archivo con contenido simulado
                await CreateSimulatedFileAsync(destinationPath, fileSize);
                
                _logger.LogInformation($"Archivo creado: {fileName} ({fileSize:N0} bytes)");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creando archivo {fileName}");
                return false;
            }
        }

        private long GetSimulatedFileSize(string fileName)
        {
            // Simular diferentes tamaños según el tipo de archivo
            if (fileName.Contains("Game"))
                return Random.Shared.Next(500000, 3000000); // 0.5MB - 3MB
            else if (fileName.Contains("Demo"))
                return Random.Shared.Next(200000, 1000000); // 0.2MB - 1MB
            else if (fileName.Contains("Magazine"))
                return Random.Shared.Next(1000000, 5000000); // 1MB - 5MB
            else if (fileName.Contains("Beta"))
                return Random.Shared.Next(300000, 2000000); // 0.3MB - 2MB
            else
                return Random.Shared.Next(100000, 1000000); // 0.1MB - 1MB
        }

        private async Task CreateSimulatedFileAsync(string filePath, long size)
        {
            // Crear archivo con datos simulados
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new BinaryWriter(stream);
            
            // Escribir encabezado simulado
            var header = Encoding.ASCII.GetBytes("WHDLoad_Simulated_File_v1.0");
            writer.Write(header);
            
            // Llenar con datos simulados hasta alcanzar el tamaño deseado
            var remainingSize = size - header.Length;
            var bufferSize = 8192;
            var buffer = new byte[bufferSize];
            
            // Crear patrón de datos semi-aleatorio
            var random = new Random();
            for (long i = 0; i < remainingSize; i += bufferSize)
            {
                var bytesToWrite = (int)Math.Min(bufferSize, remainingSize - i);
                random.NextBytes(buffer);
                writer.Write(buffer, 0, bytesToWrite);
                
                // Yield ocasional para no bloquear
                if (i % (1024 * 1024) == 0) // Cada 1MB
                {
                    await Task.Yield();
                }
            }
            
            writer.Flush();
        }

        public void DownloadFilesWithConsole(List<DownData> downloadList)
        {
            try
            {
                _logger.LogInformation("Iniciando descarga con consola detallada y sistema de colores");

                // Iniciar consola como en el original
                var consoleProcess = StartConsoleProcess("WHDLoad Download Tool");

                // Enviar mensaje inicial
                SendToConsoleAsync(consoleProcess, "===============================================");
                SendToConsoleAsync(consoleProcess, "    WHDLoad Download Tool v1.7");
                SendToConsoleAsync(consoleProcess, "===============================================");
                SendToConsoleAsync(consoleProcess, "");
                SendToConsoleColored(consoleProcess, "Checking for update...", ConsoleColor.Cyan);
                SendToConsoleAsync(consoleProcess, "");
                SendToConsoleColored(consoleProcess, "Reading ftp2.grandis.nu", ConsoleColor.Green);
                SendToConsoleAsync(consoleProcess, "");

                int descargados = 0;
                int errores = 0;
                int existentes = 0;

                foreach (var item in downloadList)
                {
                    try
                    {
                        // Verificar si el archivo ya existe
                        var fullPath = Path.Combine(_settings.WhdFolder, item.DownPath);
                        bool fileExists = File.Exists(fullPath);

                        if (fileExists)
                        {
                            // Archivo ya existe - mostrar en verde
                            SendToConsoleColored(consoleProcess, $"Found : {item.DownName}", ConsoleColor.Green);
                            existentes++;
                            continue;
                        }

                        // Mensaje como el original: "Downloading : filename.zip"
                        SendToConsoleAsync(consoleProcess, $"Downloading : {item.DownName}");

                        // Crear directorio si no existe
                        var directory = Path.GetDirectoryName(fullPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Descargar archivo
                        var exito = DownloadFileReal(item, fullPath);

                        if (exito)
                        {
                            // Mensaje como el original: "Success"
                            SendToConsoleAsync(consoleProcess, "Success");
                            descargados++;
                        }
                        else
                        {
                            SendToConsoleColored(consoleProcess, $"Error downloading : {item.DownName}", ConsoleColor.Blue);
                            errores++;
                        }

                        // Pequeña pausa para simular tiempo de descarga
                        System.Threading.Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        // Error en azul con detalles
                        SendToConsoleColored(consoleProcess, $"Error - {ex.Message} ({item.DownName})", ConsoleColor.Blue);
                        _logger.LogError(ex, $"Error descargando {item.DownName}");
                        errores++;
                    }
                }

                _logger.LogInformation($"Descarga completada. Existentes: {existentes}, Descargados: {descargados}, Errores: {errores}");

                // Procesar archivos .dat después de la descarga
                SendToConsoleAsync(consoleProcess, "");
                SendToConsoleColored(consoleProcess, "Processing dat files...", ConsoleColor.Cyan);
                SendToConsoleAsync(consoleProcess, "");
                
                try
                {
                    // Buscar y procesar todos los archivos .at descargados
                    var searchPath = Path.Combine(_settings.WhdFolder, "Dats");
                    if (!Directory.Exists(searchPath))
                    {
                        searchPath = _settings.WhdFolder; // Fallback a la carpeta principal
                    }
                    
                    SendToConsoleAsync(consoleProcess, $"Searching for .dat files in: {searchPath}");
                    
                    var datFiles = Directory.GetFiles(searchPath, "*.dat", SearchOption.AllDirectories);
                    int processedFiles = 0;
                    
                    SendToConsoleAsync(consoleProcess, $"Found {datFiles.Length} .dat files");
                    
                    if (datFiles.Length == 0)
                    {
                        // Buscar en subdirectorios específicos
                        var subdirs = new[] { "Dats", "Download", "Games", "Demos", "Magazines" };
                        foreach (var subdir in subdirs)
                        {
                            var subPath = Path.Combine(_settings.WhdFolder, subdir);
                            if (Directory.Exists(subPath))
                            {
                                var subDatFiles = Directory.GetFiles(subPath, "*.dat", SearchOption.AllDirectories);
                                if (subDatFiles.Length > 0)
                                {
                                    SendToConsoleAsync(consoleProcess, $"Found {subDatFiles.Length} .dat files in {subdir}");
                                    datFiles = datFiles.Concat(subDatFiles).ToArray();
                                }
                            }
                        }
                    }
                    
                    SendToConsoleAsync(consoleProcess, $"Total .dat files to process: {datFiles.Length}");
                    
                    if (datFiles.Length == 0)
                    {
                        SendToConsoleColored(consoleProcess, "No .dat files found!", ConsoleColor.Yellow);
                    }
                    else
                    {
                        // Limpiar listas antes de procesar nuevos archivos
                        _gameList.Clear();
                        _filteredList.Clear();
                        
                        foreach (var datFile in datFiles)
                        {
                            SendToConsoleAsync(consoleProcess, $"Processing: {Path.GetFileName(datFile)}");
                            
                            try
                            {
                                // Procesar el archivo .dat
                                ProcessDatFile(datFile);
                                processedFiles++;
                                SendToConsoleColored(consoleProcess, $"✓ Processed: {Path.GetFileName(datFile)} - {_gameList.Count} games loaded", ConsoleColor.Green);
                            }
                            catch (Exception ex)
                            {
                                SendToConsoleColored(consoleProcess, $"✗ Error processing {Path.GetFileName(datFile)}: {ex.Message}", ConsoleColor.Red);
                                _logger.LogWarning($"Error procesando archivo .dat {datFile}: {ex.Message}");
                            }
                        }
                    }
                    
                    SendToConsoleAsync(consoleProcess, "");
                    SendToConsoleColored(consoleProcess, $"Successfully processed {processedFiles} dat files", ConsoleColor.Green);
                    SendToConsoleAsync(consoleProcess, $"Total games loaded: {_gameList.Count}");
                    
                    if (processedFiles == 0 && _gameList.Count == 0)
                    {
                        SendToConsoleColored(consoleProcess, "Warning: No games loaded. Check if files were downloaded correctly.", ConsoleColor.Yellow);
                    }
                    else
                    {
                        SendToConsoleColored(consoleProcess, "Game list is ready! You can now close this window.", ConsoleColor.Green);
                    }

                    // Resumen final (al terminar de procesar .dat)
                    SendToConsoleAsync(consoleProcess, "");
                    SendToConsoleAsync(consoleProcess, "===============================================");
                    SendToConsoleAsync(consoleProcess, "Download completed!");
                    SendToConsoleAsync(consoleProcess, "===============================================");
                    SendToConsoleAsync(consoleProcess, "");
                    SendToConsoleColored(consoleProcess, $"Found existing files: {existentes}", ConsoleColor.Green);
                    SendToConsoleAsync(consoleProcess, $"Downloaded files: {descargados}");
                    if (errores > 0)
                    {
                        SendToConsoleColored(consoleProcess, $"Errors: {errores}", ConsoleColor.Blue);
                    }
                    SendToConsoleAsync(consoleProcess, "");
                    SendToConsoleAsync(consoleProcess, "Please donate to the Turran Server.");
                    SendToConsoleAsync(consoleProcess, "The link is on the 'About' window.");
                    SendToConsoleAsync(consoleProcess, "");
                }
                catch (Exception ex)
                {
                    SendToConsoleColored(consoleProcess, $"Error processing dat files: {ex.Message}", ConsoleColor.Red);
                    _logger.LogError(ex, "Error procesando archivos .dat");
                }

                // Cerrar consola después de 3 segundos SOLO cuando todo esté completo
                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    CloseConsoleProcess(consoleProcess);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DownloadFilesWithConsole");
                throw;
            }
        }

        private bool DownloadFileReal(DownData item, string destinationFilePath)
        {
            try
            {
                if (_settings.DownloadType == 0)
                {
                    var ftpRelative = $"{item.DownFtpFolder}/{item.DownName}";
                    var hosts = new[] { _settings.FtpServer, "ftp.grandis.nu", "ftp2.grandis.nu" }
                        .Where(h => !string.IsNullOrWhiteSpace(h))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    foreach (var host in hosts)
                    {
                        var ftpUrl = $"ftp://{host}/{EscaparRutaFtp(ftpRelative)}";

                        try
                        {
                            _logger.LogInformation($"Descargando por FTP: {ftpUrl}");
                            using var client = new WebClient();
                            client.Credentials = new NetworkCredential(_settings.FtpUser, _settings.FtpPass);
                            client.DownloadFile(new Uri(ftpUrl), destinationFilePath);
                            return true;
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response is FtpWebResponse ftpResponse)
                            {
                                var statusCode = (int)ftpResponse.StatusCode;
                                var statusDescription = ftpResponse.StatusDescription;
                                _logger.LogWarning(ex, $"Fallo FTP descargando {item.DownName} desde {ftpUrl}: {statusCode} {statusDescription}");

                                if (ftpResponse.StatusCode == FtpStatusCode.NotLoggedIn)
                                {
                                    return false;
                                }

                                if (ftpResponse.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                                {
                                    continue;
                                }
                            }

                            return false;
                        }
                    }

                    return false;
                }
                else
                {
                    var baseUrl = item.DownHttpFolder.TrimEnd('/');
                    var fileUrl = $"{baseUrl}/{Uri.EscapeDataString(Uri.UnescapeDataString(item.DownName))}";

                    var uri = new Uri(fileUrl);
                    var hosts = new[] { uri.Host, "ftp.grandis.nu", "ftp2.grandis.nu" }
                        .Where(h => !string.IsNullOrWhiteSpace(h))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    foreach (var host in hosts)
                    {
                        var builder = new UriBuilder(uri)
                        {
                            Host = host
                        };
                        var urlIntento = builder.Uri.ToString();

                        try
                        {
                            _logger.LogInformation($"Descargando por HTTP: {urlIntento}");
                            using var client = new WebClient();
                            client.DownloadFile(new Uri(urlIntento), destinationFilePath);
                            return true;
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response is HttpWebResponse httpResponse)
                            {
                                var statusCode = (int)httpResponse.StatusCode;
                                var statusDescription = httpResponse.StatusDescription;
                                _logger.LogWarning(ex, $"Fallo HTTP descargando {item.DownName} desde {urlIntento}: {statusCode} {statusDescription}");

                                if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                                {
                                    continue;
                                }
                            }

                            return false;
                        }
                    }

                    return false;
                }
            }
            catch (WebException ex)
            {
                var status = ex.Status.ToString();
                string? detalle = null;
                if (ex.Response is FtpWebResponse ftpResponse)
                {
                    detalle = $"FTP {(int)ftpResponse.StatusCode} {ftpResponse.StatusDescription}";
                }
                else if (ex.Response is HttpWebResponse httpResponse)
                {
                    detalle = $"HTTP {(int)httpResponse.StatusCode} {httpResponse.StatusDescription}";
                }
                _logger.LogError(ex, $"Error descargando archivo real: {item.DownName} (Status={status}, Detalle={detalle})");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error descargando archivo real: {item.DownName}");
                return false;
            }
        }

        private static string EscaparRutaUrl(string rutaConBarras)
        {
            if (string.IsNullOrWhiteSpace(rutaConBarras))
            {
                return string.Empty;
            }

            var partes = rutaConBarras
                .Replace('\\', '/')
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => Uri.EscapeDataString(Uri.UnescapeDataString(p)));

            return string.Join("/", partes);
        }

        private static string EscaparRutaFtp(string rutaConBarras)
        {
            if (string.IsNullOrWhiteSpace(rutaConBarras))
            {
                return string.Empty;
            }

            var partes = rutaConBarras
                .Replace('\\', '/')
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(EscaparSegmentoFtp);

            return string.Join("/", partes);
        }

        private static string EscaparSegmentoFtp(string segmento)
        {
            if (string.IsNullOrEmpty(segmento))
            {
                return string.Empty;
            }

            // FTP en grandis.nu usa literalmente carpetas [A], [0], etc.
            // No debemos convertir los corchetes a %5B/%5D.
            var encoded = Uri.EscapeDataString(Uri.UnescapeDataString(segmento));
            encoded = encoded
                .Replace("%5B", "[")
                .Replace("%5D", "]")
                .Replace("%26", "&");

            return encoded;
        }

        private void SendToConsoleColored(SesionConsolaDescarga? consola, string message, ConsoleColor color)
        {
            try
            {
                if (consola == null)
                    return;

                var colorWin = color switch
                {
                    ConsoleColor.Green => Color.Lime,
                    ConsoleColor.Blue => Color.DeepSkyBlue,
                    ConsoleColor.Red => Color.OrangeRed,
                    ConsoleColor.Yellow => Color.Gold,
                    _ => Color.White
                };

                consola.Escribir(message ?? string.Empty, colorWin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando mensaje coloreado a consola: {message}");
            }
        }

        private bool DownloadFileWithProgress(string fileName, string filePath, Process consoleProcess)
        {
            try
            {
                // Simular progreso de descarga
                var random = new Random();
                var success = random.Next(1, 10) > 2; // 80% éxito

                if (success)
                {
                    // Simular archivo descargado
                    var tempFile = Path.Combine(_settings.WhdFolder, fileName);
                    if (!File.Exists(tempFile))
                    {
                        // Crear archivo temporal para simular descarga
                        Directory.CreateDirectory(Path.GetDirectoryName(tempFile));
                        File.WriteAllText(tempFile, $"Simulated content for {fileName}");
                    }

                    // Copiar al destino final
                    File.Copy(tempFile, filePath, true);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error descargando archivo {fileName}");
                return false;
            }
        }

        public List<DownData> MakeDownloadList()
        {
            var downloadList = new List<DownData>();
            
            foreach (var index in _filteredList)
            {
                if (index >= 0 && index < _gameList.Count)
                {
                    var game = _gameList[index];

                    var carpeta0toZ = !string.IsNullOrWhiteSpace(game.FileSubFolder)
                        ? game.FileSubFolder
                        : GetCarpeta0ToZ(game.FileName);
                    
                    var downData = new DownData
                    {
                        DownName = game.FileName,
                        DownType = game.FileType,
                        DownIndex = index,
                        DownCrc = game.FileCrc,
                        DownGenre = game.FileGenre,
                        DownSize = game.FileSize,
                        DownFtpFolder = $"{_settings.FtpFolder}/{GetFolderForType(game)}/{carpeta0toZ}",
                        DownHttpFolder = CombinarUrl(_settings.HttpServer, $"{GetFolderForType(game)}/{carpeta0toZ}"),
                        DownPath = Path.Combine(GetSubFolderForType(game), game.FileName)
                    };
                    downData.Down0toZ = carpeta0toZ;
                    
                    downloadList.Add(downData);
                }
            }
            
            return downloadList;
        }

        public DownData? CrearDownDataParaIndice(int index)
        {
            if (index < 0 || index >= _gameList.Count)
                return null;

            var game = _gameList[index];

            var carpeta0toZ = !string.IsNullOrWhiteSpace(game.FileSubFolder)
                ? game.FileSubFolder
                : GetCarpeta0ToZ(game.FileName);

            var downData = new DownData
            {
                DownName = game.FileName,
                DownType = game.FileType,
                DownIndex = index,
                DownCrc = game.FileCrc,
                DownGenre = game.FileGenre,
                DownSize = game.FileSize,
                DownFtpFolder = $"{_settings.FtpFolder}/{GetFolderForType(game)}/{carpeta0toZ}",
                DownHttpFolder = CombinarUrl(_settings.HttpServer, $"{GetFolderForType(game)}/{carpeta0toZ}"),
                DownPath = Path.Combine(GetSubFolderForType(game), game.FileName),
                Down0toZ = carpeta0toZ
            };

            return downData;
        }

        private static string CombinarUrl(string baseUrl, string rutaRelativa)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return string.Empty;
            }

            var baseNormalizada = baseUrl.TrimEnd('/');
            var baseSegura = baseNormalizada.Replace(" ", "%20");
            var rutaNormalizada = EscaparRutaUrl(rutaRelativa);

            return $"{baseSegura}/{rutaNormalizada}";
        }

        private static string GetCarpeta0ToZ(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "0";
            }

            var primer = fileName.TrimStart();
            if (primer.Length == 0)
            {
                return "0";
            }

            var c = primer[0];
            if (char.IsDigit(c))
            {
                return "0";
            }

            if (char.IsLetter(c))
            {
                return char.ToUpperInvariant(c).ToString();
            }

            return "0";
        }

        private string GetFolderForType(GameData game)
        {
            if (game.FileBetaGame) return _settings.FtpBetaGameFolder;
            if (game.FileBetaDemo) return _settings.FtpBetaDemoFolder;
            if (game.FileType == "Game") return _settings.FtpGameFolder;
            if (game.FileType == "Demo") return _settings.FtpDemoFolder;
            if (game.FileType == "Magazine") return _settings.FtpMagsFolder;
            
            return _settings.FtpGameFolder;
        }

        private string GetSubFolderForType(GameData game)
        {
            if (game.FileBetaGame) return _settings.WhdBetaGameFolder;
            if (game.FileBetaDemo) return _settings.WhdBetaDemoFolder;
            if (game.FileType == "Game") return _settings.WhdGameFolder;
            if (game.FileType == "Demo") return _settings.WhdDemoFolder;
            if (game.FileType == "Magazine") return _settings.WhdMagsFolder;
            
            return _settings.WhdGameFolder;
        }

        public void SetFilter(bool enabled)
        {
            var properties = typeof(FilterData).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(bool))
                {
                    prop.SetValue(_filter, enabled);
                }
            }
        }

        public bool CheckFilter()
        {
            var properties = typeof(FilterData).GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(bool))
                {
                    var value = (bool)prop.GetValue(_filter);
                    if (!value) return true;
                }
            }
            return false;
        }

        private void ProcessXmlFiles(string source)
        {
            try
            {
                _logger.LogInformation($"Iniciando ProcessXmlFiles para {source}");
                
                // Limpiar listas existentes
                _gameList.Clear();
                _filteredList.Clear();
                
                // Buscar archivos .dat en la carpeta Dats
                var datsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dats");
                
                if (!Directory.Exists(datsPath))
                {
                    _logger.LogWarning($"La carpeta Dats no existe: {datsPath}");
                    return;
                }
                
                var datFiles = Directory.GetFiles(datsPath, "*.dat", SearchOption.AllDirectories);
                _logger.LogInformation($"Se encontraron {datFiles.Length} archivos .dat");
                
                foreach (var datFile in datFiles)
                {
                    try
                    {
                        _logger.LogInformation($"Procesando archivo: {datFile}");
                        ProcessDatFile(datFile);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error procesando archivo {datFile}");
                    }
                }
                
                _logger.LogInformation($"Procesamiento completado. {_gameList.Count} juegos cargados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en ProcessXmlFiles para {source}");
            }
        }

        private void ProcessDatFiles(List<string> datFiles)
        {
            try
            {
                _logger.LogInformation($"Procesando {datFiles.Count} archivos .dat");
                
                foreach (var datFile in datFiles)
                {
                    ProcessDatFile(datFile);
                }
                
                _logger.LogInformation($"Procesamiento completado. {_gameList.Count} juegos cargados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando archivos .dat");
            }
        }

        private string DetermineGenreFromFileName(string fileName)
        {
            try
            {
                // Determinar género basado en el nombre del archivo
                if (fileName.Contains("Demo") || fileName.Contains("demo"))
                    return "Demo";
                else if (fileName.Contains("Game") || fileName.Contains("game"))
                    return "Game";
                else if (fileName.Contains("Magazine") || fileName.Contains("Mag"))
                    return "Magazine";
                else if (fileName.Contains("Tool") || fileName.Contains("Utility"))
                    return "Utility";
                else
                    return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        private void CreateSampleGameData()
        {
            _logger.LogWarning("CreateSampleGameData está deshabilitado para evitar datos simulados.");
        }

        private void ProcessDatFile(string datFilePath)
        {
            try
            {
                // No limpiar listas aquí, se hace antes del bucle principal

                var lines = File.ReadAllLines(datFilePath);
                var fileName = Path.GetFileNameWithoutExtension(datFilePath);
                
                _logger.LogInformation($"Leyendo {lines.Length} líneas de {fileName}");
                
                foreach (var line in lines)
                {
                    // Ignorar líneas vacías y comentarios
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                        continue;
                    
                    // Buscar líneas con <rom name="..." size="..." ...>
                    if (line.Contains("<rom name=") && line.Contains("size="))
                    {
                        try
                        {
                            // Extraer nombre del archivo
                            var nameStart = line.IndexOf("name=\"") + 6;
                            var nameEnd = line.IndexOf("\"", nameStart);
                            var gameName = line.Substring(nameStart, nameEnd - nameStart).Trim();
                            gameName = WebUtility.HtmlDecode(gameName);
                            
                            // Extraer tamaño
                            var sizeStart = line.IndexOf("size=\"") + 6;
                            var sizeEnd = line.IndexOf("\"", sizeStart);
                            var sizeStr = line.Substring(sizeStart, sizeEnd - sizeStart).Trim();
                            
                            if (long.TryParse(sizeStr, out long fileSize))
                            {
                                // Determinar el tipo de juego basado en el nombre del archivo DAT
                                var gameType = DetermineGameType(fileName);
                                var genre = DetermineGenreFromFileName(gameName);
                                var language = DetermineLanguage(gameName);
                                var subFolder = GetCarpeta0ToZ(gameName);
                                
                                _logger.LogDebug($"Procesando: {gameName} - Tipo: {gameType} - Género: {genre}");
                                
                                var game = new GameData
                                {
                                    FileName = gameName,
                                    FileGenre = genre,
                                    FileType = gameType,
                                    FileSize = fileSize,
                                    FileLanguage = language,
                                    FileSubFolder = subFolder,
                                    FileAvailable = false,
                                    FileFiltered = true
                                };
                                
                                _gameList.Add(game);
                                
                                // Agregar a la lista filtrada por defecto
                                _filteredList.Add(_gameList.Count - 1);
                                
                                _logger.LogDebug($"Juego agregado: {gameName} - Tipo: {gameType} - Género: {genre}");
                            }
                            else
                            {
                                _logger.LogWarning($"No se pudo parsear el tamaño: '{sizeStr}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Error procesando línea XML: {ex.Message}");
                        }
                    }
                }
                
                _logger.LogInformation($"Se procesaron {_gameList.Count} juegos desde {fileName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error procesando archivo DAT {datFilePath}");
            }
        }

        private string DetermineGameType(string fileName)
        {
            if (fileName.Contains("Beta", StringComparison.OrdinalIgnoreCase) && fileName.Contains("Games", StringComparison.OrdinalIgnoreCase)) return "Beta-Game";
            if (fileName.Contains("Beta", StringComparison.OrdinalIgnoreCase) && fileName.Contains("Demos", StringComparison.OrdinalIgnoreCase)) return "Beta-Demo";
            if (fileName.Contains("Games", StringComparison.OrdinalIgnoreCase)) return "Game";
            if (fileName.Contains("Demos", StringComparison.OrdinalIgnoreCase)) return "Demo";
            if (fileName.Contains("Magazines", StringComparison.OrdinalIgnoreCase)) return "Magazine";
            return "Unknown";
        }

        private string DetermineSystem(string gameName)
        {
            // Heurística simple para determinar el sistema
            if (gameName.Contains("AGA") || gameName.Contains("HD")) return "AGA";
            if (gameName.Contains("ECS") || gameName.Contains("OCS")) return "ECS-OCS";

            return "Unknown";
        }

        private string DetermineLanguage(string gameName)
        {
            // Igual que el original: por defecto English y detección por códigos "_Xx" antes de "." o "_".
            if (string.IsNullOrWhiteSpace(gameName))
                return "English";

            if (Regex.IsMatch(gameName, "_(DeFrIt)(\\.|_)", RegexOptions.IgnoreCase))
                return "Multi";
            if (Regex.IsMatch(gameName, "_(DeEsFrIt)(\\.|_)", RegexOptions.IgnoreCase))
                return "Multi";

            if (Regex.IsMatch(gameName, "_(Hr)(\\.|_)", RegexOptions.IgnoreCase))
                return "Croatian";
            if (Regex.IsMatch(gameName, "_(Cz)(\\.|_)", RegexOptions.IgnoreCase))
                return "Czech";
            if (Regex.IsMatch(gameName, "_(De)(\\.|_)", RegexOptions.IgnoreCase))
                return "German";
            if (Regex.IsMatch(gameName, "_(Dk)(\\.|_)", RegexOptions.IgnoreCase))
                return "Danish";
            if (Regex.IsMatch(gameName, "_(Es)(\\.|_)", RegexOptions.IgnoreCase))
                return "Spanish";
            if (Regex.IsMatch(gameName, "_(Fi)(\\.|_)", RegexOptions.IgnoreCase))
                return "Finnish";
            if (Regex.IsMatch(gameName, "_(Fr)(\\.|_)", RegexOptions.IgnoreCase))
                return "French";
            if (Regex.IsMatch(gameName, "_(Gr)(\\.|_)", RegexOptions.IgnoreCase))
                return "Greek";
            if (Regex.IsMatch(gameName, "_(It)(\\.|_)", RegexOptions.IgnoreCase))
                return "Italian";
            if (Regex.IsMatch(gameName, "_(Nl)(\\.|_)", RegexOptions.IgnoreCase))
                return "Dutch";
            if (Regex.IsMatch(gameName, "_(Pl)(\\.|_)", RegexOptions.IgnoreCase))
                return "Polish";
            if (Regex.IsMatch(gameName, "_(Se)(\\.|_)", RegexOptions.IgnoreCase))
                return "Swedish";

            return "English";
        }
    }
}
