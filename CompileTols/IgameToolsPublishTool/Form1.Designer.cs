namespace IgameToolsPublishTool;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private TableLayoutPanel tableLayoutPanel1;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem menuAyuda;
    private ToolStripMenuItem menuAyudaPasos;
    private GroupBox grpConfiguracion;
    private TableLayoutPanel tableLayoutPanel2;
    private Label lblRutaProyecto;
    private TextBox txtRutaProyecto;
    private Button btnSeleccionarRutaProyecto;
    private Label lblVersion;
    private TextBox txtVersion;
    private CheckBox chkLimpiarPublish;
    private Label lblRutaIscc;
    private TextBox txtRutaIscc;
    private Button btnBuscarIscc;
    private Panel pnlAcciones;
    private Button btnLimpiarPublish;
    private Button btnBuild;
    private Button btnPublish;
    private Button btnCopiarRecursos;
    private Button btnInstalador;
    private Button btnZipPortable;
    private Button btnZipSingleFile;
    private Button btnVerificarRecursos;
    private Button btnFallbackLimpieza;
    private Button btnEjecutarTodo;
    private Button btnCancelar;
    private GroupBox grpLog;
    private RichTextBox txtLog;
    private ProgressBar progressBar1;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip();
        menuAyuda = new ToolStripMenuItem();
        menuAyudaPasos = new ToolStripMenuItem();
        tableLayoutPanel1 = new TableLayoutPanel();
        grpConfiguracion = new GroupBox();
        tableLayoutPanel2 = new TableLayoutPanel();
        txtRutaProyecto = new TextBox();
        btnSeleccionarRutaProyecto = new Button();
        txtVersion = new TextBox();
        chkLimpiarPublish = new CheckBox();
        lblRutaIscc = new Label();
        txtRutaIscc = new TextBox();
        btnBuscarIscc = new Button();
        pnlAcciones = new Panel();
        btnLimpiarPublish = new Button();
        btnBuild = new Button();
        btnPublish = new Button();
        btnCopiarRecursos = new Button();
        btnInstalador = new Button();
        btnZipPortable = new Button();
        btnZipSingleFile = new Button();
        btnVerificarRecursos = new Button();
        btnFallbackLimpieza = new Button();
        btnEjecutarTodo = new Button();
        btnCancelar = new Button();
        lblVersion = new Label();
        lblRutaProyecto = new Label();
        grpLog = new GroupBox();
        txtLog = new RichTextBox();
        progressBar1 = new ProgressBar();
        menuStrip1.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        grpConfiguracion.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        pnlAcciones.SuspendLayout();
        grpLog.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new Size(20, 20);
        menuStrip1.Items.AddRange(new ToolStripItem[] { menuAyuda });
        menuStrip1.Location = new Point(10, 10);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(980, 33);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // menuAyuda
        // 
        menuAyuda.DropDownItems.AddRange(new ToolStripItem[] { menuAyudaPasos });
        menuAyuda.Name = "menuAyuda";
        menuAyuda.Size = new Size(79, 29);
        menuAyuda.Text = "Ayuda";
        // 
        // menuAyudaPasos
        // 
        menuAyudaPasos.Name = "menuAyudaPasos";
        menuAyudaPasos.Size = new Size(251, 34);
        menuAyudaPasos.Text = "Guía paso a paso";
        menuAyudaPasos.Click += menuAyudaPasos_Click;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(grpConfiguracion, 0, 0);
        tableLayoutPanel1.Controls.Add(grpLog, 0, 1);
        tableLayoutPanel1.Controls.Add(progressBar1, 0, 2);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(10, 43);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tableLayoutPanel1.Size = new Size(980, 675);
        tableLayoutPanel1.TabIndex = 1;
        // 
        // grpConfiguracion
        // 
        grpConfiguracion.Controls.Add(tableLayoutPanel2);
        grpConfiguracion.Dock = DockStyle.Fill;
        grpConfiguracion.Location = new Point(3, 3);
        grpConfiguracion.Name = "grpConfiguracion";
        grpConfiguracion.Padding = new Padding(10);
        grpConfiguracion.Size = new Size(974, 224);
        grpConfiguracion.TabIndex = 0;
        grpConfiguracion.TabStop = false;
        grpConfiguracion.Text = "Configuración";
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 3;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 168F));
        tableLayoutPanel2.Controls.Add(txtRutaProyecto, 1, 0);
        tableLayoutPanel2.Controls.Add(btnSeleccionarRutaProyecto, 2, 0);
        tableLayoutPanel2.Controls.Add(txtVersion, 1, 1);
        tableLayoutPanel2.Controls.Add(chkLimpiarPublish, 2, 1);
        tableLayoutPanel2.Controls.Add(lblRutaIscc, 0, 2);
        tableLayoutPanel2.Controls.Add(txtRutaIscc, 1, 2);
        tableLayoutPanel2.Controls.Add(btnBuscarIscc, 2, 2);
        tableLayoutPanel2.Controls.Add(pnlAcciones, 0, 3);
        tableLayoutPanel2.Controls.Add(lblVersion, 0, 1);
        tableLayoutPanel2.Controls.Add(lblRutaProyecto, 0, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(10, 34);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 4;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
        tableLayoutPanel2.Size = new Size(954, 180);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // txtRutaProyecto
        // 
        txtRutaProyecto.Dock = DockStyle.Fill;
        txtRutaProyecto.Location = new Point(133, 3);
        txtRutaProyecto.Name = "txtRutaProyecto";
        txtRutaProyecto.Size = new Size(650, 31);
        txtRutaProyecto.TabIndex = 1;
        // 
        // btnSeleccionarRutaProyecto
        // 
        btnSeleccionarRutaProyecto.Anchor = AnchorStyles.None;
        btnSeleccionarRutaProyecto.Location = new Point(800, 3);
        btnSeleccionarRutaProyecto.Name = "btnSeleccionarRutaProyecto";
        btnSeleccionarRutaProyecto.Size = new Size(139, 29);
        btnSeleccionarRutaProyecto.TabIndex = 2;
        btnSeleccionarRutaProyecto.Text = "Seleccionar...";
        btnSeleccionarRutaProyecto.UseVisualStyleBackColor = true;
        btnSeleccionarRutaProyecto.Click += btnSeleccionarRutaProyecto_Click;
        // 
        // txtVersion
        // 
        txtVersion.Location = new Point(133, 38);
        txtVersion.Name = "txtVersion";
        txtVersion.Size = new Size(180, 31);
        txtVersion.TabIndex = 4;
        // 
        // chkLimpiarPublish
        // 
        chkLimpiarPublish.AutoSize = true;
        chkLimpiarPublish.Dock = DockStyle.Fill;
        chkLimpiarPublish.Location = new Point(789, 38);
        chkLimpiarPublish.Name = "chkLimpiarPublish";
        chkLimpiarPublish.Size = new Size(162, 31);
        chkLimpiarPublish.TabIndex = 5;
        chkLimpiarPublish.Text = "Limpiar publish";
        chkLimpiarPublish.UseVisualStyleBackColor = true;
        // 
        // lblRutaIscc
        // 
        lblRutaIscc.AutoSize = true;
        lblRutaIscc.Dock = DockStyle.Fill;
        lblRutaIscc.Location = new Point(3, 72);
        lblRutaIscc.Name = "lblRutaIscc";
        lblRutaIscc.Size = new Size(124, 34);
        lblRutaIscc.TabIndex = 6;
        lblRutaIscc.Text = "ISCC.exe";
        lblRutaIscc.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtRutaIscc
        // 
        txtRutaIscc.Dock = DockStyle.Fill;
        txtRutaIscc.Location = new Point(133, 75);
        txtRutaIscc.Name = "txtRutaIscc";
        txtRutaIscc.Size = new Size(650, 31);
        txtRutaIscc.TabIndex = 7;
        // 
        // btnBuscarIscc
        // 
        btnBuscarIscc.Dock = DockStyle.Fill;
        btnBuscarIscc.Location = new Point(789, 75);
        btnBuscarIscc.Name = "btnBuscarIscc";
        btnBuscarIscc.Size = new Size(162, 28);
        btnBuscarIscc.TabIndex = 8;
        btnBuscarIscc.Text = "Buscar...";
        btnBuscarIscc.UseVisualStyleBackColor = true;
        btnBuscarIscc.Click += btnBuscarIscc_Click;
        // 
        // pnlAcciones
        // 
        tableLayoutPanel2.SetColumnSpan(pnlAcciones, 3);
        pnlAcciones.Controls.Add(btnLimpiarPublish);
        pnlAcciones.Controls.Add(btnBuild);
        pnlAcciones.Controls.Add(btnPublish);
        pnlAcciones.Controls.Add(btnCopiarRecursos);
        pnlAcciones.Controls.Add(btnInstalador);
        pnlAcciones.Controls.Add(btnZipPortable);
        pnlAcciones.Controls.Add(btnZipSingleFile);
        pnlAcciones.Controls.Add(btnVerificarRecursos);
        pnlAcciones.Controls.Add(btnFallbackLimpieza);
        pnlAcciones.Controls.Add(btnEjecutarTodo);
        pnlAcciones.Controls.Add(btnCancelar);
        pnlAcciones.AutoScroll = true;
        pnlAcciones.BorderStyle = BorderStyle.FixedSingle;
        pnlAcciones.Dock = DockStyle.Fill;
        pnlAcciones.Location = new Point(3, 109);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Size = new Size(948, 68);
        pnlAcciones.TabIndex = 9;
        // 
        // btnLimpiarPublish
        // 
        btnLimpiarPublish.Location = new Point(3, 3);
        btnLimpiarPublish.Name = "btnLimpiarPublish";
        btnLimpiarPublish.Size = new Size(155, 33);
        btnLimpiarPublish.TabIndex = 0;
        btnLimpiarPublish.Text = "Limpiar publish";
        btnLimpiarPublish.UseVisualStyleBackColor = true;
        btnLimpiarPublish.Click += btnLimpiarPublish_Click;
        // 
        // btnBuild
        // 
        btnBuild.Location = new Point(164, 3);
        btnBuild.Name = "btnBuild";
        btnBuild.Size = new Size(90, 33);
        btnBuild.TabIndex = 1;
        btnBuild.Text = "Build";
        btnBuild.UseVisualStyleBackColor = true;
        btnBuild.Click += btnBuild_Click;
        // 
        // btnPublish
        // 
        btnPublish.Location = new Point(260, 3);
        btnPublish.Name = "btnPublish";
        btnPublish.Size = new Size(90, 33);
        btnPublish.TabIndex = 2;
        btnPublish.Text = "Publish";
        btnPublish.UseVisualStyleBackColor = true;
        btnPublish.Click += btnPublish_Click;
        // 
        // btnCopiarRecursos
        // 
        btnCopiarRecursos.Location = new Point(356, 3);
        btnCopiarRecursos.Name = "btnCopiarRecursos";
        btnCopiarRecursos.Size = new Size(147, 33);
        btnCopiarRecursos.TabIndex = 3;
        btnCopiarRecursos.Text = "Copiar recursos";
        btnCopiarRecursos.UseVisualStyleBackColor = true;
        btnCopiarRecursos.Click += btnCopiarRecursos_Click;
        // 
        // btnInstalador
        // 
        btnInstalador.Location = new Point(509, 3);
        btnInstalador.Name = "btnInstalador";
        btnInstalador.Size = new Size(101, 33);
        btnInstalador.TabIndex = 6;
        btnInstalador.Text = "Instalador";
        btnInstalador.UseVisualStyleBackColor = true;
        btnInstalador.Click += btnInstalador_Click;
        // 
        // btnZipPortable
        // 
        btnZipPortable.Location = new Point(616, 3);
        btnZipPortable.Name = "btnZipPortable";
        btnZipPortable.Size = new Size(121, 33);
        btnZipPortable.TabIndex = 4;
        btnZipPortable.Text = "ZIP portable";
        btnZipPortable.UseVisualStyleBackColor = true;
        btnZipPortable.Click += btnZipPortable_Click;
        // 
        // btnZipSingleFile
        // 
        btnZipSingleFile.Location = new Point(743, 3);
        btnZipSingleFile.Name = "btnZipSingleFile";
        btnZipSingleFile.Size = new Size(132, 33);
        btnZipSingleFile.TabIndex = 5;
        btnZipSingleFile.Text = "ZIP single-file";
        btnZipSingleFile.UseVisualStyleBackColor = true;
        btnZipSingleFile.Click += btnZipSingleFile_Click;
        // 
        // btnVerificarRecursos
        // 
        btnVerificarRecursos.Location = new Point(3, 36);
        btnVerificarRecursos.Name = "btnVerificarRecursos";
        btnVerificarRecursos.Size = new Size(155, 33);
        btnVerificarRecursos.TabIndex = 9;
        btnVerificarRecursos.Text = "Verificar recursos";
        btnVerificarRecursos.UseVisualStyleBackColor = true;
        btnVerificarRecursos.Click += btnVerificarRecursos_Click;
        // 
        // btnFallbackLimpieza
        // 
        btnFallbackLimpieza.Location = new Point(175, 34);
        btnFallbackLimpieza.Name = "btnFallbackLimpieza";
        btnFallbackLimpieza.Size = new Size(155, 33);
        btnFallbackLimpieza.TabIndex = 10;
        btnFallbackLimpieza.Text = "Fallback limpieza";
        btnFallbackLimpieza.UseVisualStyleBackColor = true;
        btnFallbackLimpieza.Click += btnFallbackLimpieza_Click;
        // 
        // btnEjecutarTodo
        // 
        btnEjecutarTodo.Location = new Point(356, 35);
        btnEjecutarTodo.Name = "btnEjecutarTodo";
        btnEjecutarTodo.Size = new Size(127, 33);
        btnEjecutarTodo.TabIndex = 11;
        btnEjecutarTodo.Text = "Ejecutar todo";
        btnEjecutarTodo.UseVisualStyleBackColor = true;
        btnEjecutarTodo.Click += btnEjecutarTodo_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.BackColor = Color.Blue;
        btnCancelar.Enabled = false;
        btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCancelar.ForeColor = Color.White;
        btnCancelar.Location = new Point(790, 36);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(155, 33);
        btnCancelar.TabIndex = 12;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = false;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // lblVersion
        // 
        lblVersion.AutoSize = true;
        lblVersion.Dock = DockStyle.Fill;
        lblVersion.Location = new Point(3, 35);
        lblVersion.Name = "lblVersion";
        lblVersion.Size = new Size(124, 37);
        lblVersion.TabIndex = 3;
        lblVersion.Text = "Versión";
        lblVersion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblRutaProyecto
        // 
        lblRutaProyecto.AutoSize = true;
        lblRutaProyecto.Dock = DockStyle.Fill;
        lblRutaProyecto.Location = new Point(3, 0);
        lblRutaProyecto.Name = "lblRutaProyecto";
        lblRutaProyecto.Size = new Size(124, 35);
        lblRutaProyecto.TabIndex = 0;
        lblRutaProyecto.Text = "Ruta proyecto";
        lblRutaProyecto.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // grpLog
        // 
        grpLog.Controls.Add(txtLog);
        grpLog.Dock = DockStyle.Fill;
        grpLog.Location = new Point(3, 233);
        grpLog.Name = "grpLog";
        grpLog.Padding = new Padding(10);
        grpLog.Size = new Size(974, 413);
        grpLog.TabIndex = 1;
        grpLog.TabStop = false;
        grpLog.Text = "Log";
        // 
        // txtLog
        // 
        txtLog.BackColor = Color.Black;
        txtLog.Dock = DockStyle.Fill;
        txtLog.Font = new Font("Consolas", 9F);
        txtLog.ForeColor = Color.Gainsboro;
        txtLog.Location = new Point(10, 34);
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.Size = new Size(954, 369);
        txtLog.TabIndex = 0;
        txtLog.Text = "";
        // 
        // progressBar1
        // 
        progressBar1.Dock = DockStyle.Fill;
        progressBar1.Location = new Point(3, 652);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(974, 20);
        progressBar1.TabIndex = 2;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 728);
        Controls.Add(tableLayoutPanel1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        MinimumSize = new Size(900, 600);
        Name = "Form1";
        Padding = new Padding(10);
        Text = "IgameTools - Publicación (Publish/ZIP/Instalador)";
        Load += Form1_Load;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        tableLayoutPanel1.ResumeLayout(false);
        grpConfiguracion.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        pnlAcciones.ResumeLayout(false);
        grpLog.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
