using System;
using System.Drawing;
using System.Windows.Forms;
using IgameToolsWinForms.Modelos;
using IgameToolsWinForms.Servicios;

namespace IgameToolsWinForms.Controles;

public partial class PanelEstadisticas : UserControl
{
    private readonly ServicioEstadisticas _servicioEstadisticas;
    private Estadisticas _estadisticasActuales = new();

    // Controles del panel
    private GroupBox grpEstadisticas;
    private Label lblTotalJuegos;
    private Label lblJuegosUnicos;
    private Label lblDuplicados;
    private Label lblDesconocidos;
    private Label lblActualizados;
    private Label lblGenerosUnicos;
    private Label lblTasaActualizacion;
    private Label lblGeneroMasComun;
    private Button btnExportar;
    private Button btnActualizar;

    public PanelEstadisticas()
    {
        _servicioEstadisticas = new ServicioEstadisticas();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        // Configuración del panel
        BackColor = Color.FromArgb(240, 240, 240);
        BorderStyle = BorderStyle.FixedSingle;
        MinimumSize = new Size(250, 300);
        Size = new Size(280, 350);

        // GroupBox principal
        grpEstadisticas = new GroupBox
        {
            Text = "📊 Estadísticas",
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 51, 51),
            Location = new Point(5, 5),
            Size = new Size(270, 320),
            TabStop = false
        };

        // Labels de estadísticas
        var yPos = 25;
        var labelHeight = 22;
        var spacing = 5;

        lblTotalJuegos = CrearLabelEstadistica("Total juegos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblJuegosUnicos = CrearLabelEstadistica("Juegos únicos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblDuplicados = CrearLabelEstadistica("Duplicados:", "0", yPos);
        yPos += labelHeight + spacing;

        lblDesconocidos = CrearLabelEstadistica("Desconocidos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblActualizados = CrearLabelEstadistica("Actualizados:", "0", yPos);
        yPos += labelHeight + spacing;

        lblGenerosUnicos = CrearLabelEstadistica("Géneros únicos:", "0", yPos);
        yPos += labelHeight + spacing;

        lblTasaActualizacion = CrearLabelEstadistica("Tasa actualización:", "0%", yPos);
        yPos += labelHeight + spacing;

        lblGeneroMasComun = CrearLabelEstadistica("Género común:", "-", yPos);
        yPos += labelHeight + spacing + 10;

        // Botones
        btnActualizar = new Button
        {
            Text = "🔄 Actualizar",
            Location = new Point(10, yPos),
            Size = new Size(120, 30),
            UseVisualStyleBackColor = true,
            Font = new Font("Segoe UI", 8F)
        };
        btnActualizar.Click += BtnActualizar_Click;

        btnExportar = new Button
        {
            Text = "📋 Exportar",
            Location = new Point(140, yPos),
            Size = new Size(120, 30),
            UseVisualStyleBackColor = true,
            Font = new Font("Segoe UI", 8F)
        };
        btnExportar.Click += BtnExportar_Click;

        // Agregar controles al GroupBox
        grpEstadisticas.Controls.AddRange(new Control[]
        {
            lblTotalJuegos, lblJuegosUnicos, lblDuplicados, lblDesconocidos,
            lblActualizados, lblGenerosUnicos, lblTasaActualizacion, lblGeneroMasComun,
            btnActualizar, btnExportar
        });

        // Agregar GroupBox al panel
        Controls.Add(grpEstadisticas);

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
            ForeColor = Color.FromArgb(0, 120, 215),
            TextAlign = ContentAlignment.MiddleRight
        };

        // Agregar ambos labels al panel (devolvemos el valor para referencia)
        grpEstadisticas.Controls.Add(lblTexto);
        return lblValor;
    }

    public void ActualizarEstadisticas(System.Collections.Generic.List<Juego> juegos)
    {
        try
        {
            _estadisticasActuales = _servicioEstadisticas.CalcularEstadisticas(juegos);
            ActualizarInterfaz();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al calcular estadísticas: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ActualizarInterfaz()
    {
        if (InvokeRequired)
        {
            Invoke(new Action(ActualizarInterfaz));
            return;
        }

        lblTotalJuegos.Text = $"{_estadisticasActuales.TotalJuegos:N0}";
        lblJuegosUnicos.Text = $"{_estadisticasActuales.JuegosUnicos:N0}";
        lblDuplicados.Text = $"{_estadisticasActuales.Duplicados:N0} ({_estadisticasActuales.PorcentajeDuplicados:F1}%)";
        lblDesconocidos.Text = $"{_estadisticasActuales.Desconocidos:N0} ({_estadisticasActuales.PorcentajeDesconocidos:F1}%)";
        lblActualizados.Text = $"{_estadisticasActuales.Actualizados:N0} ({_estadisticasActuales.PorcentajeActualizados:F1}%)";
        lblGenerosUnicos.Text = $"{_estadisticasActuales.GenerosUnicos:N0}";
        lblTasaActualizacion.Text = $"{_estadisticasActuales.TasaActualizacion:F1}%";
        
        if (!string.IsNullOrWhiteSpace(_estadisticasActuales.GeneroMasComun))
        {
            lblGeneroMasComun.Text = $"{_estadisticasActuales.GeneroMasComun} ({_estadisticasActuales.JuegosDelGeneroMasComun})";
        }
        else
        {
            lblGeneroMasComun.Text = "-";
        }

        // Colorear según estado
        lblDuplicados.ForeColor = _estadisticasActuales.Duplicados > 0 ? Color.Red : Color.Green;
        lblDesconocidos.ForeColor = _estadisticasActuales.Desconocidos > 0 ? Color.Orange : Color.Green;
        lblActualizados.ForeColor = _estadisticasActuales.PorcentajeActualizados > 80 ? Color.Green : Color.Orange;
    }

    private void BtnActualizar_Click(object? sender, EventArgs e)
    {
        // Disparar evento para que el formulario principal actualice las estadísticas
        ActualizarEstadisticasRequested?.Invoke(this, EventArgs.Empty);
    }

    private void BtnExportar_Click(object? sender, EventArgs e)
    {
        try
        {
            var textoEstadisticas = _servicioEstadisticas.FormatearEstadisticasTexto(_estadisticasActuales);
            
            Clipboard.SetText(textoEstadisticas);
            MessageBox.Show("Estadísticas copiadas al portapapeles.", "Exportación completada", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar estadísticas: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public event EventHandler? ActualizarEstadisticasRequested;
}
