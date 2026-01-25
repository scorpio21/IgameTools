using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using IgameToolsWinForms.Servicios;

namespace IgameToolsWinForms;

public partial class FormBusquedaAvanzada : Form
{
    private readonly List<Juego> _juegos;
    private List<Juego> _resultados = new();
    private int _indiceResultadoActual = -1;

    public FormBusquedaAvanzada(List<Juego> juegos)
    {
        _juegos = juegos ?? new List<Juego>();
        InitializeComponent();
        ConfigurarEventos();
        ConfigurarListView();
    }

    private void ConfigurarEventos()
    {
        btnBuscar.Click += BtnBuscar_Click;
        btnLimpiar.Click += BtnLimpiar_Click;
        btnSiguienteNav.Click += BtnSiguiente_Click;
        btnAnterior.Click += BtnAnterior_Click;
        btnSeleccionar.Click += BtnSeleccionar_Click;
        btnCerrar.Click += BtnCerrar_Click;
        txtTermino.TextChanged += TxtTermino_TextChanged;
        cmbTipo.SelectedIndexChanged += CmbTipo_SelectedIndexChanged;
        lstResultados.DoubleClick += LstResultados_DoubleClick;
        lstResultados.SelectedIndexChanged += LstResultados_SelectedIndexChanged;

        // Atajos de teclado
        KeyPreview = true;
        KeyDown += FormBusquedaAvanzada_KeyDown;
    }

    private void ConfigurarListView()
    {
        // Las columnas ya están configuradas en el diseñador
        // Solo aseguramos que estén configuradas correctamente
        if (lstResultados.Columns.Count == 0)
        {
            lstResultados.Columns.AddRange(new ColumnHeader[] { colNombre, colGenero, colSlave, colRuta });
        }
    }

    private void FormBusquedaAvanzada_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F3:
                if (e.Shift)
                    BtnAnterior_Click(null, EventArgs.Empty);
                else
                    BtnSiguiente_Click(null, EventArgs.Empty);
                break;
            case Keys.Escape:
                BtnCerrar_Click(null, EventArgs.Empty);
                break;
        }
    }

    private void CmbTipo_SelectedIndexChanged(object? sender, EventArgs e)
    {
        chkRegex.Enabled = cmbTipo.SelectedIndex == 4; // Regex
        if (cmbTipo.SelectedIndex != 4)
            chkRegex.Checked = false;
    }

    private void TxtTermino_TextChanged(object? sender, EventArgs e)
    {
        btnBuscar.Enabled = !string.IsNullOrWhiteSpace(txtTermino.Text);
    }

    private void BtnBuscar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTermino.Text))
        {
            MessageBox.Show("Ingrese un término de búsqueda.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            RealizarBusqueda();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error en la búsqueda: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RealizarBusqueda()
    {
        var campo = cmbCampo.SelectedItem?.ToString() ?? "Todos";
        var tipo = cmbTipo.SelectedItem?.ToString() ?? "Contiene";
        var termino = txtTermino.Text;
        var mayusculas = chkMayusculas.Checked;
        var usarRegex = chkRegex.Checked;

        _resultados = new ServicioBusquedaAvanzada().Buscar(_juegos, campo, tipo, termino, mayusculas, usarRegex);
        _indiceResultadoActual = _resultados.Count > 0 ? 0 : -1;

        ActualizarResultados();
    }

    private void ActualizarResultados()
    {
        lstResultados.Items.Clear();
        lblResultados.Text = $"📋 Resultados: {_resultados.Count} encontrados";

        foreach (var juego in _resultados)
        {
            var item = new ListViewItem(new string[]
            {
                juego.Nombre,
                juego.Genero,
                juego.Slave,
                juego.Path
            });
            item.Tag = juego;
            lstResultados.Items.Add(item);
        }

        if (_resultados.Count > 0)
        {
            lstResultados.Items[0].Selected = true;
            lstResultados.EnsureVisible(0);
        }

        ActualizarBotonesNavegacion();
    }

    private void ActualizarBotonesNavegacion()
    {
        btnSiguiente.Enabled = _resultados.Count > 0 && _indiceResultadoActual < _resultados.Count - 1;
        btnAnterior.Enabled = _resultados.Count > 0 && _indiceResultadoActual > 0;
        btnSeleccionar.Enabled = _resultados.Count > 0;
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        txtTermino.Clear();
        lstResultados.Items.Clear();
        _resultados.Clear();
        _indiceResultadoActual = -1;
        lblResultados.Text = "📋 Resultados: 0 encontrados";
        ActualizarBotonesNavegacion();
        txtTermino.Focus();
    }

    private void BtnSiguiente_Click(object? sender, EventArgs e)
    {
        if (_indiceResultadoActual < _resultados.Count - 1)
        {
            _indiceResultadoActual++;
            lstResultados.Items[_indiceResultadoActual].Selected = true;
            lstResultados.EnsureVisible(_indiceResultadoActual);
            ActualizarBotonesNavegacion();
        }
    }

    private void BtnAnterior_Click(object? sender, EventArgs e)
    {
        if (_indiceResultadoActual > 0)
        {
            _indiceResultadoActual--;
            lstResultados.Items[_indiceResultadoActual].Selected = true;
            lstResultados.EnsureVisible(_indiceResultadoActual);
            ActualizarBotonesNavegacion();
        }
    }

    private void LstResultados_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (lstResultados.SelectedItems.Count > 0)
        {
            _indiceResultadoActual = lstResultados.SelectedItems[0].Index;
            ActualizarBotonesNavegacion();
        }
    }

    private void LstResultados_DoubleClick(object? sender, EventArgs e)
    {
        BtnSeleccionar_Click(sender, e);
    }

    private void BtnSeleccionar_Click(object? sender, EventArgs e)
    {
        if (lstResultados.SelectedItems.Count > 0)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void BtnCerrar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    public Juego? JuegoSeleccionado => lstResultados.SelectedItems.Count > 0 
        ? (lstResultados.SelectedItems[0].Tag as Juego?)
        : null;
}
