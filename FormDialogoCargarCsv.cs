using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IgameToolsWinForms
{
    public partial class FormDialogoCargarCsv : Form
    {
        private string _rutaSeleccionada = string.Empty;
        private string _carpetaActual = string.Empty;
        private Dictionary<string, string> _carpetasRapidas = new();

        public string RutaSeleccionada => _rutaSeleccionada;

        public FormDialogoCargarCsv()
        {
            InitializeComponent();
            InicializarCarpetasRapidas();
            DetectarUnidadesAutomaticamente();
            CargarCarpetasRapidas();
            
            // Conectar eventos manualmente
            cmbCarpetasRapidas.SelectedIndexChanged += cmbCarpetasRapidas_SelectedIndexChanged;
            lstArchivos.SelectedIndexChanged += lstArchivos_SelectedIndexChanged;
            lstArchivos.DoubleClick += lstArchivos_DoubleClick;
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void InicializarCarpetasRapidas()
        {
            _carpetasRapidas = new Dictionary<string, string>
            {
                ["Documentos"] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                ["Escritorio"] = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                ["Mis Imágenes"] = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                ["Mis Videos"] = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                ["Descargas"] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                ["Carpeta de la aplicación"] = Application.StartupPath
            };
        }

        private void DetectarUnidadesAutomaticamente()
        {
            try
            {
                var todasLasUnidades = DriveInfo.GetDrives();
                foreach (var unidad in todasLasUnidades)
                {
                    if (unidad.IsReady && (unidad.DriveType == DriveType.Fixed || unidad.DriveType == DriveType.Removable))
                    {
                        var nombreUnidad = unidad.Name; // Ej: "C:\", "D:\", "E:\", etc.
                        if (!_carpetasRapidas.ContainsKey(nombreUnidad))
                        {
                            // Agregar etiqueta descriptiva
                            var etiquetaUnidad = nombreUnidad;
                            if (!string.IsNullOrEmpty(unidad.VolumeLabel))
                            {
                                etiquetaUnidad = $"{nombreUnidad} ({unidad.VolumeLabel})";
                            }
                            else if (unidad.DriveType == DriveType.Removable)
                            {
                                etiquetaUnidad = $"{nombreUnidad} (USB)";
                            }

                            _carpetasRapidas[etiquetaUnidad] = nombreUnidad;
                        }
                    }
                }
            }
            catch
            {
                // Si falla la detección, continuar con las carpetas predefinidas
            }
        }

        private void CargarCarpetasRapidas()
        {
            cmbCarpetasRapidas.Items.Clear();
            foreach (var carpeta in _carpetasRapidas)
            {
                cmbCarpetasRapidas.Items.Add(carpeta.Key);
            }

            if (cmbCarpetasRapidas.Items.Count > 0)
            {
                cmbCarpetasRapidas.SelectedIndex = 0;
            }
        }

        private void CargarContenidoCarpeta(string rutaCarpeta)
        {
            try
            {
                lstArchivos.Items.Clear();
                txtRuta.Text = string.Empty;

                if (!Directory.Exists(rutaCarpeta))
                {
                    lstArchivos.Items.Add("La carpeta no existe");
                    return;
                }

                // Agregar carpeta padre (si no es raíz)
                var padre = Directory.GetParent(rutaCarpeta);
                if (padre != null)
                {
                    lstArchivos.Items.Add("[..] " + padre.Name);
                }

                // Agregar subcarpetas
                try
                {
                    var subcarpetas = Directory.GetDirectories(rutaCarpeta);
                    foreach (var subcarpeta in subcarpetas)
                    {
                        var nombreCarpeta = Path.GetFileName(subcarpeta);
                        lstArchivos.Items.Add("[📁] " + nombreCarpeta);
                    }
                }
                catch
                {
                    // Ignorar carpetas inaccesibles
                }

                // Agregar archivos CSV
                try
                {
                    var archivos = Directory.GetFiles(rutaCarpeta, "*.csv", SearchOption.TopDirectoryOnly);
                    foreach (var archivo in archivos)
                    {
                        lstArchivos.Items.Add("[📄] " + Path.GetFileName(archivo));
                    }

                    if (lstArchivos.Items.Count == 0)
                    {
                        lstArchivos.Items.Add("No hay archivos CSV en esta carpeta");
                    }
                }
                catch
                {
                    // Ignorar errores de archivos
                }
            }
            catch (Exception ex)
            {
                lstArchivos.Items.Add($"Error: {ex.Message}");
            }
        }

        private void cmbCarpetasRapidas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCarpetasRapidas.SelectedItem != null)
            {
                var nombreCarpeta = cmbCarpetasRapidas.SelectedItem.ToString();
                if (_carpetasRapidas.TryGetValue(nombreCarpeta, out var rutaCarpeta))
                {
                    _carpetaActual = rutaCarpeta;
                    CargarContenidoCarpeta(_carpetaActual);
                }
            }
        }

        private void lstArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstArchivos.SelectedItem != null)
            {
                var itemSeleccionado = lstArchivos.SelectedItem.ToString();

                if (itemSeleccionado.StartsWith("[..] "))
                {
                    // Navegar a carpeta padre
                    var padre = Directory.GetParent(_carpetaActual);
                    if (padre != null)
                    {
                        _carpetaActual = padre.FullName;
                        CargarContenidoCarpeta(_carpetaActual);
                    }
                }
                else if (itemSeleccionado.StartsWith("[📁] "))
                {
                    // Entrar en subcarpeta
                    var nombreCarpeta = itemSeleccionado.Substring(5); // Quitar "[📁] "
                    var rutaSubcarpeta = Path.Combine(_carpetaActual, nombreCarpeta);

                    if (Directory.Exists(rutaSubcarpeta))
                    {
                        _carpetaActual = rutaSubcarpeta;
                        CargarContenidoCarpeta(_carpetaActual);
                    }
                }
                else if (itemSeleccionado.StartsWith("[📄] "))
                {
                    // Seleccionar archivo CSV
                    var nombreArchivo = itemSeleccionado.Substring(5); // Quitar "[📄] "
                    var rutaCompleta = Path.Combine(_carpetaActual, nombreArchivo);
                    txtRuta.Text = rutaCompleta;
                }
            }
        }

        private void lstArchivos_DoubleClick(object sender, EventArgs e)
        {
            if (lstArchivos.SelectedItem != null)
            {
                var itemSeleccionado = lstArchivos.SelectedItem.ToString();

                if (itemSeleccionado.StartsWith("[📁] "))
                {
                    // Entrar en subcarpeta
                    var nombreCarpeta = itemSeleccionado.Substring(5);
                    var rutaSubcarpeta = Path.Combine(_carpetaActual, nombreCarpeta);

                    if (Directory.Exists(rutaSubcarpeta))
                    {
                        _carpetaActual = rutaSubcarpeta;
                        CargarContenidoCarpeta(_carpetaActual);
                    }
                }
                else if (itemSeleccionado.StartsWith("[📄] "))
                {
                    // Cargar archivo directamente
                    btnAceptar.PerformClick();
                }
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            _rutaSeleccionada = txtRuta.Text.Trim();

            if (string.IsNullOrEmpty(_rutaSeleccionada))
            {
                MessageBox.Show(this, "No se seleccionó ningún archivo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(_rutaSeleccionada))
            {
                MessageBox.Show(this, "El archivo no existe en la ruta especificada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_rutaSeleccionada.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "El archivo debe tener extensión .csv", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
