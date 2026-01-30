using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IgameToolsWinForms
{
    public partial class FormConsolaDescarga : Form
    {
        private readonly RichTextBox _txt;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool DestroyIcon(IntPtr handle);

        public FormConsolaDescarga(string titulo)
        {
            Text = titulo;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(900, 500);
            MinimizeBox = true;
            MaximizeBox = true;

            try
            {
                var rutaIcono = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "amiga.ico");
                if (File.Exists(rutaIcono))
                {
                    Icon = new Icon(rutaIcono);
                }
                else
                {
                    var rutaPng = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "amiga.png");
                    if (File.Exists(rutaPng))
                    {
                        using var bmp = new Bitmap(rutaPng);
                        var hIcon = bmp.GetHicon();
                        try
                        {
                            using var tmp = Icon.FromHandle(hIcon);
                            Icon = (Icon)tmp.Clone();
                        }
                        finally
                        {
                            DestroyIcon(hIcon);
                        }
                    }

                    var iconoExe = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                    if (iconoExe != null)
                        Icon = iconoExe;
                }
            }
            catch
            {
            }

            _txt = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Consolas", 10f, FontStyle.Regular),
                HideSelection = false,
                DetectUrls = false,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            Controls.Add(_txt);
        }

        public void EscribirLinea(string texto, Color color)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => EscribirLinea(texto, color)));
                return;
            }

            var inicio = _txt.TextLength;
            _txt.AppendText((texto ?? string.Empty) + Environment.NewLine);
            var fin = _txt.TextLength;

            _txt.Select(inicio, fin - inicio);
            _txt.SelectionColor = color;
            _txt.SelectionLength = 0;
            _txt.ScrollToCaret();
        }

        public void Limpiar()
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(Limpiar));
                return;
            }

            _txt.Clear();
        }
    }
}
