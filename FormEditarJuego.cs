using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IgameToolsWinForms;

public partial class FormEditarJuego : Form
{
    public string NombreEditado => txtNombre.Text;
    public string NombreCortoEditado => txtNombreCorto.Text;
    public string SlaveEditado => txtSlave.Text;
    public string GeneroEditado => cmbGenero.Text;

    public FormEditarJuego(string nombre, string nombreCorto, string slave, string genero, IReadOnlyList<string> generos)
    {
        InitializeComponent();
        
        // Configurar icono del formulario
        try
        {
            var iconPath = Path.Combine(AppContext.BaseDirectory, "img", "amiga.png");
            if (File.Exists(iconPath))
            {
                using var bitmap = new Bitmap(iconPath);
                Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
            }
        }
        catch
        {
            // Si no se puede cargar el icono, continuar sin él
        }

        txtNombre.Text = nombre;
        txtNombreCorto.Text = nombreCorto;
        txtSlave.Text = slave;

        cmbGenero.BeginUpdate();
        try
        {
            cmbGenero.Items.Clear();
            foreach (var g in generos.Distinct(StringComparer.CurrentCultureIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(g))
                {
                    cmbGenero.Items.Add(g);
                }
            }
        }
        finally
        {
            cmbGenero.EndUpdate();
        }

        cmbGenero.Text = genero;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
