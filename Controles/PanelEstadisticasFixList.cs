using System;
using System.Drawing;
using System.Windows.Forms;
using IgameToolsWinForms.Modelos;
using IgameToolsWinForms.Servicios;

namespace IgameToolsWinForms.Controles;

public partial class PanelEstadisticasFixList : UserControl
{
    private readonly ServicioEstadisticasFixList _servicioEstadisticasFixList;
    private EstadisticasFixList _estadisticasActuales = new();

    // Controles del panel
    private GroupBox grpEstadisticasFix;
    private Label lblTotalProcesados;
    private Label lblActualizados;
    private Label lblSinCambios;
    private Label lblGenerosCorregidos;
    private Label lblPathsEncontrados;
    private Label lblPathsCorregidos;
    private Label lblSlavesEncontrados;
    private Label lblErroresCorregidos;
    private Label lblDatosExtra;
    private Label lblPorcentajeCorregidos;
    private Label lblDuracion;
    private Label lblFecha;
    private Button btnExportar;

    public PanelEstadisticasFixList()
    {
        _servicioEstadisticasFixList = new ServicioEstadisticasFixList();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        // Configuración del panel
        BackColor = Color.FromArgb(245, 245, 220); // Color amarillo claro para distinguir
        BorderStyle = BorderStyle.FixedSingle;
        MinimumSize = new Size(250, 300);
        Size = new Size(280, 350);

        // GroupBox principal
        grpEstadisticasFix = new GroupBox
        {
            Text = "🔧 Estadísticas Fix List",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(139, 69, 19), // Color marrón
            Location = new Point(5, 5),
            Size = new Size(270, 320),
            TabStop = false
        };

        // Labels de estadísticas
        var yPos = 25;
        var labelHeight = 22;
        var spacing = 5;

        lblTotalProcesados = CrearLabelEstadistica("Total procesados:", "0", yPos);
        yPos += labelHeight + spacing;

        lblActualizados = CrearLabelEstadistica("Actualizados:", "0", yPos);
        yPos += labelHeight + spacing;

        lblSinCambios = CrearLabelEstadistica("Sin cambios:", "0", yPos);
        yPos += labelHeight + spacing;

        lblGenerosCorregidos = CrearLabelEstadistica("Géneros corregidos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblPathsEncontrados = CrearLabelEstadistica("Paths encontrados:", "0", yPos);
        yPos += labelHeight + spacing;

        lblPathsCorregidos = CrearLabelEstadistica("Paths corregidos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblSlavesEncontrados = CrearLabelEstadistica("Slaves encontrados:", "0", yPos);
        yPos += labelHeight + spacing;

        lblErroresCorregidos = CrearLabelEstadistica("Errores corregidos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblDatosExtra = CrearLabelEstadistica("Con datos extra:", "0", yPos);
        yPos += labelHeight + spacing;

        lblPorcentajeCorregidos = CrearLabelEstadistica("% corregidos:", "0%", yPos);
        yPos += labelHeight + spacing;

        lblDuracion = CrearLabelEstadistica("Duración:", "00:00", yPos);
        yPos += labelHeight + spacing;

        lblFecha = CrearLabelEstadistica("Fecha:", "-", yPos);
        yPos += labelHeight + spacing + 10;

        // Botón exportar
        btnExportar = new Button
        {
            Text = "📋 Exportar",
            Location = new Point(85, yPos),
            Size = new Size(100, 30),
            UseVisualStyleBackColor = true,
            Font = new Font("Segoe UI", 8F)
        };
        btnExportar.Click += BtnExportar_Click;

        // Agregar controles al GroupBox
        grpEstadisticasFix.Controls.AddRange(new Control[]
        {
            lblTotalProcesados, lblActualizados, lblSinCambios, lblGenerosCorregidos,
            lblPathsEncontrados, lblPathsCorregidos, lblSlavesEncontrados, lblErroresCorregidos,
            lblDatosExtra, lblPorcentajeCorregidos, lblDuracion, lblFecha, btnExportar
        });

        // Agregar GroupBox al panel
        Controls.Add(grpEstadisticasFix);

        ResumeLayout(false);
    }

    private Label CrearLabelEstadistica(string texto, string valor, int yPos)
    {
        var lblTexto = new Label
        {
            Text = texto,
            Location = new Point(10, yPos),
            Size = new Size(120, 20),
            Font = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(51, 51, 51)
        };

        var lblValor = new Label
        {
            Text = valor,
            Name = $"lbl{texto.Replace(":", "").Replace(" ", "")}",
            Location = new Point(135, yPos),
            Size = new Size(120, 20),
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            ForeColor = Color.FromArgb(139, 69, 19), // Color marrón para Fix List
            TextAlign = ContentAlignment.MiddleRight
        };

        // Agregar ambos labels al panel (devolvemos el valor para referencia)
        grpEstadisticasFix.Controls.Add(lblTexto);
        return lblValor;
    }

    public void ActualizarEstadisticas(List<Juego> juegosEntrada, List<Juego> juegosSalida, string directorioTrabajo, TimeSpan duracion)
    {
        try
        {
            _estadisticasActuales = _servicioEstadisticasFixList.CalcularEstadisticasFixList(
                juegosEntrada, juegosSalida, directorioTrabajo, duracion);

            // Actualizar labels
            lblTotalProcesados.Text = $"{_estadisticasActuales.TotalJuegosProcesados:N0}";
            lblActualizados.Text = $"{_estadisticasActuales.JuegosActualizados:N0}";
            lblSinCambios.Text = $"{_estadisticasActuales.JuegosSinCambios:N0}";
            lblGenerosCorregidos.Text = $"{_estadisticasActuales.GenerosCorregidos:N0}";
            lblPathsEncontrados.Text = $"{_estadisticasActuales.PathsEncontrados:N0}";
            lblPathsCorregidos.Text = $"{_estadisticasActuales.PathsCorregidos:N0}";
            lblSlavesEncontrados.Text = $"{_estadisticasActuales.SlavesEncontrados:N0}";
            lblErroresCorregidos.Text = $"{_estadisticasActuales.ErroresCorregidos:N0}";
            lblDatosExtra.Text = $"{_estadisticasActuales.JuegosConDatosExtra:N0}";
            lblPorcentajeCorregidos.Text = $"{_estadisticasActuales.PorcentajeCorregidos:F1}%";
            lblDuracion.Text = $"{_estadisticasActuales.DuracionProceso:mm\\:ss}";
            lblFecha.Text = $"{_estadisticasActuales.FechaEjecucion:HH:mm:ss}";

            Visible = true;
        }
        catch (Exception ex)
        {
            // En caso de error, ocultar el panel
            Visible = false;
        }
    }

    private void BtnExportar_Click(object? sender, EventArgs e)
    {
        try
        {
            var textoEstadisticas = _servicioEstadisticasFixList.FormatearEstadisticasFixListTexto(_estadisticasActuales);
            
            Clipboard.SetText(textoEstadisticas);
            MessageBox.Show("Estadísticas Fix List copiadas al portapapeles.", "Exportación completada", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar estadísticas Fix List: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Limpiar()
    {
        _estadisticasActuales = new EstadisticasFixList();
        
        lblTotalProcesados.Text = "0";
        lblActualizados.Text = "0";
        lblSinCambios.Text = "0";
        lblGenerosCorregidos.Text = "0";
        lblPathsEncontrados.Text = "0";
        lblPathsCorregidos.Text = "0";
        lblSlavesEncontrados.Text = "0";
        lblErroresCorregidos.Text = "0";
        lblDatosExtra.Text = "0";
        lblPorcentajeCorregidos.Text = "0%";
        lblDuracion.Text = "00:00";
        lblFecha.Text = "-";
        
        Visible = false;
    }
}
