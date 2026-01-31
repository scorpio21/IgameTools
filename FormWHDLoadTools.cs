using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IgameToolsWinForms.Modelos;
using IgameToolsWinForms.Servicios;
using Microsoft.Extensions.Logging;

namespace IgameToolsWinForms
{
    public partial class FormWHDLoadTools : Form
    {
        private readonly ServicioWHDLoadTools _servicio;
        private readonly ILogger<FormWHDLoadTools> _logger;

        private bool _suspenderEventosFiltros;
        private bool _drawItemConfigurado;
        private readonly object _candadoLogDebug = new();
        private readonly string _rutaLogDebug;
        private readonly string _archivoLogDebug;

        private ToolTip? _toolTip;
        private ContextMenuStrip? _menuLista;

        private string? _rutaUltimaLista;
        private bool _listaEditadaActiva;
        private bool _listaCargadaDesdeArchivo;

        public FormWHDLoadTools(ServicioWHDLoadTools servicio, ILogger<FormWHDLoadTools> logger)
        {
            try
            {
                _logger = logger;
                _servicio = servicio;

                _rutaLogDebug = ObtenerRutaDebugLog();
                _archivoLogDebug = Path.Combine(_rutaLogDebug, "whdload-ui.log");
                Directory.CreateDirectory(_rutaLogDebug);
                EscribirLogDebug("Inicio FormWHDLoadTools");

                InitializeComponent();

                ConfigurarEventos();
                CargarConfiguracionPorDefecto();
                ConfigurarTooltips();
                ConfigurarMenuContextualLista();
                ConfigurarBarraEstado();
                ConfigurarBotonesOriginal();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en constructor de FormWHDLoadTools");
                MessageBox.Show($"Error al inicializar WHDLoad Tools: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                              "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                btnMakeFolder.Enabled = true;
                btnDownload.Enabled = true;
                btnScan.Enabled = true;
                lstMain.Enabled = true;
                EscribirLogDebug("Fin constructor FormWHDLoadTools");
            }
        }

        private void BtnClearLanguage_Click(object sender, EventArgs e)
        {
            try
            {
                _suspenderEventosFiltros = true;
                try
                {
                    chkEnglish.Checked = false;
                    chkSpanish.Checked = false;
                    chkFrench.Checked = false;
                    chkGerman.Checked = false;
                    chkCroatian.Checked = false;
                    chkCzech.Checked = false;
                    chkDanish.Checked = false;
                    chkDutch.Checked = false;
                    chkFinnish.Checked = false;
                    chkGreek.Checked = false;
                    chkItalian.Checked = false;
                    chkMulti.Checked = false;
                    chkPolish.Checked = false;
                    chkSwedish.Checked = false;
                }
                finally
                {
                    _suspenderEventosFiltros = false;
                }

                ActualizarFiltrosDesdeInterfaz();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnClearLanguage_Click");
            }
        }

        private void BtnLang_Click(object sender, EventArgs e)
        {
            BtnClearLanguage_Click(sender, e);
        }

        private void BtnCleanFiles_Click(object sender, EventArgs e)
        {
            try
            {
                _servicio.RescanFiles();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnCleanFiles_Click");
            }
        }

        private void BtnClearData_Click(object sender, EventArgs e)
        {
            BtnClear_Click(sender, e);
        }

        private void EscribirLogDebug(string mensaje)
        {
            try
            {
                lock (_candadoLogDebug)
                {
                    Directory.CreateDirectory(_rutaLogDebug);
                    File.AppendAllText(_archivoLogDebug, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {mensaje}{Environment.NewLine}");
                }
            }
            catch
            {
            }
        }

        private static Task<T> EjecutarEnHiloStaAsync<T>(Func<T> funcion)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            var hilo = new Thread(() =>
            {
                try
                {
                    var resultado = funcion();
                    tcs.TrySetResult(resultado);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            hilo.IsBackground = true;
            hilo.SetApartmentState(ApartmentState.STA);
            hilo.Start();

            return tcs.Task;
        }

        private Task<string?> SeleccionarCarpetaAsync(string titulo, string? carpetaInicial)
        {
            return EjecutarEnHiloStaAsync(() =>
            {
                try
                {
                    using var dialogo = new OpenFileDialog();
                    dialogo.Title = titulo;
                    dialogo.CheckFileExists = false;
                    dialogo.CheckPathExists = true;
                    dialogo.ValidateNames = false;
                    dialogo.FileName = "Seleccionar carpeta";

                    if (!string.IsNullOrWhiteSpace(carpetaInicial) && Directory.Exists(carpetaInicial))
                    {
                        dialogo.InitialDirectory = carpetaInicial;
                    }

                    var resultado = dialogo.ShowDialog();
                    if (resultado != DialogResult.OK)
                    {
                        return (string?)null;
                    }

                    var carpeta = Path.GetDirectoryName(dialogo.FileName);
                    return string.IsNullOrWhiteSpace(carpeta) ? null : carpeta;
                }
                catch (Exception ex)
                {
                    return (string?)$"__ERROR__:{ex.Message}";
                }
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    EscribirLogDebug($"SeleccionarCarpetaAsync ERROR: {t.Exception?.GetBaseException().Message}");
                    return (string?)null;
                }

                var valor = t.Result;
                if (!string.IsNullOrWhiteSpace(valor) && valor.StartsWith("__ERROR__:", StringComparison.Ordinal))
                {
                    EscribirLogDebug($"SeleccionarCarpetaAsync ERROR: {valor}");
                    return (string?)null;
                }

                return valor;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private string ObtenerDirectorioListasPorDefecto()
        {
            try
            {
                var baseDir = _servicio.Settings.WhdFolder;
                if (string.IsNullOrWhiteSpace(baseDir))
                {
                    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                var carpeta = Path.Combine(baseDir, "Lists");
                Directory.CreateDirectory(carpeta);
                return carpeta;
            }
            catch
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        private string ObtenerDirectorioInicialParaListas()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_rutaUltimaLista))
                {
                    var dir = Path.GetDirectoryName(_rutaUltimaLista);
                    if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                    {
                        return dir;
                    }
                }

                return ObtenerDirectorioListasPorDefecto();
            }
            catch
            {
                return ObtenerDirectorioListasPorDefecto();
            }
        }

        private Task<string?> SeleccionarArchivoGuardarAsync(string titulo, string filtro, string? nombrePorDefecto)
        {
            return EjecutarEnHiloStaAsync(() =>
            {
                try
                {
                    using var dialogo = new SaveFileDialog();
                    dialogo.Title = titulo;
                    dialogo.Filter = filtro;
                    dialogo.InitialDirectory = ObtenerDirectorioInicialParaListas();
                    if (!string.IsNullOrWhiteSpace(nombrePorDefecto))
                    {
                        dialogo.FileName = nombrePorDefecto;
                    }

                    var resultado = dialogo.ShowDialog();
                    return resultado == DialogResult.OK ? dialogo.FileName : null;
                }
                catch (Exception ex)
                {
                    return (string?)$"__ERROR__:{ex.Message}";
                }
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    EscribirLogDebug($"SeleccionarArchivoGuardarAsync ERROR: {t.Exception?.GetBaseException().Message}");
                    return (string?)null;
                }

                var valor = t.Result;
                if (!string.IsNullOrWhiteSpace(valor) && valor.StartsWith("__ERROR__:", StringComparison.Ordinal))
                {
                    EscribirLogDebug($"SeleccionarArchivoGuardarAsync ERROR: {valor}");
                    return (string?)null;
                }

                return valor;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task<string?> SeleccionarArchivoAsync(string titulo, string filtro)
        {
            return EjecutarEnHiloStaAsync(() =>
            {
                try
                {
                    using var dialogo = new OpenFileDialog();
                    dialogo.Title = titulo;
                    dialogo.Filter = filtro;
                    dialogo.InitialDirectory = ObtenerDirectorioInicialParaListas();
                    var resultado = dialogo.ShowDialog();
                    return resultado == DialogResult.OK ? dialogo.FileName : null;
                }
                catch (Exception ex)
                {
                    return (string?)$"__ERROR__:{ex.Message}";
                }
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    EscribirLogDebug($"SeleccionarArchivoAsync ERROR: {t.Exception?.GetBaseException().Message}");
                    return (string?)null;
                }

                var valor = t.Result;
                if (!string.IsNullOrWhiteSpace(valor) && valor.StartsWith("__ERROR__:", StringComparison.Ordinal))
                {
                    EscribirLogDebug($"SeleccionarArchivoAsync ERROR: {valor}");
                    return (string?)null;
                }

                return valor;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static string ObtenerRutaDebugLog()
        {
            try
            {
                var actual = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                while (actual != null)
                {
                    var candidato = Path.Combine(actual.FullName, "DebugLog");
                    if (Directory.Exists(candidato))
                    {
                        return candidato;
                    }

                    actual = actual.Parent;
                }
            }
            catch
            {
                // ignorar
            }

            // fallback: junto al ejecutable
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DebugLog");
        }

        private void ConfigurarEventos()
        {
            this.Load += FormWHDLoadTools_Load;
            this.FormClosed += FormWHDLoadTools_FormClosed;
            this.KeyDown += FormWHDLoadTools_KeyDown;

            // Botones principales
            btnScan.Click += BtnScan_Click;
            btnDownload.Click += BtnDownload_Click;
            btnMakeFolder.Click += BtnMakeFolder_Click;

            btnEditlist.Click += BtnEditList_Click;
            btnLoadlist.Click += BtnLoadList_Click;
            btnSavelist.Click += BtnSaveList_Click;
            btnAppendList.Click += BtnAppendList_Click;
            btnClearEdit.Click += BtnClearEdits_Click;

            // Folder Settings (botones dentro del panel)
            btnOpenMain.Click += BtnOpenMain_Click;
            btnSetMain.Click += BtnSetMain_Click;
            btnOpenGames.Click += BtnOpenGames_Click;
            btnOpenDemos.Click += BtnOpenDemos_Click;
            btnOpenBetaGames.Click += BtnOpenBetaGames_Click;
            btnOpenBetaDemos.Click += BtnOpenBetaDemos_Click;
            btnOpenMags.Click += BtnOpenMags_Click;

            // Filtros
            btnLang.Click += BtnLang_Click;
            btnCleaLan.Click += BtnClearFilter_Click;
            btnResetLang.Click += BtnResetFilter_Click;

            // Preferencias
            btnSavePrefs.Click += BtnSavePrefs_Click;
            btnLoadPrefs.Click += BtnLoadPrefs_Click;

            // Data
            btnClearFilter.Click += BtnCleanFiles_Click;
            btnResetFilter.Click += BtnClearData_Click;

            // Lista principal
            lstMain.DoubleClick += LstGames_DoubleClick;
            lstMain.SelectedIndexChanged += LstGames_SelectedIndexChanged;

            // Sorting
            cmbSortType.SelectedIndexChanged += CmbSortType_SelectedIndexChanged;
            cmbLanguageSplit.SelectedIndexChanged += CmbLanguageSplit_SelectedIndexChanged;

            // Cambios en filtros
            chkGames.CheckedChanged += Filter_CheckedChanged;
            chkDemos.CheckedChanged += Filter_CheckedChanged;
            chkBetaGames.CheckedChanged += Filter_CheckedChanged;
            chkBetaDemos.CheckedChanged += Filter_CheckedChanged;
            chkMagazines.CheckedChanged += Filter_CheckedChanged;

            // Filtros de sistema y chipset
            chkAGA.CheckedChanged += Filter_CheckedChanged;
            chkECS.CheckedChanged += Filter_CheckedChanged;
            chkNTSC.CheckedChanged += Filter_CheckedChanged;
            chkPAL.CheckedChanged += Filter_CheckedChanged;
            chkAmiga.CheckedChanged += Filter_CheckedChanged;
            chkArcadia.CheckedChanged += Filter_CheckedChanged;
            chkCD32.CheckedChanged += Filter_CheckedChanged;
            chkCDTV.CheckedChanged += Filter_CheckedChanged;
            chkCDROM.CheckedChanged += Filter_CheckedChanged;

            // Filtros de sonido
            chkMT32.CheckedChanged += Filter_CheckedChanged;
            chkNoVoice.CheckedChanged += Filter_CheckedChanged;
            chkNoSpeech.CheckedChanged += Filter_CheckedChanged;
            chkNoMusic.CheckedChanged += Filter_CheckedChanged;

            // Filtros de idioma
            chkEnglish.CheckedChanged += Filter_CheckedChanged;
            chkSpanish.CheckedChanged += Filter_CheckedChanged;
            chkFrench.CheckedChanged += Filter_CheckedChanged;
            chkGerman.CheckedChanged += Filter_CheckedChanged;
            chkCroatian.CheckedChanged += Filter_CheckedChanged;
            chkCzech.CheckedChanged += Filter_CheckedChanged;
            chkDanish.CheckedChanged += Filter_CheckedChanged;
            chkDutch.CheckedChanged += Filter_CheckedChanged;
            chkFinnish.CheckedChanged += Filter_CheckedChanged;
            chkGreek.CheckedChanged += Filter_CheckedChanged;
            chkItalian.CheckedChanged += Filter_CheckedChanged;
            chkMulti.CheckedChanged += Filter_CheckedChanged;
            chkPolish.CheckedChanged += Filter_CheckedChanged;
            chkSwedish.CheckedChanged += Filter_CheckedChanged;

            // Filtros de memoria/hardware
            chkChip.CheckedChanged += Filter_CheckedChanged;
            chkFast.CheckedChanged += Filter_CheckedChanged;
            chk512k.CheckedChanged += Filter_CheckedChanged;
            chk512KB.CheckedChanged += Filter_CheckedChanged;
            chk1MB.CheckedChanged += Filter_CheckedChanged;
            chk1MBChp.CheckedChanged += Filter_CheckedChanged;
            chk15MB.CheckedChanged += Filter_CheckedChanged;
            chk2MB.CheckedChanged += Filter_CheckedChanged;
            chk8MB.CheckedChanged += Filter_CheckedChanged;
            chk12MB.CheckedChanged += Filter_CheckedChanged;
            chkLowMen.CheckedChanged += Filter_CheckedChanged;
            chkSlowMm.CheckedChanged += Filter_CheckedChanged;

            // Filtros Misc
            chkFiles.CheckedChanged += Filter_CheckedChanged;
            chkImage.CheckedChanged += Filter_CheckedChanged;
            chk1Disk.CheckedChanged += Filter_CheckedChanged;
            chk2Disk.CheckedChanged += Filter_CheckedChanged;
            chk3Disk.CheckedChanged += Filter_CheckedChanged;
            chk4Disk.CheckedChanged += Filter_CheckedChanged;
            chkHiRes.CheckedChanged += Filter_CheckedChanged;
            chkLoRes.CheckedChanged += Filter_CheckedChanged;
            chkNoMovie.CheckedChanged += Filter_CheckedChanged;
            chkNoIntro.CheckedChanged += Filter_CheckedChanged;
            chkPreRelease.CheckedChanged += Filter_CheckedChanged;
            chkPreviewMisc.CheckedChanged += Filter_CheckedChanged;
            chkEnhanced.CheckedChanged += Filter_CheckedChanged;
            chkGameDemo.CheckedChanged += Filter_CheckedChanged;
            chkUnCensored.CheckedChanged += Filter_CheckedChanged;
            chkCensored.CheckedChanged += Filter_CheckedChanged;
        }

        private void ConfigurarTooltips()
        {
            _toolTip = new ToolTip
            {
                ShowAlways = true,
                AutomaticDelay = 200,
                AutoPopDelay = 5000,
                InitialDelay = 200,
                ReshowDelay = 100
            };

            _toolTip.SetToolTip(btnLang, "Clear Language Panel");
            _toolTip.SetToolTip(btnCleaLan, "Clear Filter");
            _toolTip.SetToolTip(btnResetLang, "Reset Filter");

            _toolTip.SetToolTip(btnScan, "Load Dat Files");
            _toolTip.SetToolTip(btnDownload, "Download WHDLoad files");
            _toolTip.SetToolTip(cmbDownloadType, "Server Connection Type Selector");

            _toolTip.SetToolTip(cmbSortType, "Sorting Selector");
            _toolTip.SetToolTip(cmbLanguageSplit, "Languages : Ignore/Split");

            _toolTip.SetToolTip(btnEditlist, "Add/Remove files from download list");
            _toolTip.SetToolTip(btnLoadlist, "Load edited list");
            _toolTip.SetToolTip(btnSavelist, "Save edited list");
            _toolTip.SetToolTip(btnAppendList, "Append saved list to current");
            _toolTip.SetToolTip(btnClearEdit, "Clear edits from download list");

            _toolTip.SetToolTip(btnClearFilter, "Remove old/redundant WHDLoad files");
            _toolTip.SetToolTip(btnResetFilter, "Clear all data and reset filter");
            _toolTip.SetToolTip(btnMakeFolder, "Make new folder from downloaded files");

            _toolTip.SetToolTip(btnSavePrefs, "Save current settings");
            _toolTip.SetToolTip(btnLoadPrefs, "Load saved settings");
            _toolTip.SetToolTip(btnHelp, "Open help window");
            _toolTip.SetToolTip(btnAbout, "Open about window");

            _toolTip.SetToolTip(btnOpenMain, "Open WHDLoad main folder");
            _toolTip.SetToolTip(btnSetMain, "Set WHDLoad main folder");
            _toolTip.SetToolTip(btnOpenGames, "Open Games folder");
            _toolTip.SetToolTip(btnOpenDemos, "Open Demos folder");
            _toolTip.SetToolTip(btnOpenBetaGames, "Open Beta-Game folder");
            _toolTip.SetToolTip(btnOpenBetaDemos, "Open Beta-Demo folder");
            _toolTip.SetToolTip(btnOpenMags, "Open Magazines folder");
        }

        private void ConfigurarBotonesOriginal()
        {
            try
            {
                btnPreview.Visible = false;
                btnClear.Visible = false;
                btnSetPath.Visible = false;
                btnOpenPath.Visible = false;

                btnClearEdit.Enabled = false;
                btnAppendList.Enabled = false;
                _listaEditadaActiva = false;
                _listaCargadaDesdeArchivo = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ConfigurarBotonesOriginal");
            }
        }

        private void BtnLoadList_Click(object sender, EventArgs e)
        {
            _ = CargarListaDesdeArchivoAsync();
        }

        private void BtnAppendList_Click(object sender, EventArgs e)
        {
            _ = AnadirListaDesdeArchivoAsync();
        }

        private void BtnSaveList_Click(object sender, EventArgs e)
        {
            _ = GuardarListaAsync();
        }

        private void BtnClearEdits_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Remove all edits and list data?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                foreach (var game in _servicio.GameList)
                {
                    game.FileIgnore = false;
                    game.FileExtra = false;
                }

                btnClearEdit.Enabled = false;
                btnAppendList.Enabled = false;
                _listaEditadaActiva = false;
                _listaCargadaDesdeArchivo = false;
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnClearEdits_Click");
            }
        }

        private void BtnEditList_Click(object sender, EventArgs e)
        {
            try
            {
                if (_servicio.GameList.Count == 0)
                    return;

                using var form = new Form
                {
                    Text = "Edit List",
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MinimizeBox = false,
                    MaximizeBox = false,
                    ShowInTaskbar = false,
                    ClientSize = new System.Drawing.Size(520, 650)
                };

                var lst = new CheckedListBox
                {
                    CheckOnClick = true,
                    Dock = DockStyle.Fill,
                    HorizontalScrollbar = true
                };

                var indicesOrdenados = Enumerable.Range(0, _servicio.GameList.Count)
                    .OrderBy(i => _servicio.GameList[i].FileName, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                for (int i = 0; i < indicesOrdenados.Count; i++)
                {
                    var indiceJuego = indicesOrdenados[i];
                    var game = _servicio.GameList[indiceJuego];
                    var estadoMarcado = !game.FileIgnore;
                    lst.Items.Add(game.FileName, estadoMarcado);
                }

                var panelBotones = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 70
                };

                var btnClearChecks = new Button
                {
                    Text = "Clear Checks",
                    Width = 120,
                    Height = 35,
                    Left = 25,
                    Top = 15
                };

                var btnUpdate = new Button
                {
                    Text = "Update List",
                    Width = 120,
                    Height = 35,
                    Left = 200,
                    Top = 15,
                    Enabled = false
                };

                var btnCancel = new Button
                {
                    Text = "Cancel",
                    Width = 120,
                    Height = 35,
                    Left = 375,
                    Top = 15
                };

                bool checkAll = true;
                lst.ItemCheck += (_, _) => btnUpdate.Enabled = true;

                btnCancel.Click += (_, _) =>
                {
                    form.DialogResult = DialogResult.Cancel;
                    form.Close();
                };

                btnClearChecks.Click += (_, _) =>
                {
                    btnUpdate.Enabled = true;
                    form.SuspendLayout();
                    checkAll = !checkAll;
                    btnClearChecks.Text = checkAll ? "Clear Checks" : "Check All";
                    for (int i = 0; i < lst.Items.Count; i++)
                    {
                        lst.SetItemChecked(i, checkAll);
                    }
                    form.ResumeLayout();
                };

                btnUpdate.Click += (_, _) =>
                {
                    var result = MessageBox.Show(
                        "Make changes to main list?",
                        "Warning",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Cancel)
                        return;
                    if (result == DialogResult.No)
                    {
                        form.DialogResult = DialogResult.Cancel;
                        form.Close();
                        return;
                    }

                    for (int i = 0; i < indicesOrdenados.Count; i++)
                    {
                        var indiceJuego = indicesOrdenados[i];
                        var game = _servicio.GameList[indiceJuego];

                        game.FileExtra = false;
                        if (lst.GetItemChecked(i))
                        {
                            game.FileIgnore = false;
                            if (game.FileFiltered)
                                game.FileExtra = true;
                        }
                        else
                        {
                            game.FileIgnore = true;
                        }
                    }

                    btnClearEdit.Enabled = _listaCargadaDesdeArchivo;
                    btnAppendList.Enabled = _listaCargadaDesdeArchivo;
                    _listaEditadaActiva = true;
                    _servicio.FilterList();
                    ActualizarListaJuegos();
                    ActualizarTitulo();

                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                panelBotones.Controls.Add(btnClearChecks);
                panelBotones.Controls.Add(btnUpdate);
                panelBotones.Controls.Add(btnCancel);

                form.Controls.Add(lst);
                form.Controls.Add(panelBotones);
                form.AcceptButton = btnUpdate;
                form.CancelButton = btnCancel;

                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnEditList_Click");
            }
        }

        private async Task CargarListaDesdeArchivoAsync()
        {
            try
            {
                if (_servicio.GameList.Count == 0)
                    return;

                if (_servicio.CheckFilter())
                {
                    var result = MessageBox.Show(
                        "This will reset all filters. Continue?",
                        "Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                        return;
                }

                _servicio.SetFilter(true);
                ActualizarFiltrosHaciaInterfaz();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();

                var archivo = await SeleccionarArchivoAsync(
                    "Load List File",
                    "List File (*.lst)|*.lst");

                if (string.IsNullOrWhiteSpace(archivo))
                    return;

                _rutaUltimaLista = archivo;
                _listaEditadaActiva = true;
                _listaCargadaDesdeArchivo = true;

                var lineas = await File.ReadAllLinesAsync(archivo);
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea))
                        continue;
                    if (linea.StartsWith(";", StringComparison.Ordinal))
                        continue;
                    set.Add(linea.Trim());
                }

                foreach (var game in _servicio.GameList)
                {
                    game.FileIgnore = true;
                    game.FileExtra = false;
                }

                foreach (var game in _servicio.GameList)
                {
                    if (set.Contains(game.FileName))
                    {
                        game.FileIgnore = false;
                    }
                }

                btnClearEdit.Enabled = true;
                btnAppendList.Enabled = true;
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CargarListaDesdeArchivoAsync");
            }
        }

        private async Task AnadirListaDesdeArchivoAsync()
        {
            try
            {
                if (_servicio.GameList.Count == 0)
                    return;

                if (!_listaCargadaDesdeArchivo)
                    return;

                var archivo = await SeleccionarArchivoAsync(
                    "Append List File",
                    "List File (*.lst)|*.lst");

                if (string.IsNullOrWhiteSpace(archivo))
                    return;

                _rutaUltimaLista = archivo;

                var lineas = await File.ReadAllLinesAsync(archivo);
                var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea))
                        continue;
                    if (linea.StartsWith(";", StringComparison.Ordinal))
                        continue;
                    set.Add(linea.Trim());
                }

                foreach (var game in _servicio.GameList)
                {
                    if (set.Contains(game.FileName))
                    {
                        game.FileIgnore = false;
                    }
                }

                btnClearEdit.Enabled = true;
                btnAppendList.Enabled = true;
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en AnadirListaDesdeArchivoAsync");
            }
        }

        private async Task GuardarListaAsync()
        {
            try
            {
                if (_servicio.GameList.Count == 0)
                    return;

                var archivo = await SeleccionarArchivoGuardarAsync(
                    "Save List File",
                    "List File (*.lst)|*.lst",
                    "list.lst");

                if (string.IsNullOrWhiteSpace(archivo))
                    return;

                if (!archivo.EndsWith(".lst", StringComparison.OrdinalIgnoreCase))
                    archivo += ".lst";

                _rutaUltimaLista = archivo;

                var salida = new List<string>();
                foreach (var index in _servicio.FilteredList)
                {
                    if (index < 0 || index >= _servicio.GameList.Count)
                        continue;

                    var game = _servicio.GameList[index];
                    if (game.FileIgnore)
                        continue;

                    salida.Add(game.FileName);
                }

                await File.WriteAllLinesAsync(archivo, salida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GuardarListaAsync");
                MessageBox.Show($"Error saving list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarMenuContextualLista()
        {
            _menuLista = new ContextMenuStrip();
            var itemDescargar = new ToolStripMenuItem("Download this file");
            itemDescargar.Click += async (_, _) => await DescargarSeleccionadoAsync();
            _menuLista.Items.Add(itemDescargar);

            lstMain.ContextMenuStrip = _menuLista;
            lstMain.MouseDown += LstMain_MouseDown;
        }

        private void ConfigurarBarraEstado()
        {
            try
            {
                if (toolStripStatusLabel1 != null)
                    toolStripStatusLabel1.Visible = false;
                if (toolStripStatusLabel2 != null)
                    toolStripStatusLabel2.Visible = false;

                ActualizarBarraEstado(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ConfigurarBarraEstado");
            }
        }

        private void ActualizarBarraEstado(GameData? game)
        {
            try
            {
                if (game == null)
                {
                    lblSystem.Text = "System: ?";
                    lblChipset.Text = "Chipset: ?";
                    lblTVSystem.Text = "TV System: ?";
                    lblLanguage.Text = "Language: ?";
                    lblType.Text = "Type: ?";
                    lblStatusInfo.Text = "Status: ?";
                    lblSize.Text = "Size: ?";
                    lblVersion.Text = "Version: ?";
                    return;
                }

                var sistema = "?";
                if (game.FileAmiga) sistema = "Amiga";
                if (game.FileCd32) sistema = "CD32";
                if (game.FileCdtv) sistema = "CDTV";
                if (game.FileCdrom) sistema = "CDROM";
                if (game.FileArcadia) sistema = "Arcadia";

                var chipset = game.FileAga ? "AGA" : "ECS/OCS";
                var tv = game.FileNtsc ? "NTSC" : "PAL";

                var idioma = string.IsNullOrWhiteSpace(game.FileLanguage) ? "?" : game.FileLanguage;

                var tipo = game.FileBetaGame || game.FileBetaDemo
                    ? "Beta"
                    : (string.IsNullOrWhiteSpace(game.FileType) ? "?" : game.FileType);

                var status = game.FileAvailable ? "Available" : "Missing";
                var sizeKb = game.FileSize > 0 ? (game.FileSize / 1024) : 0;
                var version = string.IsNullOrWhiteSpace(game.FileVersion) ? "?" : game.FileVersion;

                lblSystem.Text = $"System: {sistema}";
                lblChipset.Text = $"Chipset: {chipset}";
                lblTVSystem.Text = $"TV System: {tv}";
                lblLanguage.Text = $"Language: {idioma}";
                lblType.Text = $"Type: {tipo}";
                lblStatusInfo.Text = $"Status: {status}";
                lblSize.Text = $"Size: {sizeKb} KB";
                lblVersion.Text = $"Version: {version}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarBarraEstado");
            }
        }

        private void LstMain_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var index = lstMain.IndexFromPoint(e.Location);
            if (index >= 0 && index < lstMain.Items.Count)
            {
                lstMain.SelectedIndex = index;
            }
        }

        private async Task DescargarSeleccionadoAsync()
        {
            try
            {
                if (lstMain.SelectedIndex < 0)
                    return;

                ActualizarConfiguracionDesdeInterfaz();

                var filteredIndex = lstMain.SelectedIndex;
                if (filteredIndex < 0 || filteredIndex >= _servicio.FilteredList.Count)
                    return;

                var gameIndex = _servicio.FilteredList[filteredIndex];
                var downData = _servicio.CrearDownDataParaIndice(gameIndex);
                if (downData == null)
                    return;

                var seleccion = new List<DownData> { downData };

                btnDownload.Enabled = false;
                btnScan.Enabled = false;
                lstMain.Enabled = false;

                try
                {
                    await Task.Run(() => _servicio.DownloadFilesWithConsole(seleccion));
                }
                finally
                {
                    btnDownload.Enabled = true;
                    btnScan.Enabled = true;
                    lstMain.Enabled = true;
                }

                _servicio.RescanFiles();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando fichero desde menú contextual");
                MessageBox.Show($"Error al descargar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarConfiguracionPorDefecto()
        {
            try
            {
                _servicio.DefaultSettings();
                _logger.LogInformation($"Configuración por defecto cargada:");
                _logger.LogInformation($"  WHD_Folder: '{_servicio.Settings.WhdFolder}'");
                _logger.LogInformation($"  WHD_Game_Folder: '{_servicio.Settings.WhdGameFolder}'");
                _logger.LogInformation($"  WHD_Demo_Folder: '{_servicio.Settings.WhdDemoFolder}'");
                _logger.LogInformation($"  FTP_Folder: '{_servicio.Settings.FtpFolder}'");

                ActualizarInterfazDesdeConfiguracion();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CargarConfiguracionPorDefecto");
            }
        }

        private void ActualizarInterfazDesdeConfiguracion()
        {
            try
            {
                var settings = _servicio.Settings;

                _logger.LogInformation($"Actualizando interfaz desde configuración:");
                _logger.LogInformation($"  txtWHDMain.Text = '{settings.WhdFolder}'");
                _logger.LogInformation($"  txtWHDGames.Text = '{settings.WhdGameFolder}'");
                _logger.LogInformation($"  txtWHDDemos.Text = '{settings.WhdDemoFolder}'");
                _logger.LogInformation($"  txtFtpFolder.Text = '{settings.FtpFolder}'");

                // Carpeta principal local (ruta completa)
                txtWHDMain.Text = settings.WhdFolder ?? "";
                txtWHDMain.Refresh(); // Forzar actualización visual

                // Mostrar solo nombres de subcarpetas locales
                txtWHDGames.Text = settings.WhdGameFolder ?? "";
                txtWHDDemos.Text = settings.WhdDemoFolder ?? "";
                txtWHDBetaGames.Text = settings.WhdBetaGameFolder ?? "";
                txtWHDBetaDemos.Text = settings.WhdBetaDemoFolder ?? "";
                txtWHDMags.Text = settings.WhdMagsFolder ?? "";

                // Forzar actualización de todos los TextBox locales
                txtWHDGames.Refresh();
                txtWHDDemos.Refresh();
                txtWHDBetaGames.Refresh();
                txtWHDBetaDemos.Refresh();
                txtWHDMags.Refresh();

                // Mostrar rutas FTP en la sección Server Settings
                txtGamePath.Text = settings.FtpGameFolder ?? "";
                txtDemoPath.Text = settings.FtpDemoFolder ?? "";
                txtBetaGamePath.Text = settings.FtpBetaGameFolder ?? "";
                txtBetaDemoPath.Text = settings.FtpBetaDemoFolder ?? "";
                txtMagsPath.Text = settings.FtpMagsFolder ?? "";

                // Forzar actualización de los TextBox FTP
                txtGamePath.Refresh();
                txtDemoPath.Refresh();
                txtBetaGamePath.Refresh();
                txtBetaDemoPath.Refresh();
                txtMagsPath.Refresh();

                _logger.LogInformation($"Rutas actualizadas:");
                _logger.LogInformation($"  txtWHDMain.Text = '{txtWHDMain.Text}'");
                _logger.LogInformation($"  txtWHDGames.Text = '{txtWHDGames.Text}'");
                _logger.LogInformation($"  txtWHDDemos.Text = '{txtWHDDemos.Text}'");
                _logger.LogInformation($"  txtWHDBetaGames.Text = '{txtWHDBetaGames.Text}'");
                _logger.LogInformation($"  txtWHDBetaDemos.Text = '{txtWHDBetaDemos.Text}'");
                _logger.LogInformation($"  txtWHDMags.Text = '{txtWHDMags.Text}'");
                _logger.LogInformation($"  txtGamePath.Text = '{txtGamePath.Text}'");
                _logger.LogInformation($"  txtDemoPath.Text = '{txtDemoPath.Text}'");
                _logger.LogInformation($"  txtBetaGamePath.Text = '{txtBetaGamePath.Text}'");
                _logger.LogInformation($"  txtBetaDemoPath.Text = '{txtBetaDemoPath.Text}'");
                _logger.LogInformation($"  txtMagsPath.Text = '{txtMagsPath.Text}'");

                txtFtpServer.Text = settings.FtpServer;
                txtFtpUser.Text = settings.FtpUser;
                txtFtpPass.Text = settings.FtpPass;
                txtFtpPort.Text = settings.FtpPort.ToString();
                txtFtpFolder.Text = settings.FtpFolder;
                txtHttpServer.Text = settings.HttpServer;

                if (settings.DownloadType < 0 || settings.DownloadType > 1)
                {
                    settings.DownloadType = 1;
                }

                if (cmbDownloadType.Items.Count >= 2)
                {
                    cmbDownloadType.SelectedIndex = settings.DownloadType;
                }

                if (settings.SortType < 0 || settings.SortType > 3)
                {
                    settings.SortType = 0;
                }

                if (cmbSortType.Items.Count >= 4)
                {
                    cmbSortType.SelectedIndex = settings.SortType;
                }

                if (settings.SplitLanguages < 0 || settings.SplitLanguages > 1)
                {
                    settings.SplitLanguages = 0;
                }

                if (cmbLanguageSplit.Items.Count >= 2)
                {
                    cmbLanguageSplit.SelectedIndex = settings.SplitLanguages;
                }

                _logger.LogInformation("Interfaz actualizada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarInterfazDesdeConfiguracion");
            }
        }

        private void ActualizarConfiguracionDesdeInterfaz()
        {
            try
            {
                var settings = _servicio.Settings;

                settings.WhdFolder = txtWHDMain.Text;
                settings.WhdGameFolder = txtWHDGames.Text;
                settings.WhdDemoFolder = txtWHDDemos.Text;
                settings.WhdBetaGameFolder = txtWHDBetaGames.Text;
                settings.WhdBetaDemoFolder = txtWHDBetaDemos.Text;
                settings.WhdMagsFolder = txtWHDMags.Text;

                // 0 = FTP, 1 = HTTP
                settings.DownloadType = cmbDownloadType.SelectedIndex;

                // 0=No sorting, 1=Alphabetical, 2=Category, 3=Category (0-Z)
                settings.SortType = cmbSortType.SelectedIndex;

                // 0=Ignore Languages, 1=Split Languages
                settings.SplitLanguages = cmbLanguageSplit.SelectedIndex;

                settings.FtpServer = txtFtpServer.Text;
                settings.FtpUser = txtFtpUser.Text;
                settings.FtpPass = txtFtpPass.Text;
                if (int.TryParse(txtFtpPort.Text, out int port))
                    settings.FtpPort = port;
                settings.FtpFolder = txtFtpFolder.Text;
                settings.HttpServer = txtHttpServer.Text;

                _logger.LogInformation("Configuración actualizada desde interfaz");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar configuración desde interfaz");
            }
        }

        private void CmbSortType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                _servicio.UpdateGenre();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CmbSortType_SelectedIndexChanged");
            }
        }

        private void CmbLanguageSplit_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                _servicio.UpdateGenre();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CmbLanguageSplit_SelectedIndexChanged");
            }
        }

        private async void FormWHDLoadTools_Load(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("Iniciando FormWHDLoadTools_Load");

                // Cargar configuración por defecto
                CargarConfiguracionPorDefecto();

                // Forzar actualización de las rutas
                ActualizarInterfazDesdeConfiguracion();

                // Configurar filtros
                ActualizarFiltrosHaciaInterfaz();

                // Crear directorios si no existen
                CrearDirectoriosSiNoExisten();

                _logger.LogInformation("FormWHDLoadTools cargado completamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FormWHDLoadTools_Load");
                MessageBox.Show($"Error al cargar WHDLoad Tools: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                              "Error al Cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormWHDLoadTools_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                _logger.LogInformation("FormWHDLoadTools cerrado completamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FormWHDLoadTools_FormClosed");
            }
        }

        private void FormWHDLoadTools_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                // Refrescar lista
                RefrescarLista();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private async void BtnScan_Click(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar controles durante el escaneo para evitar cuelgue
                btnScan.Enabled = false;
                btnDownload.Enabled = false;
                lstMain.Enabled = false;
                btnScan.Text = "Escaneando...";

                _logger.LogInformation("Iniciando escaneo de archivos WHDLoad");

                // Actualizar configuración desde la interfaz antes de escanear
                ActualizarConfiguracionDesdeInterfaz();

                // Leer el tipo de descarga ANTES de Task.Run (en el hilo principal)
                var downloadType = cmbDownloadType.SelectedIndex;

                // Ejecutar escaneo en segundo plano para no bloquear UI
                var resultado = await Task.Run(() =>
                {
                    // Determinar tipo de descarga usando la variable capturada
                    if (downloadType == 0) // FTP
                    {
                        return _servicio.ScanFtp();
                    }
                    else // HTTP
                    {
                        return _servicio.ScanHttp();
                    }
                });

                if (resultado)
                {
                    // Actualizar UI en el hilo principal
                    _servicio.UpdateGenre();
                    _servicio.FilterList();
                    ActualizarListaJuegos();
                    ActualizarTitulo();

                    /*   lblStatus.Text = string.Empty;
                       MessageBox.Show($"Escaneo completado. {_servicio.GameList.Count} juegos encontrados.",
                                     "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
                else
                {
                    /* lblStatus.Text = "Error en el escaneo";
                     MessageBox.Show("Error durante el escaneo. Revise el log para más detalles.",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                }
            }
            catch (Exception ex)
            {
                /*   _logger.LogError(ex, "Error en BtnScan_Click");
                   lblStatus.Text = "Error";
                   MessageBox.Show($"Error durante el escaneo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
            }
            finally
            {
                // Rehabilitar controles
                btnScan.Enabled = true;
                btnDownload.Enabled = true;
                lstMain.Enabled = true;
                btnScan.Text = "Load Data";
            }
        }

        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("Botón Download presionado");

                // Asegurar que la configuración (incluido FTP/HTTP) está sincronizada antes de construir rutas/URLs
                ActualizarConfiguracionDesdeInterfaz();

                // Obtener lista de archivos para descargar
                var downloadList = _servicio.MakeDownloadList();

                if (downloadList.Count == 0)
                {
                    MessageBox.Show("No hay archivos para descargar según los filtros actuales.",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Mostrar ventana de confirmación de descarga
                using (var downloadWindow = new FormDownloadWindow(downloadList))
                {
                    var result = downloadWindow.ShowDialog(this);

                    if (result != DialogResult.OK || !downloadWindow.Confirmed)
                    {
                        _logger.LogInformation("Descarga cancelada por el usuario");
                        return;
                    }

                    _logger.LogInformation($"Descarga confirmada. 255 Files: {downloadWindow.Files255Selected}, Expand Tree: {downloadWindow.ExpandTreeSelected}, Save Txt: {downloadWindow.SaveTxtClicked}");

                    var seleccionados = downloadWindow.GetSelectedDownloadList();
                    if (seleccionados.Count == 0)
                    {
                        MessageBox.Show("No hay archivos seleccionados para descargar.",
                                      "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Iniciar descarga automáticamente después de confirmar
                    _logger.LogInformation($"Iniciando descarga de {seleccionados.Count} archivos");

                    // Deshabilitar controles durante la descarga
                    btnDownload.Enabled = false;
                    btnScan.Enabled = false;
                    lstMain.Enabled = false;
                    btnDownload.Text = "Descargando...";

                    try
                    {
                        // Iniciar descarga con consola como en el original
                        await Task.Run(() => _servicio.DownloadFilesWithConsole(seleccionados));

                        // Actualizar lista después de la descarga
                        _servicio.RescanFiles();
                        ActualizarListaJuegos();
                        ActualizarTitulo();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error durante la descarga");
                        MessageBox.Show($"Error durante la descarga: {ex.Message}",
                                      "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        // Rehabilitar controles
                        btnDownload.Enabled = true;
                        btnScan.Enabled = true;
                        lstMain.Enabled = true;
                        btnDownload.Text = "Download";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnDownload_Click");
                MessageBox.Show($"Error al iniciar descarga: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                var downloadList = _servicio.MakeDownloadList();

                if (downloadList.Count == 0)
                {
                    MessageBox.Show("No hay archivos para previsualizar.",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using var formPreview = new FormDownloadPreview(downloadList);
                formPreview.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnPreview_Click");
                MessageBox.Show($"Error al mostrar vista previa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                var resultado = MessageBox.Show(
                    "¿Está seguro de que desea limpiar todos los datos?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    _servicio.GameList.Clear();
                    _servicio.FilteredList.Clear();
                    lstMain.Items.Clear();
                    ActualizarTitulo();

                    MessageBox.Show("Datos limpiados correctamente.",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnClear_Click");
                MessageBox.Show($"Error al limpiar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnMakeFolder_Click(object sender, EventArgs e)
        {
            try
            {
                EscribirLogDebug("BtnMakeFolder_Click INICIO");
                EscribirLogDebug("BtnMakeFolder_Click mostrando dialogo carpeta salida");
                var carpetaSalida = await SeleccionarCarpetaAsync("Seleccione la carpeta de salida", null);
                EscribirLogDebug($"BtnMakeFolder_Click carpeta salida seleccionada='{carpetaSalida ?? ""}'");

                if (!string.IsNullOrWhiteSpace(carpetaSalida))
                {
                    var downloadList = _servicio.MakeDownloadList();

                    if (downloadList.Count == 0)
                    {
                        MessageBox.Show("No hay archivos para organizar.",
                                      "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    EstablecerUiOcupada(true);
                    try
                    {
                        EscribirLogDebug($"BtnMakeFolder_Click carpeta destino='{carpetaSalida}' items={downloadList.Count}");
                        int copiados = 0;

                        await Task.Run(() =>
                        {
                            foreach (var item in downloadList)
                            {
                                try
                                {
                                    var sourcePath = Path.Combine(_servicio.Settings.WhdFolder, item.DownPath);
                                    var destPath = Path.Combine(carpetaSalida, item.DownPath);

                                    if (File.Exists(sourcePath))
                                    {
                                        var destDir = Path.GetDirectoryName(destPath);
                                        if (!string.IsNullOrWhiteSpace(destDir) && !Directory.Exists(destDir))
                                        {
                                            Directory.CreateDirectory(destDir);
                                        }

                                        File.Copy(sourcePath, destPath, true);
                                        copiados++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, $"Error copiando {item.DownName}");
                                }
                            }
                        });

                        MessageBox.Show(
                            $"Carpeta creada correctamente.\nArchivos copiados: {copiados}",
                            "Resultado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        System.Diagnostics.Process.Start("explorer.exe", carpetaSalida);
                        EscribirLogDebug($"BtnMakeFolder_Click FIN copiados={copiados}");
                    }
                    finally
                    {
                        EstablecerUiOcupada(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnMakeFolder_Click");
                EscribirLogDebug($"BtnMakeFolder_Click ERROR: {ex.Message}");
                MessageBox.Show($"Error al crear carpeta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EscribirLogDebug("BtnMakeFolder_Click SALIDA");
            }
        }

        private async void BtnSetPath_Click(object sender, EventArgs e)
        {
            try
            {
                EscribirLogDebug("BtnSetPath_Click INICIO");
                EscribirLogDebug("BtnSetPath_Click mostrando dialogo carpeta");
                var carpeta = await SeleccionarCarpetaAsync("Seleccione la carpeta WHDLoad", _servicio.Settings.WhdFolder);
                EscribirLogDebug($"BtnSetPath_Click carpeta seleccionada='{carpeta ?? ""}'");

                if (!string.IsNullOrWhiteSpace(carpeta))
                {
                    _servicio.Settings.WhdFolder = carpeta;
                    txtWHDMain.Text = carpeta;

                    EstablecerUiOcupada(true);
                    try
                    {
                        EscribirLogDebug($"BtnSetPath_Click creando directorios en='{_servicio.Settings.WhdFolder}'");
                        await Task.Run(() => CrearDirectoriosSiNoExisten());
                        EscribirLogDebug("BtnSetPath_Click directorios OK");
                    }
                    finally
                    {
                        EstablecerUiOcupada(false);
                    }

                    MessageBox.Show("Ruta WHDLoad actualizada correctamente.",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnSetPath_Click");
                EscribirLogDebug($"BtnSetPath_Click ERROR: {ex.Message}");
                MessageBox.Show($"Error al establecer ruta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EscribirLogDebug("BtnSetPath_Click SALIDA");
            }
        }

        private void BtnOpenPath_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(_servicio.Settings.WhdFolder))
                {
                    System.Diagnostics.Process.Start("explorer.exe", _servicio.Settings.WhdFolder);
                }
                else
                {
                    MessageBox.Show("La carpeta WHDLoad no existe.",
                                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenPath_Click");
                MessageBox.Show($"Error al abrir carpeta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Filter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_suspenderEventosFiltros)
                {
                    return;
                }

                ActualizarFiltrosDesdeInterfaz();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Filter_CheckedChanged");
            }
        }

        private void BtnClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                _servicio.SetFilter(false);
                ActualizarFiltrosHaciaInterfaz();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnClearFilter_Click");
            }
        }

        private void BtnResetFilter_Click(object sender, EventArgs e)
        {
            try
            {
                _servicio.SetFilter(true);
                ActualizarFiltrosHaciaInterfaz();
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnResetFilter_Click");
            }
        }

        private async void BtnSavePrefs_Click(object sender, EventArgs e)
        {
            try
            {
                await GuardarPreferenciasAsync();
                MessageBox.Show("Preferencias guardadas correctamente.",
                              "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnSavePrefs_Click");
                MessageBox.Show($"Error al guardar preferencias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnLoadPrefs_Click(object sender, EventArgs e)
        {
            try
            {
                EscribirLogDebug("BtnLoadPrefs_Click INICIO");
                var archivo = await SeleccionarArchivoAsync(
                    "Cargar Preferencias",
                    "Preferencias (*.prefs)|*.prefs|Todos los archivos (*.*)|*.*");

                if (!string.IsNullOrWhiteSpace(archivo))
                {
                    EstablecerUiOcupada(true);
                    try
                    {
                        EscribirLogDebug($"BtnLoadPrefs_Click archivo='{archivo}'");
                        await CargarPreferenciasAsync(archivo);
                        EscribirLogDebug("BtnLoadPrefs_Click parse prefs OK");
                        ActualizarInterfazDesdeConfiguracion();
                        ActualizarFiltrosHaciaInterfaz();
                        EscribirLogDebug("BtnLoadPrefs_Click creando directorios...");
                        await Task.Run(() => CrearDirectoriosSiNoExisten());
                        EscribirLogDebug("BtnLoadPrefs_Click refrescando lista...");
                        await RefrescarListaAsync();
                        EscribirLogDebug("BtnLoadPrefs_Click FIN OK");
                    }
                    finally
                    {
                        EstablecerUiOcupada(false);
                    }

                    MessageBox.Show("Preferencias cargadas correctamente.",
                                  "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnLoadPrefs_Click");
                EscribirLogDebug($"BtnLoadPrefs_Click ERROR: {ex.Message}");
                MessageBox.Show($"Error al cargar preferencias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                EscribirLogDebug("BtnLoadPrefs_Click SALIDA");
            }
        }

        private void LstGames_DoubleClick(object sender, EventArgs e)
        {
            // Implementar edición de juego si es necesario
        }

        private void LstGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstMain.SelectedIndex < 0)
                {
                    ActualizarBarraEstado(null);
                    return;
                }

                if (lstMain.SelectedIndex >= _servicio.FilteredList.Count)
                {
                    ActualizarBarraEstado(null);
                    return;
                }

                var gameIndex = _servicio.FilteredList[lstMain.SelectedIndex];
                if (gameIndex < 0 || gameIndex >= _servicio.GameList.Count)
                {
                    ActualizarBarraEstado(null);
                    return;
                }

                var game = _servicio.GameList[gameIndex];
                ActualizarBarraEstado(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LstGames_SelectedIndexChanged");
            }
        }

        private void ActualizarListaJuegos()
        {
            try
            {
                lstMain.BeginUpdate();
                lstMain.Items.Clear();

                // Configurar el ListBox para permitir colores personalizados (solo una vez)
                if (!_drawItemConfigurado)
                {
                    lstMain.DrawMode = DrawMode.OwnerDrawFixed;
                    lstMain.DrawItem += LstMain_DrawItem;
                    _drawItemConfigurado = true;
                }

                var items = _servicio.FilteredList
                    .Where(index => index >= 0 && index < _servicio.GameList.Count)
                    .Select(index =>
                    {
                        var game = _servicio.GameList[index];
                        return game.FileAvailable ? game.FileName : $"{game.FileName} (Missing)";
                    })
                    .Cast<object>()
                    .ToArray();

                if (items.Length > 0)
                {
                    lstMain.Items.AddRange(items);
                }

                /* lblStatus.Text = $"Mostrando {_servicio.FilteredList.Count} de {_servicio.GameList.Count} juegos";
                 EscribirLogDebug($"ActualizarListaJuegos OK items={lstMain.Items.Count}");*/
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando lista de juegos");
                EscribirLogDebug($"ActualizarListaJuegos ERROR: {ex.Message}");
                MessageBox.Show($"Error actualizando lista: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    lstMain.EndUpdate();
                }
                catch
                {
                    // ignorar
                }
            }
        }

        private void LstMain_DrawItem(object? sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index < 0 || e.Index >= lstMain.Items.Count)
                    return;

                // Obtener el juego correspondiente
                var gameIndex = _servicio.FilteredList[e.Index];
                var game = _servicio.GameList[gameIndex];

                // Dibujar el fondo
                e.DrawBackground();

                // Determinar el color del texto
                Color textColor = game.FileAvailable ? Color.LimeGreen : Color.Red;

                // Dibujar el texto
                using (var brush = new SolidBrush(textColor))
                {
                    e.Graphics.DrawString(lstMain.Items[e.Index].ToString(), e.Font, brush, e.Bounds);
                }

                // Dibujar el foco si está seleccionado
                e.DrawFocusRectangle();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LstMain_DrawItem");
            }
        }

        private void ActualizarTitulo()
        {
            try
            {
                var totalSize = _servicio.FilteredList.Sum(i =>
                    i >= 0 && i < _servicio.GameList.Count ? _servicio.GameList[i].FileSize : 0);

                var totalSizeMB = totalSize / (1024.0 * 1024.0);

                this.Text = $"WHDLoad Tools v1.7 - (Mostrando {_servicio.FilteredList.Count} de {_servicio.GameList.Count}) - ({totalSizeMB:F2} MB)";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ActualizarTitulo");
            }
        }

        private void ActualizarFiltrosDesdeInterfaz()
        {
            var filter = _servicio.Filter;

            filter.FGames = chkGames.Checked;
            filter.FDemos = chkDemos.Checked;
            filter.FBetaGame = chkBetaGames.Checked;
            filter.FBetaDemo = chkBetaDemos.Checked;
            filter.FMags = chkMagazines.Checked;

            // Filtros de sistema y chipset
            filter.FAGA = chkAGA.Checked;
            filter.FECS = chkECS.Checked;
            filter.FNTSC = chkNTSC.Checked;
            filter.FPAL = chkPAL.Checked;
            filter.FAmiga = chkAmiga.Checked;
            filter.FArcadia = chkArcadia.Checked;
            filter.FCD32 = chkCD32.Checked;
            filter.FCDTV = chkCDTV.Checked;
            filter.FCDROM = chkCDROM.Checked;

            // Filtros de sonido
            filter.FMT32 = chkMT32.Checked;
            filter.FNoVoice = chkNoVoice.Checked;
            filter.FNoSpeech = chkNoSpeech.Checked;
            filter.FNoMusic = chkNoMusic.Checked;
            filter.FNoMovie = chkNoMovie.Checked;

            // Filtros Misc
            filter.FFiles = chkFiles.Checked;
            filter.FImage = chkImage.Checked;
            filter.F1Disk = chk1Disk.Checked;
            filter.F2Disk = chk2Disk.Checked;
            filter.F3Disk = chk3Disk.Checked;
            filter.F4Disk = chk4Disk.Checked;
            filter.FHiRes = chkHiRes.Checked;
            filter.FLoRes = chkLoRes.Checked;
            filter.FNoIntro = chkNoIntro.Checked;
            filter.FPreRelease = chkPreRelease.Checked;
            filter.FPreview = chkPreviewMisc.Checked;
            filter.FEnhanced = chkEnhanced.Checked;
            filter.FGameDemo = chkGameDemo.Checked;
            filter.FUnCensored = chkUnCensored.Checked;
            filter.FCensored = chkCensored.Checked;

            // Filtros de idioma
            filter.FEnglish = chkEnglish.Checked;
            filter.FSpanish = chkSpanish.Checked;
            filter.FFrench = chkFrench.Checked;
            filter.FGerman = chkGerman.Checked;
            filter.FCroatian = chkCroatian.Checked;
            filter.FCzech = chkCzech.Checked;
            filter.FDanish = chkDanish.Checked;
            filter.FDutch = chkDutch.Checked;
            filter.FFinnish = chkFinnish.Checked;
            filter.FGreek = chkGreek.Checked;
            filter.FItalian = chkItalian.Checked;
            filter.FMulti = chkMulti.Checked;
            filter.FPolish = chkPolish.Checked;
            filter.FSwedish = chkSwedish.Checked;

            // Filtros de memoria/hardware
            filter.FChip = chkChip.Checked;
            filter.FFast = chkFast.Checked;
            filter.F512K = chk512k.Checked;
            filter.F512KB = chk512KB.Checked;
            filter.F1MB = chk1MB.Checked;
            filter.F1MBCHIP = chk1MBChp.Checked;
            filter.F1_5MB = chk15MB.Checked;
            filter.F2MB = chk2MB.Checked;
            filter.F8MB = chk8MB.Checked;
            filter.F12MB = chk12MB.Checked;
            filter.FLowMem = chkLowMen.Checked;
            filter.FSlowMem = chkSlowMm.Checked;
        }

        private void ActualizarFiltrosHaciaInterfaz()
        {
            var filter = _servicio.Filter;

            _suspenderEventosFiltros = true;
            try
            {
                chkGames.Checked = filter.FGames;
                chkDemos.Checked = filter.FDemos;
                chkBetaGames.Checked = filter.FBetaGame;
                chkBetaDemos.Checked = filter.FBetaDemo;
                chkMagazines.Checked = filter.FMags;

                // Filtros de sistema y chipset
                chkAGA.Checked = filter.FAGA;
                chkECS.Checked = filter.FECS;
                chkNTSC.Checked = filter.FNTSC;
                chkPAL.Checked = filter.FPAL;
                chkAmiga.Checked = filter.FAmiga;
                chkArcadia.Checked = filter.FArcadia;
                chkCD32.Checked = filter.FCD32;
                chkCDTV.Checked = filter.FCDTV;
                chkCDROM.Checked = filter.FCDROM;

                // Filtros de sonido
                chkMT32.Checked = filter.FMT32;
                chkNoVoice.Checked = filter.FNoVoice;
                chkNoSpeech.Checked = filter.FNoSpeech;
                chkNoMusic.Checked = filter.FNoMusic;
                chkNoMovie.Checked = filter.FNoMovie;

                // Filtros Misc
                chkFiles.Checked = filter.FFiles;
                chkImage.Checked = filter.FImage;
                chk1Disk.Checked = filter.F1Disk;
                chk2Disk.Checked = filter.F2Disk;
                chk3Disk.Checked = filter.F3Disk;
                chk4Disk.Checked = filter.F4Disk;
                chkHiRes.Checked = filter.FHiRes;
                chkLoRes.Checked = filter.FLoRes;
                chkNoIntro.Checked = filter.FNoIntro;
                chkPreRelease.Checked = filter.FPreRelease;
                chkPreviewMisc.Checked = filter.FPreview;
                chkEnhanced.Checked = filter.FEnhanced;
                chkGameDemo.Checked = filter.FGameDemo;
                chkUnCensored.Checked = filter.FUnCensored;
                chkCensored.Checked = filter.FCensored;

                // Filtros de idioma
                chkEnglish.Checked = filter.FEnglish;
                chkSpanish.Checked = filter.FSpanish;
                chkFrench.Checked = filter.FFrench;
                chkGerman.Checked = filter.FGerman;
                chkCroatian.Checked = filter.FCroatian;
                chkCzech.Checked = filter.FCzech;
                chkDanish.Checked = filter.FDanish;
                chkDutch.Checked = filter.FDutch;
                chkFinnish.Checked = filter.FFinnish;
                chkGreek.Checked = filter.FGreek;
                chkItalian.Checked = filter.FItalian;
                chkMulti.Checked = filter.FMulti;
                chkPolish.Checked = filter.FPolish;
                chkSwedish.Checked = filter.FSwedish;

                // Filtros de memoria/hardware
                chkChip.Checked = filter.FChip;
                chkFast.Checked = filter.FFast;
                chk512k.Checked = filter.F512K;
                chk512KB.Checked = filter.F512KB;
                chk1MB.Checked = filter.F1MB;
                chk1MBChp.Checked = filter.F1MBCHIP;
                chk15MB.Checked = filter.F1_5MB;
                chk2MB.Checked = filter.F2MB;
                chk8MB.Checked = filter.F8MB;
                chk12MB.Checked = filter.F12MB;
                chkLowMen.Checked = filter.FLowMem;
                chkSlowMm.Checked = filter.FSlowMem;
            }
            finally
            {
                _suspenderEventosFiltros = false;
            }
        }

        private void CrearDirectoriosSiNoExisten()
        {
            try
            {
                var directorios = new[]
                {
                    _servicio.Settings.WhdFolder,
                    Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdGameFolder),
                    Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdDemoFolder),
                    Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdBetaGameFolder),
                    Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdBetaDemoFolder),
                    Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdMagsFolder)
                };

                foreach (var directorio in directorios)
                {
                    if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
                    {
                        Directory.CreateDirectory(directorio);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear directorios");
            }
        }

        private async Task CargarPreferenciasPorDefectoAsync()
        {
            try
            {
                var defaultPrefsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.prefs");
                if (File.Exists(defaultPrefsPath))
                {
                    await CargarPreferenciasAsync(defaultPrefsPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar preferencias por defecto");
            }
        }

        private async Task GuardarPreferenciasAsync()
        {
            try
            {
                var prefsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _servicio.Settings.PrefsName);
                await GuardarPreferenciasAsync(prefsPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar preferencias");
                throw;
            }
        }

        private async Task GuardarPreferenciasAsync(string filePath)
        {
            var settings = _servicio.Settings;
            var filter = _servicio.Filter;

            var lines = new List<string>
            {
                "[FTP]",
                $"FTP_User={settings.FtpUser}",
                $"FTP_Pass={settings.FtpPass}",
                $"FTP_Server={settings.FtpServer}",
                $"HTTP_Server={settings.HttpServer}",
                $"FTP_Port={settings.FtpPort}",
                $"FTP_Folder={settings.FtpFolder}",
                $"FTP_Game_Folder={settings.FtpGameFolder}",
                $"FTP_Demo_Folder={settings.FtpDemoFolder}",
                $"FTP_Beta_Folder1={settings.FtpBetaGameFolder}",
                $"FTP_Beta_Folder2={settings.FtpBetaDemoFolder}",
                $"FTP_Magazine_Folder={settings.FtpMagsFolder}",
                $"Download_Type={settings.DownloadType}",
                "",
                "[Paths]",
                $"WHD_Path={settings.WhdFolder}",
                $"WHD_Game={settings.WhdGameFolder}",
                $"WHD_Demo={settings.WhdDemoFolder}",
                $"WHD_Beta_Game={settings.WhdBetaGameFolder}",
                $"WHD_Beta_Demo={settings.WhdBetaDemoFolder}",
                $"WHD_Mags={settings.WhdMagsFolder}",
                $"WHD_Sort={settings.SortType}",
                $"WHD_Language_Split={settings.SplitLanguages}",
                "",
                "[Filter]",
                $"Filter_Games={(filter.FGames ? 1 : 0)}",
                $"Filter_Demos={(filter.FDemos ? 1 : 0)}",
                $"Filter_Beta_Game={(filter.FBetaGame ? 1 : 0)}",
                $"Filter_Beta_Demo={(filter.FBetaDemo ? 1 : 0)}",
                $"Filter_Mags={(filter.FMags ? 1 : 0)}",
                $"Filter_AGA={(filter.FAGA ? 1 : 0)}",
                $"Filter_ECS={(filter.FECS ? 1 : 0)}",
                $"Filter_NTSC={(filter.FNTSC ? 1 : 0)}",
                $"Filter_PAL={(filter.FPAL ? 1 : 0)}",
                $"Filter_Amiga={(filter.FAmiga ? 1 : 0)}",
                $"Filter_Arcadia={(filter.FArcadia ? 1 : 0)}",
                $"Filter_CD32={(filter.FCD32 ? 1 : 0)}",
                $"Filter_CDTV={(filter.FCDTV ? 1 : 0)}",
                $"Filter_CDROM={(filter.FCDROM ? 1 : 0)}",
                $"Filter_Files={(filter.FFiles ? 1 : 0)}",
                $"Filter_Image={(filter.FImage ? 1 : 0)}",
                $"Filter_Chip={(filter.FChip ? 1 : 0)}",
                $"Filter_Fast={(filter.FFast ? 1 : 0)}",
                $"Filter_Croatian={(filter.FCroatian ? 1 : 0)}",
                $"Filter_Czech={(filter.FCzech ? 1 : 0)}",
                $"Filter_Danish={(filter.FDanish ? 1 : 0)}",
                $"Filter_Dutch={(filter.FDutch ? 1 : 0)}",
                $"Filter_English={(filter.FEnglish ? 1 : 0)}",
                $"Filter_Finnish={(filter.FFinnish ? 1 : 0)}",
                $"Filter_French={(filter.FFrench ? 1 : 0)}",
                $"Filter_German={(filter.FGerman ? 1 : 0)}",
                $"Filter_Greek={(filter.FGreek ? 1 : 0)}",
                $"Filter_Italian={(filter.FItalian ? 1 : 0)}",
                $"Filter_Multi={(filter.FMulti ? 1 : 0)}",
                $"Filter_Polish={(filter.FPolish ? 1 : 0)}",
                $"Filter_Spanish={(filter.FSpanish ? 1 : 0)}",
                $"Filter_Swedish={(filter.FSwedish ? 1 : 0)}",
                $"Filter_512K={(filter.F512K ? 1 : 0)}",
                $"Filter_512KB={(filter.F512KB ? 1 : 0)}",
                $"Filter_1MB={(filter.F1MB ? 1 : 0)}",
                $"Filter_1_5MB={(filter.F1_5MB ? 1 : 0)}",
                $"Filter_1MBCHIP={(filter.F1MBCHIP ? 1 : 0)}",
                $"Filter_2MB={(filter.F2MB ? 1 : 0)}",
                $"Filter_8MB={(filter.F8MB ? 1 : 0)}",
                $"Filter_12MB={(filter.F12MB ? 1 : 0)}",
                $"Filter_LowMem={(filter.FLowMem ? 1 : 0)}",
                $"Filter_SlowMem={(filter.FSlowMem ? 1 : 0)}",
                $"Filter_NoIntro={(filter.FNoIntro ? 1 : 0)}",
                $"Filter_MT32={(filter.FMT32 ? 1 : 0)}",
                $"Filter_NoVoice={(filter.FNoVoice ? 1 : 0)}",
                $"Filter_NoSpeech={(filter.FNoSpeech ? 1 : 0)}",
                $"Filter_NoMusic={(filter.FNoMusic ? 1 : 0)}",
                $"Filter_NoMovie={(filter.FNoMovie ? 1 : 0)}",
                $"Filter_1Disk={(filter.F1Disk ? 1 : 0)}",
                $"Filter_2Disk={(filter.F2Disk ? 1 : 0)}",
                $"Filter_3Disk={(filter.F3Disk ? 1 : 0)}",
                $"Filter_4Disk={(filter.F4Disk ? 1 : 0)}",
                $"Filter_HiRes={(filter.FHiRes ? 1 : 0)}",
                $"Filter_LoRes={(filter.FLoRes ? 1 : 0)}",
                $"Filter_GameDemo={(filter.FGameDemo ? 1 : 0)}",
                $"Filter_Preview={(filter.FPreview ? 1 : 0)}",
                $"Filter_PreRelease={(filter.FPreRelease ? 1 : 0)}",
                $"Filter_Enhanced={(filter.FEnhanced ? 1 : 0)}",
                $"Filter_Censored={(filter.FCensored ? 1 : 0)}",
                $"Filter_UnCensored={(filter.FUnCensored ? 1 : 0)}",
                "",
                "[Compat]",
                $"WHD_Folder={settings.WhdFolder}",
                $"WHD_Game_Folder={settings.WhdGameFolder}",
                $"WHD_Demo_Folder={settings.WhdDemoFolder}",
                $"WHD_Beta_Game_Folder={settings.WhdBetaGameFolder}",
                $"WHD_Beta_Demo_Folder={settings.WhdBetaDemoFolder}",
                $"WHD_Mags_Folder={settings.WhdMagsFolder}",
                $"FTP_Mags_Folder={settings.FtpMagsFolder}",
                $"Sort_Type={settings.SortType}",
                $"Split_Languages={settings.SplitLanguages}",
                $"A500Mini={settings.A500Mini}"
            };

            await File.WriteAllLinesAsync(filePath, lines);
        }

        private async Task CargarPreferenciasAsync(string filePath)
        {
            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);
                var settings = _servicio.Settings;
                var filter = _servicio.Filter;

                foreach (var line in lines)
                {
                    var linea = line.Trim();
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    if (linea.StartsWith(";", StringComparison.Ordinal) || linea.StartsWith("#", StringComparison.Ordinal)) continue;
                    if (linea.StartsWith("[", StringComparison.Ordinal) && linea.EndsWith("]", StringComparison.Ordinal)) continue;
                    if (!linea.Contains("=")) continue;

                    var parts = linea.Split('=', 2);
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    bool LeerBoolDesdePrefs(string v)
                    {
                        if (string.Equals(v, "1", StringComparison.OrdinalIgnoreCase)) return true;
                        if (string.Equals(v, "0", StringComparison.OrdinalIgnoreCase)) return false;
                        if (bool.TryParse(v, out var b)) return b;
                        if (int.TryParse(v, out var i)) return i != 0;
                        return false;
                    }

                    switch (key)
                    {
                        case "WHD_Folder":
                        case "WHD_Path":
                            settings.WhdFolder = value;
                            break;
                        case "WHD_Game_Folder":
                        case "WHD_Game":
                            settings.WhdGameFolder = value;
                            break;
                        case "WHD_Demo_Folder":
                        case "WHD_Demo":
                            settings.WhdDemoFolder = value;
                            break;
                        case "WHD_Beta_Game_Folder":
                        case "WHD_Beta_Game":
                            settings.WhdBetaGameFolder = value;
                            break;
                        case "WHD_Beta_Demo_Folder":
                        case "WHD_Beta_Demo":
                            settings.WhdBetaDemoFolder = value;
                            break;
                        case "WHD_Mags_Folder":
                        case "WHD_Mags":
                            settings.WhdMagsFolder = value;
                            break;
                        case "FTP_Server":
                            settings.FtpServer = value;
                            break;
                        case "FTP_User":
                            settings.FtpUser = value;
                            break;
                        case "FTP_Pass":
                            settings.FtpPass = value;
                            break;
                        case "FTP_Port":
                            if (int.TryParse(value, out var port))
                                settings.FtpPort = port;
                            break;
                        case "FTP_Folder":
                            settings.FtpFolder = value;
                            break;
                        case "FTP_Game_Folder":
                            settings.FtpGameFolder = value;
                            break;
                        case "FTP_Demo_Folder":
                            settings.FtpDemoFolder = value;
                            break;
                        case "FTP_Beta_Game_Folder":
                        case "FTP_Beta_Folder1":
                            settings.FtpBetaGameFolder = value;
                            break;
                        case "FTP_Beta_Demo_Folder":
                        case "FTP_Beta_Folder2":
                            settings.FtpBetaDemoFolder = value;
                            break;
                        case "FTP_Mags_Folder":
                        case "FTP_Magazine_Folder":
                            settings.FtpMagsFolder = value;
                            break;
                        case "HTTP_Server":
                            settings.HttpServer = value;
                            break;
                        case "Download_Type":
                            if (int.TryParse(value, out var downloadType))
                                settings.DownloadType = downloadType;
                            break;
                        case "Sort_Type":
                        case "WHD_Sort":
                            if (int.TryParse(value, out var sortType))
                                settings.SortType = sortType;
                            break;
                        case "Split_Languages":
                        case "WHD_Language_Split":
                            if (int.TryParse(value, out var splitLanguages))
                                settings.SplitLanguages = splitLanguages;
                            break;
                        case "A500Mini":
                            settings.A500Mini = LeerBoolDesdePrefs(value);
                            break;

                        case "Filter_Games":
                            filter.FGames = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Demos":
                            filter.FDemos = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Beta_Game":
                            filter.FBetaGame = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Beta_Demo":
                            filter.FBetaDemo = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Mags":
                            filter.FMags = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_AGA":
                            filter.FAGA = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_ECS":
                            filter.FECS = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_NTSC":
                            filter.FNTSC = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_PAL":
                            filter.FPAL = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Amiga":
                            filter.FAmiga = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Arcadia":
                            filter.FArcadia = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_CD32":
                            filter.FCD32 = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_CDTV":
                            filter.FCDTV = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_CDROM":
                            filter.FCDROM = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Files":
                            filter.FFiles = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Image":
                            filter.FImage = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Chip":
                            filter.FChip = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Fast":
                            filter.FFast = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Croatian":
                            filter.FCroatian = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Czech":
                            filter.FCzech = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Danish":
                            filter.FDanish = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Dutch":
                            filter.FDutch = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_English":
                            filter.FEnglish = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Finnish":
                            filter.FFinnish = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_French":
                            filter.FFrench = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_German":
                            filter.FGerman = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Greek":
                            filter.FGreek = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Italian":
                            filter.FItalian = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Multi":
                            filter.FMulti = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Polish":
                            filter.FPolish = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Spanish":
                            filter.FSpanish = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Swedish":
                            filter.FSwedish = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_512K":
                            filter.F512K = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_512KB":
                            filter.F512KB = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_1MB":
                            filter.F1MB = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_1_5MB":
                            filter.F1_5MB = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_1MBCHIP":
                            filter.F1MBCHIP = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_2MB":
                            filter.F2MB = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_8MB":
                            filter.F8MB = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_12MB":
                            filter.F12MB = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_LowMem":
                            filter.FLowMem = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_SlowMem":
                            filter.FSlowMem = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_NoIntro":
                            filter.FNoIntro = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_MT32":
                            filter.FMT32 = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_NoVoice":
                            filter.FNoVoice = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_NoSpeech":
                            filter.FNoSpeech = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_NoMusic":
                            filter.FNoMusic = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_NoMovie":
                            filter.FNoMovie = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_1Disk":
                            filter.F1Disk = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_2Disk":
                            filter.F2Disk = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_3Disk":
                            filter.F3Disk = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_4Disk":
                            filter.F4Disk = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_HiRes":
                            filter.FHiRes = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_LoRes":
                            filter.FLoRes = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_GameDemo":
                            filter.FGameDemo = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Preview":
                            filter.FPreview = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_PreRelease":
                            filter.FPreRelease = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Enhanced":
                            filter.FEnhanced = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_Censored":
                            filter.FCensored = LeerBoolDesdePrefs(value);
                            break;
                        case "Filter_UnCensored":
                            filter.FUnCensored = LeerBoolDesdePrefs(value);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar preferencias desde {filePath}");
                throw;
            }
        }

        private void AbrirCarpetaSiExiste(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                MessageBox.Show("La ruta está vacía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(ruta))
            {
                MessageBox.Show($"La carpeta no existe:\n{ruta}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Diagnostics.Process.Start("explorer.exe", ruta);
        }

        private void BtnOpenMain_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                AbrirCarpetaSiExiste(_servicio.Settings.WhdFolder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenMain_Click");
            }
        }

        private async void BtnSetMain_Click(object sender, EventArgs e)
        {
            try
            {
                EscribirLogDebug("BtnSetMain_Click INICIO");
                EscribirLogDebug("BtnSetMain_Click mostrando dialogo carpeta");
                var carpeta = await SeleccionarCarpetaAsync("Seleccione la carpeta WHDLoad", _servicio.Settings.WhdFolder);
                EscribirLogDebug($"BtnSetMain_Click carpeta seleccionada='{carpeta ?? ""}'");

                if (!string.IsNullOrWhiteSpace(carpeta))
                {
                    _servicio.Settings.WhdFolder = carpeta;
                    txtWHDMain.Text = carpeta;
                    EscribirLogDebug($"BtnSetMain_Click carpeta='{_servicio.Settings.WhdFolder}'");
                    _ = AplicarCambioDeCarpetaAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnSetMain_Click");
                EscribirLogDebug($"BtnSetMain_Click ERROR: {ex.Message}");
            }
            finally
            {
                EscribirLogDebug("BtnSetMain_Click SALIDA");
            }
        }

        private void BtnOpenGames_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                AbrirCarpetaSiExiste(Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdGameFolder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenGames_Click");
            }
        }

        private void BtnOpenDemos_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                AbrirCarpetaSiExiste(Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdDemoFolder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenDemos_Click");
            }
        }

        private void BtnOpenBetaGames_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                AbrirCarpetaSiExiste(Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdBetaGameFolder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenBetaGames_Click");
            }
        }

        private void BtnOpenBetaDemos_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                AbrirCarpetaSiExiste(Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdBetaDemoFolder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenBetaDemos_Click");
            }
        }

        private void BtnOpenMags_Click(object sender, EventArgs e)
        {
            try
            {
                ActualizarConfiguracionDesdeInterfaz();
                AbrirCarpetaSiExiste(Path.Combine(_servicio.Settings.WhdFolder, _servicio.Settings.WhdMagsFolder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BtnOpenMags_Click");
            }
        }

        private void RefrescarLista()
        {
            try
            {
                _servicio.FilterList();
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al refrescar lista");
            }
        }

        private void EstablecerUiOcupada(bool ocupado)
        {
            Cursor.Current = ocupado ? Cursors.WaitCursor : Cursors.Default;
            btnMakeFolder.Enabled = !ocupado;
            btnDownload.Enabled = !ocupado;
            btnScan.Enabled = !ocupado;
            lstMain.Enabled = !ocupado;
        }

        private async Task RefrescarListaAsync()
        {
            try
            {
                await Task.Run(() => _servicio.FilterList());
                ActualizarListaJuegos();
                ActualizarTitulo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al refrescar lista");
            }
        }

        private async Task AplicarCambioDeCarpetaAsync()
        {
            EstablecerUiOcupada(true);
            try
            {
                await Task.Run(() => CrearDirectoriosSiNoExisten());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aplicando cambio de carpeta");
            }
            finally
            {
                EstablecerUiOcupada(false);
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {

        }
    }
}
