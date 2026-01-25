using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IgameToolsWinForms;

public partial class FormProgreso : Form
{
    // Inicialización explícita de los campos para evitar CS8618
    private ProgressBar _progressBar = null!;
    private Label _lblEstado = null!;
    private Button _btnCancelar = null!;
    
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _cancellationToken;
    private readonly bool _permitirCancelar;
    
    public bool Cancelado => _cancellationToken.IsCancellationRequested;
    
    public FormProgreso(string titulo = "Progreso", bool permitirCancelar = true)
    {
        _permitirCancelar = permitirCancelar;
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        
        InitializeComponent(titulo);
    }

    private void InitializeComponent(string titulo)
    {
        // Configuración del formulario
        Text = titulo;
        Size = new Size(450, 180);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        Font = new Font("Segoe UI", 9F);

        // Etiqueta de estado
        _lblEstado = new Label
        {
            Text = "Iniciando...",
            Location = new Point(20, 20),
            Size = new Size(400, 25),
            TextAlign = ContentAlignment.MiddleLeft,
            UseCompatibleTextRendering = true
        };

        // ProgressBar
        _progressBar = new ProgressBar
        {
            Location = new Point(20, 50),
            Size = new Size(400, 25),
            Style = ProgressBarStyle.Continuous,
            Minimum = 0,
            Maximum = 100,
            Value = 0
        };

        // Botón cancelar
        _btnCancelar = new Button
        {
            Text = "Cancelar",
            Location = new Point(345, 90),
            Size = new Size(75, 30),
            UseVisualStyleBackColor = true,
            Enabled = _permitirCancelar
        };
        _btnCancelar.Click += BtnCancelar_Click;

        Controls.AddRange(new Control[] { _lblEstado, _progressBar, _btnCancelar });
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        if (_permitirCancelar && _cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _btnCancelar.Enabled = false;
            _lblEstado.Text = "Cancelando...";
        }
    }

    public CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? CancellationToken.None;

    public void ActualizarProgreso(int porcentaje, string? estado = null)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<int, string?>(ActualizarProgreso), porcentaje, estado);
            return;
        }

        _progressBar.Value = Math.Max(0, Math.Min(100, porcentaje));
        if (!string.IsNullOrWhiteSpace(estado))
        {
            _lblEstado.Text = estado;
        }
    }

    public void ActualizarEstado(string estado)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(ActualizarEstado), estado);
            return;
        }

        _lblEstado.Text = estado;
    }

    public void EstablecerIndeterminado(bool indeterminado)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<bool>(EstablecerIndeterminado), indeterminado);
            return;
        }

        _progressBar.Style = indeterminado ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!Cancelado && _permitirCancelar)
        {
            // Si el usuario cierra la ventana, tratamos como cancelación
            _cancellationTokenSource?.Cancel();
        }
        base.OnFormClosing(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource?.Dispose();
        }
        base.Dispose(disposing);
    }
}