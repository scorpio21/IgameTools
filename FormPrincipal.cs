using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualBasic;
using IgameToolsWinForms.Servicios;

namespace IgameToolsWinForms;

public partial class FormPrincipal : Form
{
    private static string Version => 
        System.Reflection.Assembly.GetExecutingAssembly()
            .GetName().Version?.ToString() ?? "0.1.7";

    private string _textoBusqueda = string.Empty;
    private int _columnaOrden = 0;
    private bool _ordenAscendente = true;

    // Sistema de Undo/Redo
    internal interface ICommand
    {
        string Description { get; }
        void Execute();
        void Undo();
    }

    internal class EditarJuegoCommand : ICommand
    {
        private readonly List<Juego> _juegos;
        private readonly int _indice;
        private readonly Juego _juegoOriginal;
        private readonly Juego _juegoNuevo;

        public string Description { get; }

        public EditarJuegoCommand(List<Juego> juegos, int indice, Juego juegoOriginal, Juego juegoNuevo)
        {
            _juegos = juegos;
            _indice = indice;
            _juegoOriginal = juegoOriginal;
            _juegoNuevo = juegoNuevo;
            Description = $"Editar juego: {juegoOriginal.Nombre} → {juegoNuevo.Nombre}";
        }

        public void Execute()
        {
            if (_indice >= 0 && _indice < _juegos.Count)
            {
                _juegos[_indice] = _juegoNuevo;
            }
        }

        public void Undo()
        {
            if (_indice >= 0 && _indice < _juegos.Count)
            {
                _juegos[_indice] = _juegoOriginal;
            }
        }
    }

    internal class QuickTagCommand : ICommand
    {
        private readonly List<Juego> _juegos;
        private readonly List<int> _indices;
        private readonly string _etiqueta;
        private readonly List<Juego> _juegosOriginales;

        public string Description { get; }

        public QuickTagCommand(List<Juego> juegos, List<int> indices, string etiqueta)
        {
            _juegos = juegos;
            _indices = indices;
            _etiqueta = etiqueta;
            _juegosOriginales = indices.Select(i => juegos[i]).ToList();
            Description = $"Quick Tag: '{etiqueta}' en {_indices.Count} juego(s)";
        }

        public void Execute()
        {
            foreach (var indice in _indices)
            {
                if (indice >= 0 && indice < _juegos.Count)
                {
                    var juego = _juegos[indice];
                    _juegos[indice] = juego with { Nombre = $"{juego.Nombre}{_etiqueta}" };
                }
            }
        }

        public void Undo()
        {
            for (int i = 0; i < _indices.Count; i++)
            {
                var indice = _indices[i];
                if (indice >= 0 && indice < _juegos.Count)
                {
                    _juegos[indice] = _juegosOriginales[i];
                }
            }
        }
    }

    internal class FixListCommand : ICommand
    {
        private readonly List<Juego> _juegos;
        private readonly List<Juego> _juegosOriginales;
        private readonly List<Juego> _juegosNuevos;

        public string Description { get; }

        public FixListCommand(List<Juego> juegos, List<Juego> juegosNuevos)
        {
            _juegos = juegos;
            _juegosOriginales = new List<Juego>(juegos);
            _juegosNuevos = new List<Juego>(juegosNuevos);
            Description = $"Fix List: {_juegosNuevos.Count} juegos procesados";
        }

        public void Execute()
        {
            _juegos.Clear();
            _juegos.AddRange(_juegosNuevos);
        }

        public void Undo()
        {
            _juegos.Clear();
            _juegos.AddRange(_juegosOriginales);
        }
    }

    internal class LimpiarListaCommand : ICommand
    {
        private readonly List<Juego> _juegos;
        private readonly List<Juego> _juegosOriginales;

        public string Description { get; }

        public LimpiarListaCommand(List<Juego> juegos)
        {
            _juegos = juegos;
            _juegosOriginales = new List<Juego>(juegos);
            Description = $"Limpiar lista: {_juegosOriginales.Count} juegos eliminados";
        }

        public void Execute()
        {
            _juegos.Clear();
        }

        public void Undo()
        {
            _juegos.Clear();
            _juegos.AddRange(_juegosOriginales);
        }
    }

    internal class UndoRedoManager
    {
        private readonly Stack<ICommand> _undoStack = new();
        private readonly Stack<ICommand> _redoStack = new();
        private readonly int _maxHistorySize;

        public UndoRedoManager(int maxHistorySize = 50)
        {
            _maxHistorySize = maxHistorySize;
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();

            while (_undoStack.Count > _maxHistorySize)
            {
                var items = _undoStack.ToArray();
                _undoStack.Clear();
                for (int i = items.Length - 1; i >= Math.Max(0, items.Length - _maxHistorySize); i--)
                {
                    _undoStack.Push(items[i]);
                }
            }
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public string? UndoDescription => CanUndo ? _undoStack.Peek().Description : null;
        public string? RedoDescription => CanRedo ? _redoStack.Peek().Description : null;

        public void Undo()
        {
            if (!CanUndo) return;

            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }

        public void Redo()
        {
            if (!CanRedo) return;

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }

    private sealed class FormEstadoFix : Form
    {
        private readonly Label _lblTitulo;
        private readonly Label _lblDetalle;

        public FormEstadoFix(Servicios.EstadoFix estadoInicial)
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Left = 16;
            Top = 52;
            Width = 500; // Mantenido en 500px
            Height = 200; // Aumentado a 200px para más espacio
            Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 9f, FontStyle.Bold);
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.White; // Fondo blanco para mejor legibilidad

            // Inicializar los labels
            _lblTitulo = new Label
            {
                Left = 10,
                Top = 10,
                Width = 480, // Casi todo el ancho
                Height = 40, // Más alto para el título
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 12f, FontStyle.Bold), // Título más grande
                ForeColor = Color.DarkBlue, // Color contrastante
                BackColor = Color.White
            };

            _lblDetalle = new Label
            {
                Left = 10,
                Top = 55, // Espacio para el título
                Width = 480, // Casi todo el ancho
                Height = 120, // Aumentado a 120px para mensajes largos
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 10f, FontStyle.Regular), // Detalle legible
                ForeColor = Color.Black, // Negro para máximo contraste
                BackColor = Color.White
            };

            Controls.Add(_lblTitulo);
            Controls.Add(_lblDetalle);

            Actualizar(estadoInicial);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (Owner == null)
            {
                CenterToScreen();
                return;
            }

            var ownerBounds = Owner.Bounds;
            Left = ownerBounds.Left + (ownerBounds.Width - Width) / 2;
            Top = ownerBounds.Top + (ownerBounds.Height - Height) / 2;
        }

        public void Actualizar(Servicios.EstadoFix estado)
        {
            Text = estado.Titulo;
            _lblTitulo.Text = estado.Titulo;
            _lblDetalle.Text = estado.Detalle;
            Refresh();
        }
    }
    
    private sealed class CompData
    {
        public required string Nombre { get; init; }
        public required string NombreCorto { get; init; }
        public required string Genero { get; init; }
        public string Folder { get; init; } = string.Empty;
    }

    private readonly List<Juego> _juegos = new();
    private string _rutaCsvActual = string.Empty;

    // TextBox de búsqueda (añadir desde el diseñador)
    private TextBox? txtBusquedaControl;

    // Settings de la aplicación
    private AppSettings _settings = new();

    // Servicios de negocio
    private readonly ServicioCsv _servicioCsv = new();
    private readonly ServicioFixList _servicioFixList = new();
    private readonly ServicioUndo _servicioUndo = new();
    private readonly ServicioJuegos _servicioJuegos = new();

    public FormPrincipal()
    {
        InitializeComponent();
        
        // Configurar icono del formulario
        try
        {
            var iconPath = Path.Combine(AppContext.BaseDirectory, "img", "amiga.png");
            if (File.Exists(iconPath))
            {
                // Cargar la imagen como icono del formulario
                using var bitmap = new Bitmap(iconPath);
                Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
            }
        }
        catch
        {
            // Si no se puede cargar el icono, continuar sin él
        }

        // Conectar evento ColumnClick para ordenación
        listaJuegos.ColumnClick += ListaJuegos_ColumnClick;
        
        // Conectar evento TextChanged para búsqueda
        txtBusqueda.TextChanged += TxtBusqueda_TextChanged;

        Resize += FormPrincipal_Resize;
    }

    private void FormPrincipal_Resize(object? sender, EventArgs e)
    {
        AjustarLayoutInferior();
    }

    private void AjustarLayoutInferior()
    {
        if (!IsHandleCreated)
        {
            return;
        }

        const int margen = 10;
        const int anchoMinimoBusqueda = 120;

        var xIzquierda = Math.Max(
            Math.Max(chkVerDuplicados.Right, chkVerDesconocidos.Right),
            Math.Max(chkMantenerDatos.Right, chkNombresCortos.Right)) + margen;

        var xDerecha = Math.Min(lblTitleCase.Left, cmbTitleCase.Left);
        xDerecha -= margen;

        var anchoDisponible = xDerecha - xIzquierda;
        if (anchoDisponible <= 0)
        {
            return;
        }

        txtBusqueda.Left = xIzquierda;
        txtBusqueda.Width = Math.Max(anchoMinimoBusqueda, anchoDisponible);

        if (txtBusqueda.Right > xDerecha)
        {
            txtBusqueda.Width = Math.Max(20, xDerecha - txtBusqueda.Left);
        }
    }

    private void TxtBusqueda_TextChanged(object? sender, EventArgs e)
    {
        if (sender is TextBox txt)
        {
            _textoBusqueda = txt.Text;
            DibujarLista();
        }
    }

    private void ListaJuegos_ColumnClick(object? sender, ColumnClickEventArgs e)
    {
        // Si se hace clic en la misma columna, invertir dirección
        if (_columnaOrden == e.Column)
        {
            _ordenAscendente = !_ordenAscendente;
        }
        else
        {
            _columnaOrden = e.Column;
            _ordenAscendente = true;
        }

        DibujarLista();
    }

    private void FormPrincipal_Resize_Debug(object? sender, EventArgs e)
    {
        // Mostrar información de depuración en tiempo real
        var formInfo = $"FORMULARIO: Size={Size}, ClientSize={ClientSize}";
        var panelInfo = $"PANEL ESTADÍSTICAS: Location={panelEstadisticas.Location}, Size={panelEstadisticas.Size}";
        var panelEndX = panelEstadisticas.Location.X + panelEstadisticas.Size.Width;
        var marginInfo = $"MARGEN DERECHO: {ClientSize.Width - panelEndX}px";
        
        // Actualizar el título del formulario con la información
        Text = $"Igame Tools - DEBUG | {formInfo} | {panelInfo} | {marginInfo}";
    }

    private void FormPrincipal_Load_Debug(object? sender, EventArgs e)
    {
        // Mostrar información de depuración temporal
        var formInfo = $"FORMULARIO: Size={Size}, ClientSize={ClientSize}";
        var panelInfo = $"PANEL ESTADÍSTICAS: Location={panelEstadisticas.Location}, Size={panelEstadisticas.Size}";
        var panelEndX = panelEstadisticas.Location.X + panelEstadisticas.Size.Width;
        var marginInfo = $"MARGEN DERECHO: {ClientSize.Width - panelEndX}px";
        
        MessageBox.Show($"{formInfo}\n\n{panelInfo}\n{marginInfo}\n\n" +
                       "Usa esta información para ajustar el tamaño perfecto", 
                       "INFO DE DEPURACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        // Llamar al método original
        FormPrincipal_Load(sender, e);
    }

    private void FormPrincipal_Load(object sender, EventArgs e)
    {
        // Cargar settings
        _settings = AppSettings.Load();

        // Asignar referencia al TextBox del diseñador
        txtBusquedaControl = txtBusqueda;

        // Inicializar panel de estadísticas
        panelEstadisticas.ActualizarEstadisticasRequested += PanelEstadisticas_ActualizarEstadisticasRequested;

        // Aplicar preferencias guardadas
        chkMantenerDatos.Checked = _settings.MantenerDatos;
        chkNombresCortos.Checked = _settings.NombresCortos;
        chkVerDuplicados.Checked = _settings.VerDuplicados;
        chkVerDesconocidos.Checked = _settings.VerDesconocidos;
        cmbTitleCase.SelectedIndex = _settings.TitleCaseIndex;

        // Aplicar tamaño/posición de ventana si está guardado
        if (_settings.WindowLeft >= 0 && _settings.WindowTop >= 0)
        {
            Left = _settings.WindowLeft;
            Top = _settings.WindowTop;
        }
        if (_settings.WindowWidth > 0 && _settings.WindowHeight > 0)
        {
            Width = _settings.WindowWidth;
            Height = _settings.WindowHeight;
        }

        chkNombresCortos.Enabled = false;
        chkMantenerDatos.Enabled = false;
        chkVerDuplicados.Enabled = false;
        chkVerDesconocidos.Enabled = false;
        cmbTitleCase.Enabled = false;

        btnArreglarLista.Enabled = false;
        btnGuardarCsv.Enabled = false;
        btnEtiquetaRapida.Enabled = false;
        btnLimpiarLista.Enabled = false;
        btnDeshacer.Enabled = false;

        // Deshabilitar búsqueda hasta que se cargue una lista
        if (txtBusquedaControl != null)
        {
            txtBusquedaControl.Enabled = false;
        }

        // Cargar último CSV si existe y está disponible (modo silencioso)
        if (!string.IsNullOrWhiteSpace(_settings.LastCsvFile) && File.Exists(_settings.LastCsvFile))
        {
            CargarCsvSilencioso(_settings.LastCsvFile);
        }

        ActualizarEstadoYTitulo();
        ActualizarBotones();

        AjustarLayoutInferior();
    }

    private void FormPrincipal_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // Guardar preferencias actuales
        _settings.MantenerDatos = chkMantenerDatos.Checked;
        _settings.NombresCortos = chkNombresCortos.Checked;
        _settings.VerDuplicados = chkVerDuplicados.Checked;
        _settings.VerDesconocidos = chkVerDesconocidos.Checked;
        _settings.TitleCaseIndex = cmbTitleCase.SelectedIndex;

        // Guardar tamaño/posición de la ventana (solo si no está minimizado/maximizado)
        if (WindowState == FormWindowState.Normal)
        {
            _settings.WindowLeft = Left;
            _settings.WindowTop = Top;
            _settings.WindowWidth = Width;
            _settings.WindowHeight = Height;
        }

        _settings.Save();
    }

    private void btnCargarCsv_Click(object sender, EventArgs e)
    {
        using var dialogo = new OpenFileDialog
        {
            Title = "Abrir gameslist.csv",
            Filter = "CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*",
            CheckFileExists = true,
            CheckPathExists = true
        };

        if (dialogo.ShowDialog(this) == DialogResult.OK)
        {
            CargarCsv(dialogo.FileName);
        }
    }

    private void CargarCsv(string rutaCsv)
    {
        var task = Task.Run(() =>
        {
            try
            {
                // Mostrar progreso en la interfaz principal
                MostrarProgresoPrincipal("Cargando archivo CSV...", indeterminado: true);
                
                // Usar el servicio CSV de forma síncrona
                var (juegos, validacion) = _servicioCsv.CargarCsv(rutaCsv);
                
                // Actualizar UI en el hilo principal
                this.Invoke(new Action(() =>
                {
                    if (!validacion.IsValid)
                    {
                        OcultarProgresoPrincipal();
                        var mensajeError = $"No se puede cargar el archivo CSV:\n\n{string.Join("\n", validacion.Errors)}";
                        MessageBox.Show(this, mensajeError, "Error de validación CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Mostrar advertencias si las hay
                    if (validacion.Warnings.Any())
                    {
                        OcultarProgresoPrincipal();
                        var mensajeAdvertencias = $"Advertencias al cargar el CSV:\n\n{string.Join("\n", validacion.Warnings)}\n\n¿Desea continuar?";
                        var resultado = MessageBox.Show(this, mensajeAdvertencias, "Advertencias CSV", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (resultado != DialogResult.Yes)
                        {
                            return;
                        }
                        MostrarProgresoPrincipal("Continuando carga del CSV...", indeterminado: true);
                    }

                    _rutaCsvActual = rutaCsv;
                    _juegos.Clear();
                    _juegos.AddRange(juegos);

                    // Limpiar panel de estadísticas Fix List al cargar nuevo CSV
                    panelEstadisticasFix.Limpiar();

                    // Guardar en settings
                    _settings.LastCsvFile = rutaCsv;
                    _settings.AddRecentFile(rutaCsv);
                    _settings.Save();

                    // Validar versión de IGame
                    ValidarVersionIGame(rutaCsv, juegos);

                    // Ocultar progreso
                    OcultarProgresoPrincipal();

                    DibujarLista();
                    ActualizarBotones();
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    OcultarProgresoPrincipal();
                    MessageBox.Show(this, ex.Message, "Error al cargar CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        });
    }

    private void DibujarLista()
    {
        // Guardar selección actual
        Juego? selectedJuego = null;
        if (listaJuegos.SelectedItems.Count > 0 && listaJuegos.SelectedItems[0].Tag is Juego juego)
        {
            selectedJuego = juego;
        }

        listaJuegos.BeginUpdate();
        try
        {
            listaJuegos.Items.Clear();

            var nombresDuplicados = CalcularNombresDuplicados(_juegos);
            var juegosParaMostrar = ObtenerJuegosParaMostrar(nombresDuplicados);

            foreach (var juegoItem in juegosParaMostrar)
            {
                var nombreAMostrar = chkNombresCortos.Checked ? juegoItem.NombreCorto : juegoItem.Nombre;
                nombreAMostrar = AplicarTitleCase(nombreAMostrar);

                // Debug temporal para nombres cortos
                if (chkNombresCortos.Checked && !string.IsNullOrEmpty(juegoItem.NombreCorto) && juegoItem.NombreCorto != juegoItem.Nombre)
                {
                    System.Diagnostics.Debug.WriteLine($"NOMBRE CORTO: '{juegoItem.Nombre}' -> '{juegoItem.NombreCorto}'");
                }

                var item = new ListViewItem(nombreAMostrar);
                item.SubItems.Add(juegoItem.Genero);
                item.SubItems.Add(juegoItem.Slave);
                item.SubItems.Add(juegoItem.Ruta);
                
                // Agregar preview con Title Case aplicado
                var nombreOriginal = chkNombresCortos.Checked ? juegoItem.NombreCorto : juegoItem.Nombre;
                var preview = AplicarTitleCase(nombreOriginal);
                item.SubItems.Add(preview);
                
                item.Tag = juegoItem;

                // Aplicar colores según el programa original IGame Tool
                AplicarColoreoAvanzado(item, juegoItem, nombresDuplicados);

                listaJuegos.Items.Add(item);

                // Restaurar selección si coincide
                if (selectedJuego.HasValue && juegoItem.Equals(selectedJuego.Value))
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }

            // Autoajustar columnas al contenido
            foreach (ColumnHeader column in listaJuegos.Columns)
            {
                column.Width = -1; // -1 = autoajuste al contenido
            }
            
            // Restaurar anchos guardados por el usuario
            RestaurarAnchoColumnas();
            
            // Ajustar última columna automáticamente
            AjustarColumnasAutomaticamente();
        }
        finally
        {
            listaJuegos.EndUpdate();
        }

        ActualizarEstadoYTitulo();
        ActualizarBotones();
        
        // Actualizar panel de estadísticas
        ActualizarEstadisticas();
    }

    private void PanelEstadisticas_ActualizarEstadisticasRequested(object? sender, EventArgs e)
    {
        ActualizarEstadisticas();
    }

    private void ActualizarEstadisticas()
    {
        try
        {
            panelEstadisticas.ActualizarEstadisticas(_juegos);
        }
        catch (Exception ex)
        {
            // Silenciar errores de estadísticas para no interrumpir el flujo principal
            System.Diagnostics.Debug.WriteLine($"Error al actualizar estadísticas: {ex.Message}");
        }
    }

    private void listaJuegos_ColumnWidthChanging(object? sender, ColumnWidthChangingEventArgs e)
    {
        // Guardar el ancho de las columnas cuando el usuario las ajusta
        GuardarAnchoColumnas();
    }

    private void listaJuegos_ColumnWidthChanged(object? sender, ColumnWidthChangedEventArgs e)
    {
        // Evitar recursión infinita - solo ajustar si no es la última columna
        if (e.ColumnIndex < 4) // Solo ajustar cuando cambian las primeras 4 columnas
        {
            AjustarColumnasAutomaticamente();
        }
    }

    private void GuardarAnchoColumnas()
    {
        // Guardar anchos de columnas en settings para persistencia
        if (listaJuegos.Columns.Count >= 5)
        {
            _settings.AnchoColumnas = new Dictionary<string, int>
            {
                ["Nombre"] = listaJuegos.Columns[0].Width,
                ["Genero"] = listaJuegos.Columns[1].Width,
                ["Slave"] = listaJuegos.Columns[2].Width,
                ["Ruta"] = listaJuegos.Columns[3].Width,
                ["Preview"] = listaJuegos.Columns[4].Width
            };
            _settings.Save();
        }
    }

    private void RestaurarAnchoColumnas()
    {
        // Restaurar anchos guardados si existen
        if (_settings.AnchoColumnas != null && listaJuegos.Columns.Count >= 5)
        {
            if (_settings.AnchoColumnas.TryGetValue("Nombre", out var anchoNombre))
                listaJuegos.Columns[0].Width = anchoNombre;
            if (_settings.AnchoColumnas.TryGetValue("Genero", out var anchoGenero))
                listaJuegos.Columns[1].Width = anchoGenero;
            if (_settings.AnchoColumnas.TryGetValue("Slave", out var anchoSlave))
                listaJuegos.Columns[2].Width = anchoSlave;
            if (_settings.AnchoColumnas.TryGetValue("Ruta", out var anchoRuta))
                listaJuegos.Columns[3].Width = anchoRuta;
            if (_settings.AnchoColumnas.TryGetValue("Preview", out var anchoPreview))
                listaJuegos.Columns[4].Width = anchoPreview;
        }
    }

    private void AjustarColumnasAutomaticamente()
    {
        // Ajustar la última columna para ocupar el espacio restante
        // Solo ejecutar si el ListView está inicializado y visible
        if (listaJuegos.IsHandleCreated && listaJuegos.Columns.Count >= 5 && listaJuegos.Visible)
        {
            try
            {
                var anchoTotal = listaJuegos.ClientSize.Width;
                var anchoUsado = 0;
                
                // Sumar anchos de las primeras 4 columnas
                for (int i = 0; i < 4; i++)
                {
                    anchoUsado += listaJuegos.Columns[i].Width;
                }
                
                // Asignar el espacio restante a la última columna (Preview)
                var anchoRestante = Math.Max(100, anchoTotal - anchoUsado - 20); // 20px de margen
                listaJuegos.Columns[4].Width = anchoRestante;
            }
            catch
            {
                // Ignorar errores durante la inicialización
            }
        }
    }

    private void AplicarColoreoAvanzado(ListViewItem item, Juego juego, HashSet<string> nombresDuplicados)
    {
        // 🔴 Duplicados (Rojo) - Prioridad más alta
        if (nombresDuplicados.Contains(juego.Nombre))
        {
            item.ForeColor = Color.Red;
            item.BackColor = Color.LightCoral;
            return;
        }

        // 🔵 Desconocidos (Azul) - Segunda prioridad
        if (juego.EsDesconocido || string.IsNullOrWhiteSpace(juego.Genero))
        {
            item.ForeColor = Color.Blue;
            item.BackColor = Color.LightBlue;
            return;
        }

        // ⚪ Missing Entries (Amarillo) - Tercera prioridad
        // Se marcan como missing si no tienen datos básicos válidos
        if (string.IsNullOrWhiteSpace(juego.Nombre) || string.IsNullOrWhiteSpace(juego.Slave))
        {
            item.ForeColor = Color.Orange;
            item.BackColor = Color.LightYellow;
            return;
        }

        // 🟢 Normal (Sin coloreo)
        // Juegos válidos y únicos mantienen el color por defecto
        item.ForeColor = Color.Empty;
        item.BackColor = Color.Empty;
    }

    private void ActualizarEstadoYTitulo()
    {
        var total = _juegos.Count;
        var mostrados = listaJuegos.Items.Count;
        Text = $"IGame Tool {Version} (Showing {mostrados} of {total} Games)";
    }

    private void ActualizarBotones()
    {
        var hayLista = _juegos.Count > 0;
        btnArreglarLista.Enabled = hayLista;
        btnGuardarCsv.Enabled = hayLista;
        btnLimpiarLista.Enabled = hayLista;
        btnEtiquetaRapida.Enabled = listaJuegos.Items.Count > 0;
        
        // Habilitar checkboxes y controles relacionados cuando hay juegos
        chkNombresCortos.Enabled = hayLista;
        chkMantenerDatos.Enabled = hayLista;
        chkVerDuplicados.Enabled = hayLista;
        chkVerDesconocidos.Enabled = hayLista;
        cmbTitleCase.Enabled = hayLista;

        // Habilitar búsqueda cuando hay juegos
        if (txtBusquedaControl != null)
        {
            txtBusquedaControl.Enabled = hayLista;
        }

        // Actualizar botón Deshacer con el nuevo gestor
        btnDeshacer.Enabled = _servicioUndo.CanUndo;
        btnDeshacer.Text = _servicioUndo.CanUndo 
            ? $"Deshacer: {_servicioUndo.UndoDescription}" 
            : "Deshacer";
    }

    private IEnumerable<Juego> ObtenerJuegosParaMostrar(HashSet<string> nombresDuplicados)
    {
        IEnumerable<Juego> query = _juegos;

        if (chkVerDuplicados.Checked)
        {
            query = query.Where(j => nombresDuplicados.Contains(j.Nombre));
        }

        if (chkVerDesconocidos.Checked)
        {
            query = query.Where(j => j.EsDesconocido);
        }

        if (!string.IsNullOrWhiteSpace(_textoBusqueda))
        {
            var busqueda = _textoBusqueda.Trim();
            query = query.Where(j =>
                j.Nombre.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ||
                j.NombreCorto.Contains(busqueda, StringComparison.OrdinalIgnoreCase) ||
                j.Genero.Contains(busqueda, StringComparison.OrdinalIgnoreCase));
        }

        // Aplicar ordenación
        query = _columnaOrden switch
        {
            0 => _ordenAscendente ? query.OrderBy(j => j.Nombre, StringComparer.CurrentCultureIgnoreCase) : query.OrderByDescending(j => j.Nombre, StringComparer.CurrentCultureIgnoreCase),
            1 => _ordenAscendente ? query.OrderBy(j => j.Genero, StringComparer.CurrentCultureIgnoreCase) : query.OrderByDescending(j => j.Genero, StringComparer.CurrentCultureIgnoreCase),
            2 => _ordenAscendente ? query.OrderBy(j => j.Slave, StringComparer.CurrentCultureIgnoreCase) : query.OrderByDescending(j => j.Slave, StringComparer.CurrentCultureIgnoreCase),
            3 => _ordenAscendente ? query.OrderBy(j => j.Path, StringComparer.CurrentCultureIgnoreCase) : query.OrderByDescending(j => j.Path, StringComparer.CurrentCultureIgnoreCase),
            _ => query
        };

        return query;
    }

    private HashSet<string> CalcularNombresDuplicados(List<Juego> juegos)
    {
        return juegos
            .GroupBy(j => j.Nombre, StringComparer.CurrentCultureIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet(StringComparer.CurrentCultureIgnoreCase);
    }

    private string AplicarTitleCase(string texto)
    {
        return cmbTitleCase.SelectedIndex switch
        {
            1 => texto.ToLowerInvariant(),
            2 => texto.ToUpperInvariant(),
            _ => texto
        };
    }

    private void chkNombresCortos_CheckedChanged(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        DibujarLista();
    }

    private void cmbTitleCase_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        DibujarLista();
    }

    private void chkFiltros_CheckedChanged(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        DibujarLista();
    }

    private void btnLimpiarLista_Click(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        if (MessageBox.Show(this, "¿Limpiar todos los datos?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
        {
            return;
        }

        var comando = new Servicios.LimpiarListaCommand(_juegos);
        _servicioUndo.ExecuteCommand(comando);

        _rutaCsvActual = string.Empty;
        listaJuegos.Items.Clear();

        chkNombresCortos.Checked = false;
        chkVerDuplicados.Checked = false;
        chkVerDesconocidos.Checked = false;
        cmbTitleCase.SelectedIndex = 0;

        chkNombresCortos.Enabled = false;
        chkMantenerDatos.Enabled = false;
        chkVerDuplicados.Enabled = false;
        chkVerDesconocidos.Enabled = false;
        cmbTitleCase.Enabled = false;

        btnArreglarLista.Enabled = false;
        btnGuardarCsv.Enabled = false;
        btnEtiquetaRapida.Enabled = false;
        btnLimpiarLista.Enabled = false;

        ActualizarEstadoYTitulo();
        ActualizarBotones();

        ActualizarEstadisticas();
    }

    private void btnDeshacer_Click(object sender, EventArgs e)
    {
        if (_servicioUndo.CanUndo)
        {
            _servicioUndo.Undo();
            DibujarLista();
            ActualizarBotones();
            
            // Ocultar estadísticas Fix List al deshacer cambios
            panelEstadisticasFix.Limpiar();
            
            // Actualizar estadísticas normales
            ActualizarEstadisticas();
        }
    }

    private void FormPrincipal_KeyDown(object? sender, KeyEventArgs e)
    {
        // F1 = Ayuda
        if (e.KeyCode == Keys.F1)
        {
            btnAyuda_Click(btnAyuda, EventArgs.Empty);
            e.Handled = true;
            return;
        }

        // Ctrl+Z = Undo, Ctrl+Y = Redo
        if (e.Control)
        {
            if (e.KeyCode == Keys.Z && _servicioUndo.CanUndo)
            {
                _servicioUndo.Undo();
                DibujarLista();
                ActualizarBotones();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Y && _servicioUndo.CanRedo)
            {
                _servicioUndo.Redo();
                DibujarLista();
                ActualizarBotones();
                e.Handled = true;
            }
        }
    }

    private async void btnArreglarLista_Click(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        try
        {
            Cursor.Current = Cursors.WaitCursor;

            var tiempoInicio = DateTime.Now;
            var juegosEntrada = _juegos.Select(j => _servicioJuegos.CopiarJuego(j)).ToList();

            var directorioTrabajo = ObtenerDirectorioTrabajo();
            Directory.CreateDirectory(directorioTrabajo);

            // Mostrar progreso en la interfaz principal
            MostrarProgresoPrincipal("Probando conexión FTP...", indeterminado: true);

            // Probar conexión FTP primero
            var conexionOk = await _servicioFixList.ProbarConexionFtpAsync(new Progress<Servicios.EstadoFix>(estado =>
            {
                ActualizarProgresoPrincipal(0, $"{estado.Titulo}: {estado.Detalle}");
            }));
            
            if (!conexionOk)
            {
                OcultarProgresoPrincipal();
                ActualizarProgresoPrincipal(0, "Error de conexión FTP. Verifique su conexión a internet o firewall.");
                await Task.Delay(3000);
                OcultarProgresoPrincipal();
                return;
            }

            // Usar el servicio FixList con progreso
            var juegosSalida = await _servicioFixList.EjecutarFixListAsync(directorioTrabajo, juegosEntrada, new Progress<Servicios.EstadoFix>(estado =>
            {
                // Extraer porcentaje del detalle si contiene información de progreso
                var porcentaje = 0;
                if (estado.Detalle.Contains("/"))
                {
                    var partes = estado.Detalle.Split('/');
                    if (partes.Length == 2 && int.TryParse(partes[0].Trim(), out var actual) && int.TryParse(partes[1].Trim(), out var total))
                    {
                        porcentaje = (int)((double)actual / total * 100);
                    }
                }
                
                ActualizarProgresoPrincipal(porcentaje, $"{estado.Titulo}: {estado.Detalle}");
            }));

            var duracion = DateTime.Now - tiempoInicio;

            // Crear y ejecutar comando para deshacer
            var comando = new Servicios.FixListCommand(_juegos, juegosSalida);
            _servicioUndo.ExecuteCommand(comando);

            DibujarLista();
            
            // ACTUALIZAR AMBOS PANELS DE ESTADÍSTICAS
            ActualizarEstadisticas();
            
            // Actualizar panel de estadísticas Fix List
            panelEstadisticasFix.ActualizarEstadisticas(juegosEntrada, juegosSalida, directorioTrabajo, duracion);

            ActualizarProgresoPrincipal(100, "Fix List completado exitosamente.");
            await Task.Delay(1000);
            OcultarProgresoPrincipal();
        }
        catch (Exception ex)
        {
            // Generar archivo de debug del error
            try
            {
                var directorioTrabajo = ObtenerDirectorioTrabajo();
                var debugPath = Path.Combine(directorioTrabajo, "error_fixlist.txt");
                
                using var writer = new StreamWriter(debugPath, false, Encoding.UTF8);
                writer.WriteLine($"=== ERROR EN FIX LIST - {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
                writer.WriteLine($"Error: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                writer.WriteLine();
                writer.WriteLine($"Estado actual:");
                writer.WriteLine($"- Juegos cargados: {_juegos.Count}");
                writer.WriteLine($"- Directorio trabajo: {directorioTrabajo}");
                
                ActualizarProgresoPrincipal(0, $"Error: {ex.Message}");
                await Task.Delay(2000);
                OcultarProgresoPrincipal();
                
                MessageBox.Show(this, $"Error: {ex.Message}\n\nSe ha creado un archivo de debug en:\n{debugPath}", 
                               "Error en Fix List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                ActualizarProgresoPrincipal(0, $"Error: {ex.Message}");
                await Task.Delay(2000);
                OcultarProgresoPrincipal();
                
                MessageBox.Show(this, ex.Message, "Error en Fix List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    private List<Juego> EjecutarFixList(string directorioTrabajo, List<Juego> juegosEntrada, IProgress<EstadoFix> progreso)
    {
        progreso.Report(new EstadoFix("Checking database...", ""));
        var rutaDb = DescargarBaseDatosSiHaceFalta(directorioTrabajo, progreso);

        progreso.Report(new EstadoFix("Checking database...", "Checking genres..."));
        DescargarArchivoGenresSiHaceFalta(directorioTrabajo, progreso);

        progreso.Report(new EstadoFix("Loading database...", Path.GetFileName(rutaDb) ?? string.Empty));
        var mapa = CargarMapaBaseDatos(rutaDb);

        progreso.Report(new EstadoFix("Fixing list...", $"0 / {juegosEntrada.Count}"));
        var salida = new List<Juego>(juegosEntrada.Count);

        for (var i = 0; i < juegosEntrada.Count; i++)
        {
            var juego = juegosEntrada[i];
            var key = juego.Slave.ToLowerInvariant();

            if (mapa.TryGetValue(key, out var comp))
            {
                salida.Add(new Juego
                {
                    Nombre = comp.Nombre,
                    NombreCorto = comp.NombreCorto,
                    Genero = comp.Genero,
                    Ruta = string.IsNullOrEmpty(juego.Path) ? "" : juego.Path + "/",
                    Path = juego.Path,
                    Slave = juego.Slave,
                    EsDesconocido = false,
                    Dato1 = juego.Dato1,
                    Dato2 = juego.Dato2,
                    Dato3 = juego.Dato3,
                    Dato4 = juego.Dato4
                });
            }
            else
            {
                salida.Add(new Juego
                {
                    Nombre = juego.Nombre,
                    NombreCorto = string.IsNullOrWhiteSpace(juego.Nombre) ? string.Empty : juego.Nombre,
                    Genero = "Unknown",
                    Ruta = string.IsNullOrEmpty(juego.Path) ? "" : juego.Path + "/",
                    Path = juego.Path,
                    Slave = juego.Slave,
                    EsDesconocido = true,
                    Dato1 = juego.Dato1,
                    Dato2 = juego.Dato2,
                    Dato3 = juego.Dato3,
                    Dato4 = juego.Dato4
                });
            }

            if (i == 0 || (i + 1) % 50 == 0 || i + 1 == juegosEntrada.Count)
            {
                progreso.Report(new EstadoFix("Fixing list...", $"{i + 1} / {juegosEntrada.Count}"));
            }
        }

        salida.Sort((a, b) => StringComparer.CurrentCultureIgnoreCase.Compare(a.Nombre, b.Nombre));
        progreso.Report(new EstadoFix("Fix List", "Done."));
        return salida;
    }

    private async void btnGuardarCsv_Click(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        var rutaDestino = ObtenerRutaDestinoGuardado();
        if (string.IsNullOrWhiteSpace(rutaDestino))
        {
            return;
        }

        // Mostrar progreso en la interfaz principal
        MostrarProgresoPrincipal("Iniciando guardado...", indeterminado: false);
        
        var task = Task.Run(() =>
        {
            try
            {
                // Backup automático ANTES de sobrescribir (como el original)
                if (File.Exists(rutaDestino))
                {
                    var directorio = Path.GetDirectoryName(rutaDestino);
                    var nombre = Path.GetFileNameWithoutExtension(rutaDestino);
                    var extension = Path.GetExtension(rutaDestino);
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var rutaBackup = Path.Combine(directorio ?? "", $"{nombre}_{timestamp}{extension}");
                    File.Copy(rutaDestino, rutaBackup, overwrite: true);
                }

                using var escritor = new StreamWriter(rutaDestino, false, Encoding.ASCII);
                var totalJuegos = _juegos.Count;
                var procesados = 0;

                foreach (var juego in _juegos)
                {
                    // Actualizar progreso
                    var porcentaje = (int)((double)procesados / totalJuegos * 100);
                    this.Invoke(new Action(() =>
                    {
                        ActualizarProgresoPrincipal(porcentaje, $"Guardando juego {procesados + 1} de {totalJuegos}...");
                    }));

                    var nombre = chkNombresCortos.Checked && !string.IsNullOrWhiteSpace(juego.NombreCorto)
                        ? juego.NombreCorto
                        : juego.Nombre;

                    var genero = juego.Genero;
                    var prefijo = $"0;{nombre};{genero};";

                    if (cmbTitleCase.SelectedIndex == 1)
                    {
                        prefijo = prefijo.ToLowerInvariant();
                    }
                    else if (cmbTitleCase.SelectedIndex == 2)
                    {
                        prefijo = prefijo.ToUpperInvariant();
                    }

                    var ruta = juego.Path + juego.Slave;

                    var sb = new StringBuilder();
                    sb.Append(prefijo);
                    sb.Append(ruta);
                    sb.Append(';');

                    if (!chkMantenerDatos.Checked)
                    {
                        sb.Append("0;0;0;0");
                    }
                    else
                    {
                        sb.Append(juego.Dato1);
                        sb.Append(';');
                        sb.Append(juego.Dato2);
                        sb.Append(';');
                        sb.Append(juego.Dato3);
                        sb.Append(';');
                        sb.Append(juego.Dato4);
                    }

                    escritor.WriteLine(sb.ToString());
                    procesados++;
                }

                // Actualizar UI en el hilo principal
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(this, "CSV guardado.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OcultarProgresoPrincipal();
                }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(this, ex.Message, "Error al guardar CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    OcultarProgresoPrincipal();
                }));
            }
        });

        // Esperar a que termine el guardado de forma asíncrona
        try
        {
            await task; // Esperar sin bloquear el UI thread
        }
        catch (Exception ex)
        {
            // Asegurar que el progreso se oculte incluso si hay error
            this.Invoke(new Action(() =>
            {
                OcultarProgresoPrincipal();
            }));
        }
    }

    private void btnEtiquetaRapida_Click(object sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            return;
        }

        if (listaJuegos.SelectedItems.Count == 0)
        {
            MessageBox.Show(this, "Selecciona uno o varios juegos en la lista.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var tag = Interaction.InputBox("Enter a new tag", "Add Tag", "");
        if (string.IsNullOrWhiteSpace(tag))
        {
            return;
        }

        // Obtener índices de los juegos seleccionados
        var indices = new List<int>();
        foreach (ListViewItem item in listaJuegos.SelectedItems)
        {
            if (item.Tag is Juego juego)
            {
                var indice = _juegos.IndexOf(juego);
                if (indice >= 0)
                {
                    indices.Add(indice);
                }
            }
        }

        if (indices.Count == 0)
        {
            return;
        }

        // Crear y ejecutar comando
        var comando = new Servicios.QuickTagCommand(_juegos, indices, $" ({tag.Trim()})");
        _servicioUndo.ExecuteCommand(comando);

        _juegos.Sort((a, b) => StringComparer.CurrentCultureIgnoreCase.Compare(a.Nombre, b.Nombre));
        DibujarLista();
        ActualizarBotones();
    }

    private void btnAyuda_Click(object sender, EventArgs e)
    {
        using var form = new FormAyuda();
        form.ShowDialog(this);
    }

    private void CargarCsvSilencioso(string rutaCsv)
    {
        try
        {
            // Usar el servicio CSV de forma síncrona (sin mostrar mensajes)
            var (juegos, validacion) = _servicioCsv.CargarCsv(rutaCsv);
            
            if (!validacion.IsValid)
            {
                // En modo silencioso, si hay errores de validación, no cargar
                return;
            }

            // En modo silencioso, ignorar advertencias y continuar

            // Actualizar datos
            _juegos.Clear();
            _juegos.AddRange(juegos);
            _rutaCsvActual = rutaCsv;

            // Guardar en settings
            _settings.LastCsvFile = rutaCsv;
            _settings.AddRecentFile(rutaCsv);
            _settings.Save();

            // Actualizar interfaz
            DibujarLista();
            ActualizarEstadoYTitulo();
            ActualizarBotones();
        }
        catch (Exception ex)
        {
            // En modo silencioso, ignorar errores y continuar sin cargar
            // Podríamos loggear el error si quisiéramos
        }
    }

    private void ValidarVersionIGame(string rutaCsv, List<Juego> juegos)
    {
        // Validar que el archivo sea un CSV de IGame válido
        var nombreArchivo = Path.GetFileName(rutaCsv) ?? "";
        
        // 1. Verificar extensión .csv
        if (!nombreArchivo.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            var resultado = MessageBox.Show(
                "⚠️ **ADVERTENCIA: Versión Antigua de IGame Detectada**\n\n" +
                "El archivo no tiene extensión .csv\n" +
                "Esto indica que estás usando una versión antigua de IGame.\n\n" +
                "**Recomendación:**\n" +
                "• Actualiza IGame a la última versión en:\n" +
                "  https://github.com/MrZammler/iGame/releases\n" +
                "• Vuelve a escanear tus repositorios\n" +
                "• Usa el nuevo archivo gameslist.csv generado\n\n" +
                "¿Deseas continuar de todos modos?",
                "Versión IGame No Compatible",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
                
            if (resultado != DialogResult.Yes)
            {
                throw new OperationCanceledException("Operación cancelada por el usuario");
            }
            return;
        }

        // 2. Verificar nombre del archivo (debe ser gameslist.csv)
        if (!nombreArchivo.Equals("gameslist.csv", StringComparison.OrdinalIgnoreCase))
        {
            var resultado = MessageBox.Show(
                "⚠️ **ADVERTENCIA: Nombre de Archivo No Estándar**\n\n" +
                $"El archivo '{nombreArchivo}' no es el nombre estándar.\n" +
                "IGame moderno genera archivos llamados 'gameslist.csv'.\n\n" +
                "**Posibles causas:**\n" +
                "• Versión antigua de IGame\n" +
                "• Archivo renombrado manualmente\n" +
                "• Copia de seguridad\n\n" +
                "**Recomendación:**\n" +
                "• Verifica que estás usando el archivo correcto\n" +
                "• Considera actualizar IGame si es una versión antigua\n\n" +
                "¿Deseas continuar?",
                "Nombre de Archivo No Estándar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
                
            if (resultado != DialogResult.Yes)
            {
                throw new OperationCanceledException("Operación cancelada por el usuario");
            }
        }

        // 3. Verificar estructura del CSV (debe tener campos válidos)
        var juegosInvalidos = juegos.Where(j => 
            string.IsNullOrWhiteSpace(j.Nombre) || 
            string.IsNullOrWhiteSpace(j.Slave)).ToList();
            
        if (juegosInvalidos.Count > 0 && juegosInvalidos.Count == juegos.Count)
        {
            var resultado = MessageBox.Show(
                "❌ **ERROR: Estructura CSV No Válida**\n\n" +
                "El archivo CSV no tiene la estructura esperada de IGame.\n" +
                "Todos los juegos tienen campos obligatorios vacíos.\n\n" +
                "**Verificaciones:**\n" +
                $"• Archivo: {nombreArchivo}\n" +
                $"• Juegos inválidos: {juegosInvalidos.Count} de {juegos.Count}\n" +
                $"• Campos vacíos: Nombre o Slave\n\n" +
                "**Causas posibles:**\n" +
                "• Formato de archivo incorrecto\n" +
                "• Versión muy antigua de IGame\n" +
                "• Archivo dañado o corrupto\n\n" +
                "**Recomendación:**\n" +
                "• Actualiza IGame a la última versión\n" +
                "• Genera un nuevo gameslist.csv\n" +
                "• Verifica que el archivo no esté dañado\n\n" +
                "¿Deseas continuar de todos modos?",
                "Estructura CSV No Válida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);
                
            if (resultado != DialogResult.Yes)
            {
                throw new OperationCanceledException("Operación cancelada por el usuario");
            }
        }

        // 4. Verificar si hay demasiados "Unknown" (posible versión antigua)
        var desconocidos = juegos.Count(j => j.EsDesconocido || string.IsNullOrWhiteSpace(j.Genero));
        var porcentajeDesconocidos = (desconocidos * 100.0) / juegos.Count;
        
        if (porcentajeDesconocidos > 50)
        {
            var resultado = MessageBox.Show(
                "⚠️ **ADVERTENCIA: Alta Tasa de Juegos Desconocidos**\n\n" +
                $"Se detectaron {desconocidos} juegos desconocidos ({porcentajeDesconocidos:F1}%).\n" +
                "Esto puede indicar:\n\n" +
                "• Versión antigua de IGame sin base de datos actualizada\n" +
                "• Base de datos IG_Data.dat desactualizada\n" +
                "• Archivo de una versión muy antigua\n\n" +
                "**Recomendaciones:**\n" +
                "• Actualiza IGame a la última versión\n" +
                "• Asegúrate de tener la base de datos más reciente\n" +
                "• Considera ejecutar 'Fix List' para actualizar géneros\n\n" +
                "¿Deseas continuar?",
                "Alta Tasa de Desconocidos",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
                
            if (resultado != DialogResult.Yes)
            {
                throw new OperationCanceledException("Operación cancelada por el usuario");
            }
        }

        // 5. Éxito - Validación pasada
        if (juegosInvalidos.Count == 0 && porcentajeDesconocidos <= 50)
        {
            // Solo mostrar mensaje si todo está bien y es la primera carga
            if (_settings.LastCsvFile == rutaCsv)
            {
                MessageBox.Show(
                    "✅ **Validación IGame Exitosa**\n\n" +
                    "El archivo CSV es compatible con IGame moderno.\n\n" +
                    $"• Archivo: {nombreArchivo}\n" +
                    $"• Juegos válidos: {juegos.Count - juegosInvalidos.Count}\n" +
                    $"• Tasa desconocidos: {porcentajeDesconocidos:F1}%\n\n" +
                    "¡Todo listo para procesar!",
                    "Validación Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }

    private void listaJuegos_DoubleClick(object sender, EventArgs e)
    {
        if (listaJuegos.SelectedItems.Count != 1)
        {
            return;
        }

        if (listaJuegos.SelectedItems[0].Tag is not Juego juego)
        {
            return;
        }

        var generos = ObtenerGenerosParaEditor();

        using var editor = new FormEditarJuego(
            juego.Nombre,
            juego.NombreCorto,
            juego.Slave,
            juego.Genero,
            generos);

        if (editor.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        GuardarUndo();

        var indice = _juegos.IndexOf(juego);
        if (indice < 0)
        {
            return;
        }

        var nombreNuevo = editor.NombreEditado;
        var generoNuevo = editor.GeneroEditado;
        var slaveNuevo = editor.SlaveEditado;
        var nombreCortoNuevo = string.IsNullOrWhiteSpace(editor.NombreCortoEditado)
            ? GenerarNombreCorto(nombreNuevo)
            : editor.NombreCortoEditado;

        _juegos[indice] = new Juego
        {
            Nombre = nombreNuevo,
            Genero = generoNuevo,
            Ruta = string.IsNullOrEmpty(juego.Path) ? "" : juego.Path + "/",
            Path = juego.Path,
            Slave = slaveNuevo,
            NombreCorto = nombreCortoNuevo,
            EsDesconocido = juego.EsDesconocido,
            Dato1 = juego.Dato1,
            Dato2 = juego.Dato2,
            Dato3 = juego.Dato3,
            Dato4 = juego.Dato4
        };

        _juegos.Sort((a, b) => StringComparer.CurrentCultureIgnoreCase.Compare(a.Nombre, b.Nombre));
        DibujarLista();
    }

    private static Juego CopiarJuego(Juego j)
    {
        return new Juego
        {
            Nombre = j.Nombre,
            Genero = j.Genero,
            Ruta = j.Path + j.Slave,
            Path = j.Path,
            Slave = j.Slave,
            NombreCorto = j.NombreCorto,
            EsDesconocido = j.EsDesconocido,
            Dato1 = j.Dato1,
            Dato2 = j.Dato2,
            Dato3 = j.Dato3,
            Dato4 = j.Dato4
        };
    }

    private void GuardarUndo()
    {
        // El sistema de undo ahora se maneja a través de comandos
        // Este método ya no es necesario con el nuevo sistema
    }

    private List<string> ObtenerGenerosParaEditor()
    {
        var generos = new List<string>();

        var rutaGeneros = ObtenerRutaArchivoGeneros();
        if (!string.IsNullOrWhiteSpace(rutaGeneros) && File.Exists(rutaGeneros))
        {
            try
            {
                generos.AddRange(File.ReadAllLines(rutaGeneros, Encoding.UTF8)
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrWhiteSpace(l)));
            }
            catch
            {
            }
        }

        if (generos.Count == 0)
        {
            generos.AddRange(_juegos
                .Select(j => j.Genero)
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(g => g, StringComparer.CurrentCultureIgnoreCase));
        }

        return generos;
    }

    private string ObtenerDirectorioTrabajo()
    {
        if (!string.IsNullOrWhiteSpace(_rutaCsvActual))
        {
            var dir = Path.GetDirectoryName(_rutaCsvActual);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                return dir;
            }
        }

        var baseDir = AppContext.BaseDirectory;
        return Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
    }

    private string DescargarBaseDatosSiHaceFalta(string directorioTrabajo, IProgress<EstadoFix>? progreso = null)
    {
        var dbLocal = Directory
            .EnumerateFiles(directorioTrabajo, "IG_Data*", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .ToList();

        var dbActualLocal = dbLocal.Count > 0 ? dbLocal[0]! : string.Empty;

        progreso?.Report(new EstadoFix("Checking database...", "Connecting to FTP..."));
        var remoto = ObtenerNombreBaseDatosRemota(progreso);
        if (string.IsNullOrWhiteSpace(remoto))
        {
            if (string.IsNullOrWhiteSpace(dbActualLocal))
            {
                throw new InvalidOperationException("No se pudo localizar la base de datos IG_Data (ni local ni remota).");
            }

            return Path.Combine(directorioTrabajo, dbActualLocal);
        }

        if (!string.IsNullOrWhiteSpace(dbActualLocal) && string.Equals(dbActualLocal, remoto, StringComparison.OrdinalIgnoreCase))
        {
            progreso?.Report(new EstadoFix("Checking database...", "Data file up to date."));
            Thread.Sleep(500);
            return Path.Combine(directorioTrabajo, dbActualLocal);
        }

        if (!string.IsNullOrWhiteSpace(dbActualLocal))
        {
            var rutaVieja = Path.Combine(directorioTrabajo, dbActualLocal);
            if (File.Exists(rutaVieja))
            {
                File.Delete(rutaVieja);
            }
        }

        var rutaNueva = Path.Combine(directorioTrabajo, remoto);
        progreso?.Report(new EstadoFix("Checking database...", "Downloading data file."));
        DescargarFtp(remoto, rutaNueva, progreso);
        return rutaNueva;
    }

    private void DescargarArchivoGenresSiHaceFalta(string directorioTrabajo, IProgress<EstadoFix>? progreso = null)
    {
        var ruta = Path.Combine(directorioTrabajo, "genres");
        if (File.Exists(ruta))
        {
            return;
        }

        // Intentar descargar genres, pero si no existe, crearlo desde IG_Data.dat
        try
        {
            progreso?.Report(new EstadoFix("Checking database...", "Downloading genres..."));
            DescargarFtp("genres", ruta, progreso);
        }
        catch
        {
            // Si no se puede descargar genres, crearlo desde IG_Data.dat
            var rutaIGData = Path.Combine(directorioTrabajo, "IG_Data.dat");
            if (File.Exists(rutaIGData))
            {
                CrearArchivoGenresDesdeIGData(rutaIGData, ruta);
            }
        }
    }

    private void CrearArchivoGenresDesdeIGData(string rutaIGData, string rutaGenres)
    {
        try
        {
            var generos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var linea in File.ReadLines(rutaIGData, Encoding.ASCII))
            {
                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                var campos = ParsearLineaCsv(linea, ';');
                if (campos.Count >= 3)
                {
                    var genero = ObtenerCampo(campos, 2);
                    if (!string.IsNullOrWhiteSpace(genero))
                    {
                        generos.Add(genero.Trim());
                    }
                }
            }

            // Guardar géneros únicos ordenados
            using var writer = new StreamWriter(rutaGenres, false, Encoding.UTF8);
            foreach (var genero in generos.OrderBy(g => g, StringComparer.CurrentCultureIgnoreCase))
            {
                writer.WriteLine(genero);
            }
        }
        catch
        {
            // Si no se puede crear el archivo genres, continuar sin él
            // No es crítico para el funcionamiento de Fix List
        }
    }

    private string? ObtenerNombreBaseDatosRemota(IProgress<EstadoFix>? progreso = null)
    {
        try
        {
            var archivos = ListarFtp("", progreso);
            progreso?.Report(new EstadoFix("Checking database...", "Connected to FTP"));
            
            // Debug: guardar lista de archivos encontrados
            var debugPath = Path.Combine(ObtenerDirectorioTrabajo(), "debug_ftp_files.txt");
            using var writer = new StreamWriter(debugPath, false, Encoding.UTF8);
            writer.WriteLine($"=== ARCHIVOS FTP ENCONTRADOS - {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
            writer.WriteLine($"Total archivos: {archivos.Count}");
            writer.WriteLine();
            
            foreach (var archivo in archivos)
            {
                writer.WriteLine(archivo);
            }
            
            writer.WriteLine();
            writer.WriteLine("=== BÚSQUEDA DE IG_Data ===");
            
            // Buscar cualquier archivo que contenga "IG_Data" en el nombre
            var archivoDb = archivos.FirstOrDefault(a => a.Contains("IG_Data", StringComparison.OrdinalIgnoreCase));
            
            if (string.IsNullOrWhiteSpace(archivoDb))
            {
                writer.WriteLine("No se encontró ningún archivo que contenga 'IG_Data'");
            }
            else
            {
                writer.WriteLine($"Archivo encontrado: {archivoDb}");
            }
            
            return archivoDb;
        }
        catch (Exception ex)
        {
            var debugPath = Path.Combine(ObtenerDirectorioTrabajo(), "debug_ftp_error.txt");
            using var writer = new StreamWriter(debugPath, false, Encoding.UTF8);
            writer.WriteLine($"ERROR AL LISTAR FTP: {ex.Message}");
            writer.WriteLine($"StackTrace: {ex.StackTrace}");
            return null;
        }
    }

    private List<string> ListarFtp(string rutaRelativa, IProgress<EstadoFix>? progreso = null)
    {
        var request = CrearRequestFtp(rutaRelativa);
        request.Method = WebRequestMethods.Ftp.ListDirectory;

        using var response = (FtpWebResponse)request.GetResponse();
        using var stream = response.GetResponseStream();
        if (stream == null)
        {
            return new List<string>();
        }

        using var reader = new StreamReader(stream, Encoding.ASCII);
        var contenido = reader.ReadToEnd();

        return contenido
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();
    }

    private void DescargarFtp(string rutaRelativa, string rutaDestino, IProgress<EstadoFix>? progreso = null)
    {
        var request = CrearRequestFtp(rutaRelativa);
        request.Method = WebRequestMethods.Ftp.DownloadFile;

        using var response = (FtpWebResponse)request.GetResponse();
        using var stream = response.GetResponseStream();
        if (stream == null)
        {
            throw new InvalidOperationException("No se pudo descargar el archivo desde FTP.");
        }

        using var salida = File.Create(rutaDestino);
        stream.CopyTo(salida);
    }

    private FtpWebRequest CrearRequestFtp(string rutaRelativa)
    {
        var baseUri = new Uri("ftp://ftp.grandis.nu/~Uploads/mrv2k/");
        
        // Si la ruta relativa está vacía, usamos la URI base directamente
        Uri uri;
        if (string.IsNullOrWhiteSpace(rutaRelativa))
        {
            uri = baseUri;
        }
        else
        {
            var relativa = rutaRelativa.TrimStart('/');
            uri = new Uri(baseUri, relativa);
        }

        var request = (FtpWebRequest)FtpWebRequest.Create(uri);
        request.Credentials = new NetworkCredential("ftp", "amiga");
        request.UsePassive = true;
        request.UseBinary = true;
        request.KeepAlive = false;
        return request;
    }

    private Dictionary<string, CompData> CargarMapaBaseDatos(string rutaDb)
    {
        var mapa = new Dictionary<string, CompData>(StringComparer.OrdinalIgnoreCase);
        var numeroLinea = 0;
        var debugPath = Path.Combine(Path.GetDirectoryName(rutaDb) ?? "", "debug_csv.txt");

        try
        {
            using var debugWriter = new StreamWriter(debugPath, false, Encoding.UTF8);
            debugWriter.WriteLine($"=== DEBUG CSV - {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
            debugWriter.WriteLine($"Archivo: {rutaDb}");
            debugWriter.WriteLine();

            foreach (var linea in File.ReadLines(rutaDb, Encoding.ASCII))
            {
                numeroLinea++;
                
                if (string.IsNullOrWhiteSpace(linea))
                {
                    debugWriter.WriteLine($"Línea {numeroLinea}: [VACÍA]");
                    continue;
                }

                try
                {
                    var campos = ParsearLineaCsv(linea, ';');
                    
                    // Depuración: guardar información de la línea en archivo
                    if (numeroLinea <= 10 || numeroLinea % 100 == 0 || campos.Count < 5)
                    {
                        debugWriter.WriteLine($"Línea {numeroLinea}: {campos.Count} campos");
                        debugWriter.WriteLine($"Original: {linea}");
                        debugWriter.WriteLine($"Campos: [{string.Join(" | ", campos)}]");
                        debugWriter.WriteLine();
                    }
                    
                    // IG_Data.dat tiene 6 campos: Slave;Nombre;Género;NombreCompleto;NombreCorto;Tipo
                    if (campos.Count < 5)
                    {
                        debugWriter.WriteLine($"ERROR: Línea {numeroLinea} - Solo tiene {campos.Count} campos (mínimo 5 requeridos)");
                        continue;
                    }

                    var slave = (ObtenerCampo(campos, 0) ?? string.Empty).ToLowerInvariant();
                    var nombre = ObtenerCampo(campos, 1) ?? string.Empty;
                    var genero = ObtenerCampo(campos, 2) ?? string.Empty;
                    var nombreCompleto = ObtenerCampo(campos, 3) ?? string.Empty;
                    var nombreCorto = ObtenerCampo(campos, 4) ?? string.Empty;
                    var folder = string.Empty; // IG_Data.dat no tiene folder

                    if (!string.IsNullOrWhiteSpace(slave))
                    {
                        mapa[slave] = new CompData
                        {
                            Nombre = nombreCompleto, // Usar el nombre completo
                            NombreCorto = nombreCorto,
                            Genero = genero,
                            Folder = folder
                        };
                    }
                }
                catch (Exception ex)
                {
                    debugWriter.WriteLine($"ERROR PROCESANDO LÍNEA {numeroLinea}:");
                    debugWriter.WriteLine($"Línea: {linea}");
                    debugWriter.WriteLine($"Error: {ex.Message}");
                    debugWriter.WriteLine($"StackTrace: {ex.StackTrace}");
                    debugWriter.WriteLine();
                }
            }

            debugWriter.WriteLine($"=== RESUMEN ===");
            debugWriter.WriteLine($"Total líneas procesadas: {numeroLinea}");
            debugWriter.WriteLine($"Total entradas en mapa: {mapa.Count}");
        }
        catch (Exception ex)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                    MessageBox.Show($"Error al crear archivo de debug: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
            }
            else
            {
                MessageBox.Show($"Error al crear archivo de debug: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        return mapa;
    }

    private static string ObtenerFolderDesdePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var normalizada = path.Replace('\\', '/').TrimEnd('/');

        var idx = normalizada.LastIndexOf('/');
        if (idx >= 0 && idx + 1 < normalizada.Length)
        {
            return normalizada[(idx + 1)..];
        }

        idx = normalizada.LastIndexOf(':');
        if (idx >= 0 && idx + 1 < normalizada.Length)
        {
            return normalizada[(idx + 1)..];
        }

        return normalizada;
    }

    private string? ObtenerRutaArchivoGeneros()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_rutaCsvActual))
            {
                var dir = Path.GetDirectoryName(_rutaCsvActual);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    var ruta = Path.Combine(dir, "genres");
                    return ruta;
                }
            }

            var baseDir = AppContext.BaseDirectory;
            return Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "genres"));
        }
        catch
        {
            return null;
        }
    }

    private string? ObtenerRutaDestinoGuardado()
    {
        if (!string.IsNullOrWhiteSpace(_rutaCsvActual) && File.Exists(_rutaCsvActual))
        {
            // Diálogo exacto como el original IGame Tool
            var respuesta = MessageBox.Show(
                this,
                "Overwrite Old Game List?\nSelect 'No' to create a new file.",
                "Warning",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            switch (respuesta)
            {
                case DialogResult.Yes:
                    // Sobrescribir el archivo existente
                    return AsegurarExtensionCsv(_rutaCsvActual);
                
                case DialogResult.No:
                    // Abrir diálogo para crear nuevo archivo
                    return MostrarDialogoNuevoArchivo();
                
                case DialogResult.Cancel:
                    // Cancelar operación
                    return null;
            }
        }

        // Si no hay archivo actual, mostrar diálogo directamente
        return MostrarDialogoNuevoArchivo();
    }

    private string? MostrarDialogoNuevoArchivo()
    {
        using var dialogo = new SaveFileDialog
        {
            Title = "New File",
            Filter = "CSV File (*.csv)|*.csv",
            OverwritePrompt = true,
            AddExtension = true,
            DefaultExt = "csv"
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return null;
        }

        return AsegurarExtensionCsv(dialogo.FileName);
    }

    private static string AsegurarExtensionCsv(string ruta)
    {
        if (string.Equals(Path.GetExtension(ruta), ".csv", StringComparison.OrdinalIgnoreCase))
        {
            return ruta;
        }

        return ruta + ".csv";
    }

    private static string ObtenerCampo(IReadOnlyList<string> campos, int indice)
    {
        return indice >= 0 && indice < campos.Count ? campos[indice] : string.Empty;
    }

    private static IEnumerable<List<string>> EnumerarFilasCsv(string rutaCsv)
    {
        using var lector = new StreamReader(rutaCsv, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        string? linea;
        while ((linea = lector.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(linea))
            {
                yield return new List<string>();
                continue;
            }

            yield return ParsearLineaCsv(linea, ';');
        }
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

    private static string GenerarNombreCorto(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return string.Empty;
        }

        const int max = 26;
        return nombre.Length <= max ? nombre : nombre.Substring(0, max);
    }

    private static string ObtenerUltimoSegmentoRuta(string ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta))
        {
            return string.Empty;
        }

        var normalizada = ruta.Replace('\\', '/');
        var indice = normalizada.LastIndexOf('/');
        return indice >= 0 && indice + 1 < normalizada.Length ? normalizada[(indice + 1)..] : normalizada;
    }

    private static string ObtenerDirectorioRuta(string ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta))
        {
            return string.Empty;
        }

        var normalizada = ruta.Replace('\\', '/');
        var indice = normalizada.LastIndexOf('/');
        return indice >= 0 ? normalizada[..(indice + 1)] : string.Empty;
    }

    private static string? ObtenerRutaCsvPorDefecto()
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;
            var ruta = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "csv", "gameslist.csv"));
            return ruta;
        }
        catch
        {
            return null;
        }
    }

    private void MenuItemSalir_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void MenuItemBusquedaAvanzada_Click(object? sender, EventArgs e)
    {
        if (_juegos.Count == 0)
        {
            MessageBox.Show(this, "No hay juegos cargados para buscar.", "Búsqueda Avanzada", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            using var formBusqueda = new FormBusquedaAvanzada(_juegos);
            var resultado = formBusqueda.ShowDialog(this);

            if (resultado == DialogResult.OK && formBusqueda.JuegoSeleccionado.HasValue)
            {
                // Seleccionar el juego encontrado en la lista principal
                SeleccionarJuegoEnLista(formBusqueda.JuegoSeleccionado.Value);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Error al abrir búsqueda avanzada: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #region Métodos de Progreso Principal

    private void MostrarProgresoPrincipal(string estado, bool indeterminado = false)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string, bool>(MostrarProgresoPrincipal), estado, indeterminado);
            return;
        }

        lblEstadoPrincipal.Text = estado;
        lblEstadoPrincipal.Visible = true;
        progressBarPrincipal.Visible = true;
        progressBarPrincipal.Style = indeterminado ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
        progressBarPrincipal.Value = 0;
    }

    private void ActualizarProgresoPrincipal(int porcentaje, string estado)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<int, string>(ActualizarProgresoPrincipal), porcentaje, estado);
            return;
        }

        lblEstadoPrincipal.Text = estado;
        progressBarPrincipal.Value = Math.Max(0, Math.Min(100, porcentaje));
    }

    private void OcultarProgresoPrincipal()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(OcultarProgresoPrincipal));
            return;
        }

        lblEstadoPrincipal.Visible = false;
        progressBarPrincipal.Visible = false;
    }

    #endregion

    private void SeleccionarJuegoEnLista(Juego juegoSeleccionado)
    {
        for (int i = 0; i < listaJuegos.Items.Count; i++)
        {
            if (listaJuegos.Items[i].Tag is Juego juego && juego.Nombre == juegoSeleccionado.Nombre)
            {
                listaJuegos.Items[i].Selected = true;
                listaJuegos.EnsureVisible(i);
                listaJuegos.Focus();
                return;
            }
        }

        MessageBox.Show(this, "El juego seleccionado no se encuentra en la vista actual (posiblemente filtrado).", 
            "Búsqueda Avanzada", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
