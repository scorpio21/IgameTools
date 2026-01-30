namespace IgameToolsWinForms;

partial class FormPrincipal
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
        listaJuegos = new ListView();
        colNombre = new ColumnHeader();
        colGenero = new ColumnHeader();
        colSlave = new ColumnHeader();
        colRuta = new ColumnHeader();
        colPreview = new ColumnHeader();
        btnCargarCsv = new Button();
        btnArreglarLista = new Button();
        btnGuardarCsv = new Button();
        btnEtiquetaRapida = new Button();
        btnLimpiarLista = new Button();
        btnDeshacer = new Button();
        btnAyuda = new Button();
        chkMantenerDatos = new CheckBox();
        chkNombresCortos = new CheckBox();
        chkVerDuplicados = new CheckBox();
        chkVerDesconocidos = new CheckBox();
        lblTitleCase = new Label();
        cmbTitleCase = new ComboBox();
        txtBusqueda = new TextBox();
        panelEstadisticas = new IgameToolsWinForms.Controles.PanelEstadisticas();
        panelEstadisticasFix = new IgameToolsWinForms.Controles.PanelEstadisticasFixList();
        menuPrincipal = new MenuStrip();
        menuArchivo = new ToolStripMenuItem();
        menuItemSalir = new ToolStripMenuItem();
        menuUtilidades = new ToolStripMenuItem();
        menuItemBusquedaAvanzada = new ToolStripMenuItem();
        progressBarPrincipal = new ProgressBar();
        lblEstadoPrincipal = new Label();
        menuPrincipal.SuspendLayout();
        SuspendLayout();
        // 
        // listaJuegos
        // 
        listaJuegos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        listaJuegos.Columns.AddRange(new ColumnHeader[] { colNombre, colGenero, colSlave, colRuta, colPreview });
        listaJuegos.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        listaJuegos.FullRowSelect = true;
        listaJuegos.GridLines = true;
        listaJuegos.Location = new Point(0, 37);
        listaJuegos.Margin = new Padding(4, 5, 4, 5);
        listaJuegos.Name = "listaJuegos";
        listaJuegos.Size = new Size(1317, 875);
        listaJuegos.TabIndex = 0;
        listaJuegos.UseCompatibleStateImageBehavior = false;
        listaJuegos.View = View.Details;
        listaJuegos.ColumnWidthChanged += listaJuegos_ColumnWidthChanged;
            // listaJuegos.ColumnWidthChanging += listaJuegos_ColumnWidthChanging; // Comentado - método no existe
        listaJuegos.DoubleClick += listaJuegos_DoubleClick;
        // 
        // colNombre
        // 
        colNombre.Text = "Nombre";
        colNombre.Width = 240;
        // 
        // colGenero
        // 
        colGenero.Text = "Genero";
        colGenero.Width = 220;
        // 
        // colSlave
        // 
        colSlave.Text = "Nombre .Slave";
        colSlave.Width = 200;
        // 
        // colRuta
        // 
        colRuta.Text = "Ruta";
        colRuta.Width = 220;
        // 
        // colPreview
        // 
        colPreview.Text = "Preview";
        colPreview.Width = 180;
        // 
        // btnCargarCsv
        // 
        btnCargarCsv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnCargarCsv.Location = new Point(7, 950);
        btnCargarCsv.Margin = new Padding(4, 5, 4, 5);
        btnCargarCsv.Name = "btnCargarCsv";
        btnCargarCsv.Size = new Size(114, 67);
        btnCargarCsv.TabIndex = 1;
        btnCargarCsv.Text = "Cargar CSV";
        btnCargarCsv.UseVisualStyleBackColor = true;
        btnCargarCsv.Click += btnCargarCsv_Click;
        // 
        // btnArreglarLista
        // 
        btnArreglarLista.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnArreglarLista.Location = new Point(129, 950);
        btnArreglarLista.Margin = new Padding(4, 5, 4, 5);
        btnArreglarLista.Name = "btnArreglarLista";
        btnArreglarLista.Size = new Size(114, 67);
        btnArreglarLista.TabIndex = 2;
        btnArreglarLista.Text = "Arreglar lista";
        btnArreglarLista.UseVisualStyleBackColor = true;
        btnArreglarLista.Click += btnArreglarLista_Click;
        // 
        // btnGuardarCsv
        // 
        btnGuardarCsv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnGuardarCsv.Location = new Point(250, 950);
        btnGuardarCsv.Margin = new Padding(4, 5, 4, 5);
        btnGuardarCsv.Name = "btnGuardarCsv";
        btnGuardarCsv.Size = new Size(114, 67);
        btnGuardarCsv.TabIndex = 3;
        btnGuardarCsv.Text = "Guardar CSV";
        btnGuardarCsv.UseVisualStyleBackColor = true;
        btnGuardarCsv.Click += btnGuardarCsv_Click;
        // 
        // btnEtiquetaRapida
        // 
        btnEtiquetaRapida.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnEtiquetaRapida.Location = new Point(371, 950);
        btnEtiquetaRapida.Margin = new Padding(4, 5, 4, 5);
        btnEtiquetaRapida.Name = "btnEtiquetaRapida";
        btnEtiquetaRapida.Size = new Size(114, 67);
        btnEtiquetaRapida.TabIndex = 4;
        btnEtiquetaRapida.Text = "Quitar etiqueta";
        btnEtiquetaRapida.UseVisualStyleBackColor = true;
        btnEtiquetaRapida.Click += btnEtiquetaRapida_Click;
        // 
        // btnLimpiarLista
        // 
        btnLimpiarLista.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnLimpiarLista.Location = new Point(493, 950);
        btnLimpiarLista.Margin = new Padding(4, 5, 4, 5);
        btnLimpiarLista.Name = "btnLimpiarLista";
        btnLimpiarLista.Size = new Size(114, 67);
        btnLimpiarLista.TabIndex = 5;
        btnLimpiarLista.Text = "Limpiar lista";
        btnLimpiarLista.UseVisualStyleBackColor = true;
        btnLimpiarLista.Click += btnLimpiarLista_Click;
        // 
        // btnDeshacer
        // 
        btnDeshacer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDeshacer.Location = new Point(614, 950);
        btnDeshacer.Margin = new Padding(4, 5, 4, 5);
        btnDeshacer.Name = "btnDeshacer";
        btnDeshacer.Size = new Size(114, 67);
        btnDeshacer.TabIndex = 6;
        btnDeshacer.Text = "Deshacer";
        btnDeshacer.UseVisualStyleBackColor = true;
        btnDeshacer.Click += btnDeshacer_Click;
        // 
        // btnAyuda
        // 
        btnAyuda.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAyuda.Location = new Point(1580, 948);
        btnAyuda.Margin = new Padding(4, 5, 4, 5);
        btnAyuda.Name = "btnAyuda";
        btnAyuda.Size = new Size(83, 67);
        btnAyuda.TabIndex = 13;
        btnAyuda.Text = "Ayuda";
        btnAyuda.UseVisualStyleBackColor = true;
        btnAyuda.Click += btnAyuda_Click;
        // 
        // chkMantenerDatos
        // 
        chkMantenerDatos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        chkMantenerDatos.AutoSize = true;
        chkMantenerDatos.Location = new Point(736, 956);
        chkMantenerDatos.Margin = new Padding(4, 5, 4, 5);
        chkMantenerDatos.Name = "chkMantenerDatos";
        chkMantenerDatos.Size = new Size(167, 29);
        chkMantenerDatos.TabIndex = 7;
        chkMantenerDatos.Text = "Conservar datos";
        chkMantenerDatos.UseVisualStyleBackColor = true;
        // 
        // chkNombresCortos
        // 
        chkNombresCortos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        chkNombresCortos.AutoSize = true;
        chkNombresCortos.Location = new Point(736, 990);
        chkNombresCortos.Margin = new Padding(4, 5, 4, 5);
        chkNombresCortos.Name = "chkNombresCortos";
        chkNombresCortos.Size = new Size(167, 29);
        chkNombresCortos.TabIndex = 8;
        chkNombresCortos.Text = "Nombres cortos";
        chkNombresCortos.UseVisualStyleBackColor = true;
        chkNombresCortos.CheckedChanged += chkNombresCortos_CheckedChanged;
        // 
        // chkVerDuplicados
        // 
        chkVerDuplicados.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        chkVerDuplicados.AutoSize = true;
        chkVerDuplicados.Location = new Point(896, 956);
        chkVerDuplicados.Margin = new Padding(4, 5, 4, 5);
        chkVerDuplicados.Name = "chkVerDuplicados";
        chkVerDuplicados.Size = new Size(192, 29);
        chkVerDuplicados.TabIndex = 9;
        chkVerDuplicados.Text = "Mostrar duplicados";
        chkVerDuplicados.UseVisualStyleBackColor = true;
        chkVerDuplicados.CheckedChanged += chkFiltros_CheckedChanged;
        // 
        // chkVerDesconocidos
        // 
        chkVerDesconocidos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        chkVerDesconocidos.AutoSize = true;
        chkVerDesconocidos.Location = new Point(896, 990);
        chkVerDesconocidos.Margin = new Padding(4, 5, 4, 5);
        chkVerDesconocidos.Name = "chkVerDesconocidos";
        chkVerDesconocidos.Size = new Size(215, 29);
        chkVerDesconocidos.TabIndex = 10;
        chkVerDesconocidos.Text = "Mostrar desconocidos";
        chkVerDesconocidos.UseVisualStyleBackColor = true;
        chkVerDesconocidos.CheckedChanged += chkFiltros_CheckedChanged;
        // 
        // lblTitleCase
        // 
        lblTitleCase.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblTitleCase.AutoSize = true;
        lblTitleCase.Location = new Point(1445, 950);
        lblTitleCase.Margin = new Padding(4, 0, 4, 0);
        lblTitleCase.Name = "lblTitleCase";
        lblTitleCase.Size = new Size(98, 25);
        lblTitleCase.TabIndex = 11;
        lblTitleCase.Text = "Titulo Case";
        // 
        // cmbTitleCase
        // 
        cmbTitleCase.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cmbTitleCase.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTitleCase.FormattingEnabled = true;
        cmbTitleCase.Items.AddRange(new object[] { "Camel Case", "lower case", "UPPER CASE" });
        cmbTitleCase.Location = new Point(1445, 980);
        cmbTitleCase.Margin = new Padding(4, 5, 4, 5);
        cmbTitleCase.Name = "cmbTitleCase";
        cmbTitleCase.Size = new Size(127, 33);
        cmbTitleCase.TabIndex = 12;
        cmbTitleCase.SelectedIndexChanged += cmbTitleCase_SelectedIndexChanged;
        // 
        // txtBusqueda
        // 
        txtBusqueda.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtBusqueda.Location = new Point(1118, 980);
        txtBusqueda.Name = "txtBusqueda";
        txtBusqueda.PlaceholderText = "Buscar por nombre, corto o género...";
        txtBusqueda.Size = new Size(307, 31);
        txtBusqueda.TabIndex = 14;
        txtBusqueda.TextChanged += TxtBusqueda_TextChanged;
        // 
        // panelEstadisticas
        // 
        panelEstadisticas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        panelEstadisticas.BackColor = Color.FromArgb(240, 240, 240);
        panelEstadisticas.BorderStyle = BorderStyle.FixedSingle;
        panelEstadisticas.Location = new Point(1343, 37);
        panelEstadisticas.MinimumSize = new Size(250, 300);
        panelEstadisticas.Name = "panelEstadisticas";
        panelEstadisticas.Size = new Size(320, 425);
        panelEstadisticas.TabIndex = 17;
        // 
        // panelEstadisticasFix
        // 
        panelEstadisticasFix.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        panelEstadisticasFix.BackColor = Color.FromArgb(245, 245, 220);
        panelEstadisticasFix.BorderStyle = BorderStyle.FixedSingle;
        panelEstadisticasFix.Location = new Point(1343, 380);
        panelEstadisticasFix.MinimumSize = new Size(250, 300);
        panelEstadisticasFix.Name = "panelEstadisticasFix";
        panelEstadisticasFix.Size = new Size(320, 425);
        panelEstadisticasFix.TabIndex = 18;
        panelEstadisticasFix.Visible = false;
        // 
        // menuPrincipal
        // 
        menuPrincipal.ImageScalingSize = new Size(20, 20);
        menuPrincipal.Items.AddRange(new ToolStripItem[] { menuArchivo, menuUtilidades });
        menuPrincipal.Location = new Point(0, 0);
        menuPrincipal.Name = "menuPrincipal";
        menuPrincipal.Size = new Size(1675, 33);
        menuPrincipal.TabIndex = 0;
        // 
        // menuArchivo
        // 
        menuArchivo.DropDownItems.AddRange(new ToolStripItem[] { menuItemSalir });
        menuArchivo.Name = "menuArchivo";
        menuArchivo.Size = new Size(88, 29);
        menuArchivo.Text = "Archivo";
        // 
        // menuItemSalir
        // 
        menuItemSalir.Name = "menuItemSalir";
        menuItemSalir.Size = new Size(147, 34);
        menuItemSalir.Text = "Salir";
        menuItemSalir.Click += MenuItemSalir_Click;
        // 
        // menuUtilidades
        // 
        this.menuItemWHDLoadTools = new System.Windows.Forms.ToolStripMenuItem();
        menuUtilidades.DropDownItems.AddRange(new ToolStripItem[] { menuItemBusquedaAvanzada, menuItemWHDLoadTools });
        menuUtilidades.Name = "menuUtilidades";
        menuUtilidades.Size = new Size(106, 29);
        menuUtilidades.Text = "Utilidades";
        // 
        // menuItemBusquedaAvanzada
        // 
        menuItemBusquedaAvanzada.Name = "menuItemBusquedaAvanzada";
        menuItemBusquedaAvanzada.ShortcutKeys = Keys.Control | Keys.F;
        menuItemBusquedaAvanzada.Size = new Size(334, 34);
        menuItemBusquedaAvanzada.Text = "Búsqueda Avanzada";
        menuItemBusquedaAvanzada.Click += MenuItemBusquedaAvanzada_Click;
        // 
        // menuItemWHDLoadTools
        // 
        menuItemWHDLoadTools.Name = "menuItemWHDLoadTools";
        menuItemWHDLoadTools.Size = new Size(334, 34);
        menuItemWHDLoadTools.Text = "WHDLoad Tools";
        menuItemWHDLoadTools.Click += MenuItemWHDLoadTools_Click;
        // 
        // progressBarPrincipal
        // 
        progressBarPrincipal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        progressBarPrincipal.Location = new Point(12, 1030);
        progressBarPrincipal.Name = "progressBarPrincipal";
        progressBarPrincipal.Size = new Size(800, 23);
        progressBarPrincipal.Style = ProgressBarStyle.Continuous;
        progressBarPrincipal.TabIndex = 16;
        progressBarPrincipal.Visible = false;
        // 
        // lblEstadoPrincipal
        // 
        lblEstadoPrincipal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblEstadoPrincipal.AutoSize = true;
        lblEstadoPrincipal.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblEstadoPrincipal.Location = new Point(12, 1010);
        lblEstadoPrincipal.Name = "lblEstadoPrincipal";
        lblEstadoPrincipal.Size = new Size(45, 20);
        lblEstadoPrincipal.TabIndex = 17;
        lblEstadoPrincipal.Text = "Estado";
        lblEstadoPrincipal.Visible = false;
        // 
        // FormPrincipal
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(1675, 1055);
        Controls.Add(progressBarPrincipal);
        Controls.Add(lblEstadoPrincipal);
        Controls.Add(menuPrincipal);
        Controls.Add(txtBusqueda);
        Controls.Add(cmbTitleCase);
        Controls.Add(lblTitleCase);
        Controls.Add(chkVerDesconocidos);
        Controls.Add(chkVerDuplicados);
        Controls.Add(chkNombresCortos);
        Controls.Add(chkMantenerDatos);
        Controls.Add(btnAyuda);
        Controls.Add(btnDeshacer);
        Controls.Add(btnLimpiarLista);
        Controls.Add(btnEtiquetaRapida);
        Controls.Add(btnGuardarCsv);
        Controls.Add(btnArreglarLista);
        Controls.Add(btnCargarCsv);
        Controls.Add(listaJuegos);
        Controls.Add(panelEstadisticas);
        Controls.Add(panelEstadisticasFix);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuPrincipal;
        Margin = new Padding(4, 5, 4, 5);
        MinimumSize = new Size(1299, 1028);
        Name = "FormPrincipal";
        Text = "IGame Tool";
        Load += FormPrincipal_Load;
        menuPrincipal.ResumeLayout(false);
        menuPrincipal.PerformLayout();
        ResumeLayout(false);
        PerformLayout();

    }

    private System.Windows.Forms.ListView listaJuegos;
    private System.Windows.Forms.ColumnHeader colNombre;
    private System.Windows.Forms.ColumnHeader colGenero;
    private System.Windows.Forms.ColumnHeader colSlave;
    private System.Windows.Forms.ColumnHeader colRuta;
    private System.Windows.Forms.ColumnHeader colPreview;
    private System.Windows.Forms.Button btnCargarCsv;
    private System.Windows.Forms.Button btnArreglarLista;
    private System.Windows.Forms.Button btnGuardarCsv;
    private System.Windows.Forms.Button btnEtiquetaRapida;
    private System.Windows.Forms.Button btnLimpiarLista;
    private System.Windows.Forms.Button btnDeshacer;
    private System.Windows.Forms.Button btnAyuda;
    private System.Windows.Forms.CheckBox chkMantenerDatos;
    private System.Windows.Forms.CheckBox chkNombresCortos;
    private System.Windows.Forms.CheckBox chkVerDuplicados;
    private System.Windows.Forms.CheckBox chkVerDesconocidos;
    private System.Windows.Forms.Label lblTitleCase;
    private System.Windows.Forms.ComboBox cmbTitleCase;
    private System.Windows.Forms.TextBox txtBusqueda;
    private IgameToolsWinForms.Controles.PanelEstadisticas panelEstadisticas;
    private IgameToolsWinForms.Controles.PanelEstadisticasFixList panelEstadisticasFix;
    private System.Windows.Forms.MenuStrip menuPrincipal;
    private System.Windows.Forms.ToolStripMenuItem menuArchivo;
    private System.Windows.Forms.ToolStripMenuItem menuUtilidades;
    private System.Windows.Forms.ToolStripMenuItem menuItemSalir;
    private System.Windows.Forms.ToolStripMenuItem menuItemBusquedaAvanzada;
    private System.Windows.Forms.ToolStripMenuItem menuItemWHDLoadTools;
    private System.Windows.Forms.ProgressBar progressBarPrincipal;
    private System.Windows.Forms.Label lblEstadoPrincipal;
}
