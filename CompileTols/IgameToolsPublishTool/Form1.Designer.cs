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
    private FlowLayoutPanel pnlAcciones;
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
        this.components = new System.ComponentModel.Container();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.menuAyuda = new System.Windows.Forms.ToolStripMenuItem();
        this.menuAyudaPasos = new System.Windows.Forms.ToolStripMenuItem();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.grpConfiguracion = new System.Windows.Forms.GroupBox();
        this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        this.lblRutaProyecto = new System.Windows.Forms.Label();
        this.txtRutaProyecto = new System.Windows.Forms.TextBox();
        this.btnSeleccionarRutaProyecto = new System.Windows.Forms.Button();
        this.lblVersion = new System.Windows.Forms.Label();
        this.txtVersion = new System.Windows.Forms.TextBox();
        this.chkLimpiarPublish = new System.Windows.Forms.CheckBox();
        this.lblRutaIscc = new System.Windows.Forms.Label();
        this.txtRutaIscc = new System.Windows.Forms.TextBox();
        this.btnBuscarIscc = new System.Windows.Forms.Button();
        this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
        this.btnLimpiarPublish = new System.Windows.Forms.Button();
        this.btnBuild = new System.Windows.Forms.Button();
        this.btnPublish = new System.Windows.Forms.Button();
        this.btnCopiarRecursos = new System.Windows.Forms.Button();
        this.btnZipPortable = new System.Windows.Forms.Button();
        this.btnZipSingleFile = new System.Windows.Forms.Button();
        this.btnInstalador = new System.Windows.Forms.Button();
        this.btnVerificarRecursos = new System.Windows.Forms.Button();
        this.btnFallbackLimpieza = new System.Windows.Forms.Button();
        this.btnEjecutarTodo = new System.Windows.Forms.Button();
        this.btnCancelar = new System.Windows.Forms.Button();
        this.grpLog = new System.Windows.Forms.GroupBox();
        this.txtLog = new System.Windows.Forms.RichTextBox();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.menuStrip1.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.grpConfiguracion.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.pnlAcciones.SuspendLayout();
        this.grpLog.SuspendLayout();
        this.SuspendLayout();
        // 
        // menuStrip1
        // 
        this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.menuAyuda});
        this.menuStrip1.Location = new System.Drawing.Point(10, 10);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(980, 28);
        this.menuStrip1.TabIndex = 0;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // menuAyuda
        // 
        this.menuAyuda.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.menuAyudaPasos});
        this.menuAyuda.Name = "menuAyuda";
        this.menuAyuda.Size = new System.Drawing.Size(65, 24);
        this.menuAyuda.Text = "Ayuda";
        // 
        // menuAyudaPasos
        // 
        this.menuAyudaPasos.Name = "menuAyudaPasos";
        this.menuAyudaPasos.Size = new System.Drawing.Size(224, 26);
        this.menuAyudaPasos.Text = "Guía paso a paso";
        this.menuAyudaPasos.Click += new System.EventHandler(this.menuAyudaPasos_Click);
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.ColumnCount = 1;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.Controls.Add(this.grpConfiguracion, 0, 0);
        this.tableLayoutPanel1.Controls.Add(this.grpLog, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 38);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 3;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
        this.tableLayoutPanel1.Size = new System.Drawing.Size(980, 592);
        this.tableLayoutPanel1.TabIndex = 1;
        // 
        // grpConfiguracion
        // 
        this.grpConfiguracion.Controls.Add(this.tableLayoutPanel2);
        this.grpConfiguracion.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grpConfiguracion.Location = new System.Drawing.Point(3, 3);
        this.grpConfiguracion.Name = "grpConfiguracion";
        this.grpConfiguracion.Padding = new System.Windows.Forms.Padding(10);
        this.grpConfiguracion.Size = new System.Drawing.Size(974, 184);
        this.grpConfiguracion.TabIndex = 0;
        this.grpConfiguracion.TabStop = false;
        this.grpConfiguracion.Text = "Configuración";
        // 
        // tableLayoutPanel2
        // 
        this.tableLayoutPanel2.ColumnCount = 3;
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
        this.tableLayoutPanel2.Controls.Add(this.lblRutaProyecto, 0, 0);
        this.tableLayoutPanel2.Controls.Add(this.txtRutaProyecto, 1, 0);
        this.tableLayoutPanel2.Controls.Add(this.btnSeleccionarRutaProyecto, 2, 0);
        this.tableLayoutPanel2.Controls.Add(this.lblVersion, 0, 1);
        this.tableLayoutPanel2.Controls.Add(this.txtVersion, 1, 1);
        this.tableLayoutPanel2.Controls.Add(this.chkLimpiarPublish, 2, 1);
        this.tableLayoutPanel2.Controls.Add(this.lblRutaIscc, 0, 2);
        this.tableLayoutPanel2.Controls.Add(this.txtRutaIscc, 1, 2);
        this.tableLayoutPanel2.Controls.Add(this.btnBuscarIscc, 2, 2);
        this.tableLayoutPanel2.Controls.Add(this.pnlAcciones, 0, 3);
        this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel2.Location = new System.Drawing.Point(10, 30);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 4;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel2.Size = new System.Drawing.Size(954, 144);
        this.tableLayoutPanel2.TabIndex = 0;
        // 
        // lblRutaProyecto
        // 
        this.lblRutaProyecto.AutoSize = true;
        this.lblRutaProyecto.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lblRutaProyecto.Location = new System.Drawing.Point(3, 0);
        this.lblRutaProyecto.Name = "lblRutaProyecto";
        this.lblRutaProyecto.Size = new System.Drawing.Size(114, 34);
        this.lblRutaProyecto.TabIndex = 0;
        this.lblRutaProyecto.Text = "Ruta proyecto";
        this.lblRutaProyecto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // txtRutaProyecto
        // 
        this.txtRutaProyecto.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtRutaProyecto.Location = new System.Drawing.Point(123, 3);
        this.txtRutaProyecto.Name = "txtRutaProyecto";
        this.txtRutaProyecto.Size = new System.Drawing.Size(688, 27);
        this.txtRutaProyecto.TabIndex = 1;
        // 
        // btnSeleccionarRutaProyecto
        // 
        this.btnSeleccionarRutaProyecto.Dock = System.Windows.Forms.DockStyle.Fill;
        this.btnSeleccionarRutaProyecto.Location = new System.Drawing.Point(817, 3);
        this.btnSeleccionarRutaProyecto.Name = "btnSeleccionarRutaProyecto";
        this.btnSeleccionarRutaProyecto.Size = new System.Drawing.Size(134, 28);
        this.btnSeleccionarRutaProyecto.TabIndex = 2;
        this.btnSeleccionarRutaProyecto.Text = "Seleccionar...";
        this.btnSeleccionarRutaProyecto.UseVisualStyleBackColor = true;
        this.btnSeleccionarRutaProyecto.Click += new System.EventHandler(this.btnSeleccionarRutaProyecto_Click);
        // 
        // lblVersion
        // 
        this.lblVersion.AutoSize = true;
        this.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lblVersion.Location = new System.Drawing.Point(3, 34);
        this.lblVersion.Name = "lblVersion";
        this.lblVersion.Size = new System.Drawing.Size(114, 34);
        this.lblVersion.TabIndex = 3;
        this.lblVersion.Text = "Versión";
        this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // txtVersion
        // 
        this.txtVersion.Location = new System.Drawing.Point(123, 37);
        this.txtVersion.Name = "txtVersion";
        this.txtVersion.Size = new System.Drawing.Size(180, 27);
        this.txtVersion.TabIndex = 4;
        // 
        // chkLimpiarPublish
        // 
        this.chkLimpiarPublish.AutoSize = true;
        this.chkLimpiarPublish.Dock = System.Windows.Forms.DockStyle.Fill;
        this.chkLimpiarPublish.Location = new System.Drawing.Point(817, 37);
        this.chkLimpiarPublish.Name = "chkLimpiarPublish";
        this.chkLimpiarPublish.Size = new System.Drawing.Size(134, 28);
        this.chkLimpiarPublish.TabIndex = 5;
        this.chkLimpiarPublish.Text = "Limpiar publish";
        this.chkLimpiarPublish.UseVisualStyleBackColor = true;
        // 
        // lblRutaIscc
        // 
        this.lblRutaIscc.AutoSize = true;
        this.lblRutaIscc.Dock = System.Windows.Forms.DockStyle.Fill;
        this.lblRutaIscc.Location = new System.Drawing.Point(3, 68);
        this.lblRutaIscc.Name = "lblRutaIscc";
        this.lblRutaIscc.Size = new System.Drawing.Size(114, 34);
        this.lblRutaIscc.TabIndex = 6;
        this.lblRutaIscc.Text = "ISCC.exe";
        this.lblRutaIscc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // txtRutaIscc
        // 
        this.txtRutaIscc.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtRutaIscc.Location = new System.Drawing.Point(123, 71);
        this.txtRutaIscc.Name = "txtRutaIscc";
        this.txtRutaIscc.Size = new System.Drawing.Size(688, 27);
        this.txtRutaIscc.TabIndex = 7;
        // 
        // btnBuscarIscc
        // 
        this.btnBuscarIscc.Dock = System.Windows.Forms.DockStyle.Fill;
        this.btnBuscarIscc.Location = new System.Drawing.Point(817, 71);
        this.btnBuscarIscc.Name = "btnBuscarIscc";
        this.btnBuscarIscc.Size = new System.Drawing.Size(134, 28);
        this.btnBuscarIscc.TabIndex = 8;
        this.btnBuscarIscc.Text = "Buscar...";
        this.btnBuscarIscc.UseVisualStyleBackColor = true;
        this.btnBuscarIscc.Click += new System.EventHandler(this.btnBuscarIscc_Click);
        // 
        // pnlAcciones
        // 
        this.tableLayoutPanel2.SetColumnSpan(this.pnlAcciones, 3);
        this.pnlAcciones.Controls.Add(this.btnLimpiarPublish);
        this.pnlAcciones.Controls.Add(this.btnBuild);
        this.pnlAcciones.Controls.Add(this.btnPublish);
        this.pnlAcciones.Controls.Add(this.btnCopiarRecursos);
        this.pnlAcciones.Controls.Add(this.btnInstalador);
        this.pnlAcciones.Controls.Add(this.btnZipPortable);
        this.pnlAcciones.Controls.Add(this.btnZipSingleFile);
        this.pnlAcciones.Controls.Add(this.btnVerificarRecursos);
        this.pnlAcciones.Controls.Add(this.btnFallbackLimpieza);
        this.pnlAcciones.Controls.Add(this.btnEjecutarTodo);
        this.pnlAcciones.Controls.Add(this.btnCancelar);
        this.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnlAcciones.Location = new System.Drawing.Point(3, 105);
        this.pnlAcciones.Name = "pnlAcciones";
        this.pnlAcciones.Size = new System.Drawing.Size(948, 36);
        this.pnlAcciones.TabIndex = 9;
        // 
        // btnLimpiarPublish
        // 
        this.btnLimpiarPublish.Location = new System.Drawing.Point(3, 3);
        this.btnLimpiarPublish.Name = "btnLimpiarPublish";
        this.btnLimpiarPublish.Size = new System.Drawing.Size(120, 29);
        this.btnLimpiarPublish.TabIndex = 0;
        this.btnLimpiarPublish.Text = "Limpiar publish";
        this.btnLimpiarPublish.UseVisualStyleBackColor = true;
        this.btnLimpiarPublish.Click += new System.EventHandler(this.btnLimpiarPublish_Click);
        // 
        // btnBuild
        // 
        this.btnBuild.Location = new System.Drawing.Point(129, 3);
        this.btnBuild.Name = "btnBuild";
        this.btnBuild.Size = new System.Drawing.Size(90, 29);
        this.btnBuild.TabIndex = 1;
        this.btnBuild.Text = "Build";
        this.btnBuild.UseVisualStyleBackColor = true;
        this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
        // 
        // btnPublish
        // 
        this.btnPublish.Location = new System.Drawing.Point(225, 3);
        this.btnPublish.Name = "btnPublish";
        this.btnPublish.Size = new System.Drawing.Size(90, 29);
        this.btnPublish.TabIndex = 2;
        this.btnPublish.Text = "Publish";
        this.btnPublish.UseVisualStyleBackColor = true;
        this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
        // 
        // btnCopiarRecursos
        // 
        this.btnCopiarRecursos.Location = new System.Drawing.Point(321, 3);
        this.btnCopiarRecursos.Name = "btnCopiarRecursos";
        this.btnCopiarRecursos.Size = new System.Drawing.Size(130, 29);
        this.btnCopiarRecursos.TabIndex = 3;
        this.btnCopiarRecursos.Text = "Copiar recursos";
        this.btnCopiarRecursos.UseVisualStyleBackColor = true;
        this.btnCopiarRecursos.Click += new System.EventHandler(this.btnCopiarRecursos_Click);
        // 
        // btnZipPortable
        // 
        this.btnZipPortable.Location = new System.Drawing.Point(457, 3);
        this.btnZipPortable.Name = "btnZipPortable";
        this.btnZipPortable.Size = new System.Drawing.Size(110, 29);
        this.btnZipPortable.TabIndex = 4;
        this.btnZipPortable.Text = "ZIP portable";
        this.btnZipPortable.UseVisualStyleBackColor = true;
        this.btnZipPortable.Click += new System.EventHandler(this.btnZipPortable_Click);
        // 
        // btnZipSingleFile
        // 
        this.btnZipSingleFile.Location = new System.Drawing.Point(573, 3);
        this.btnZipSingleFile.Name = "btnZipSingleFile";
        this.btnZipSingleFile.Size = new System.Drawing.Size(122, 29);
        this.btnZipSingleFile.TabIndex = 5;
        this.btnZipSingleFile.Text = "ZIP single-file";
        this.btnZipSingleFile.UseVisualStyleBackColor = true;
        this.btnZipSingleFile.Click += new System.EventHandler(this.btnZipSingleFile_Click);
        // 
        // btnInstalador
        // 
        this.btnInstalador.Location = new System.Drawing.Point(701, 3);
        this.btnInstalador.Name = "btnInstalador";
        this.btnInstalador.Size = new System.Drawing.Size(90, 29);
        this.btnInstalador.TabIndex = 6;
        this.btnInstalador.Text = "Instalador";
        this.btnInstalador.UseVisualStyleBackColor = true;
        this.btnInstalador.Click += new System.EventHandler(this.btnInstalador_Click);
        // 
        // btnVerificarRecursos
        // 
        this.btnVerificarRecursos.Location = new System.Drawing.Point(1041, 3);
        this.btnVerificarRecursos.Name = "btnVerificarRecursos";
        this.btnVerificarRecursos.Size = new System.Drawing.Size(145, 29);
        this.btnVerificarRecursos.TabIndex = 9;
        this.btnVerificarRecursos.Text = "Verificar recursos";
        this.btnVerificarRecursos.UseVisualStyleBackColor = true;
        this.btnVerificarRecursos.Click += new System.EventHandler(this.btnVerificarRecursos_Click);
        // 
        // btnFallbackLimpieza
        // 
        this.btnFallbackLimpieza.Location = new System.Drawing.Point(1192, 3);
        this.btnFallbackLimpieza.Name = "btnFallbackLimpieza";
        this.btnFallbackLimpieza.Size = new System.Drawing.Size(155, 29);
        this.btnFallbackLimpieza.TabIndex = 10;
        this.btnFallbackLimpieza.Text = "Fallback limpieza";
        this.btnFallbackLimpieza.UseVisualStyleBackColor = true;
        this.btnFallbackLimpieza.Click += new System.EventHandler(this.btnFallbackLimpieza_Click);
        // 
        // btnEjecutarTodo
        // 
        this.btnEjecutarTodo.Location = new System.Drawing.Point(1353, 3);
        this.btnEjecutarTodo.Name = "btnEjecutarTodo";
        this.btnEjecutarTodo.Size = new System.Drawing.Size(120, 29);
        this.btnEjecutarTodo.TabIndex = 11;
        this.btnEjecutarTodo.Text = "Ejecutar todo";
        this.btnEjecutarTodo.UseVisualStyleBackColor = true;
        this.btnEjecutarTodo.Click += new System.EventHandler(this.btnEjecutarTodo_Click);
        // 
        // btnCancelar
        // 
        this.btnCancelar.Enabled = false;
        this.btnCancelar.Location = new System.Drawing.Point(1479, 3);
        this.btnCancelar.Name = "btnCancelar";
        this.btnCancelar.Size = new System.Drawing.Size(90, 29);
        this.btnCancelar.TabIndex = 12;
        this.btnCancelar.Text = "Cancelar";
        this.btnCancelar.UseVisualStyleBackColor = true;
        this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
        // 
        // grpLog
        // 
        this.grpLog.Controls.Add(this.txtLog);
        this.grpLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.grpLog.Location = new System.Drawing.Point(3, 193);
        this.grpLog.Name = "grpLog";
        this.grpLog.Padding = new System.Windows.Forms.Padding(10);
        this.grpLog.Size = new System.Drawing.Size(974, 398);
        this.grpLog.TabIndex = 1;
        this.grpLog.TabStop = false;
        this.grpLog.Text = "Log";
        // 
        // txtLog
        // 
        this.txtLog.BackColor = System.Drawing.Color.Black;
        this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.txtLog.ForeColor = System.Drawing.Color.Gainsboro;
        this.txtLog.Location = new System.Drawing.Point(10, 30);
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.Size = new System.Drawing.Size(954, 358);
        this.txtLog.TabIndex = 0;
        this.txtLog.Text = "";
        // 
        // progressBar1
        // 
        this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.progressBar1.Location = new System.Drawing.Point(3, 597);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(974, 20);
        this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
        this.progressBar1.TabIndex = 2;
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1000, 640);
        this.Controls.Add(this.tableLayoutPanel1);
        this.Controls.Add(this.menuStrip1);
        this.MainMenuStrip = this.menuStrip1;
        this.MinimumSize = new System.Drawing.Size(900, 600);
        this.Name = "Form1";
        this.Padding = new System.Windows.Forms.Padding(10);
        this.Text = "IgameTools - Publicación (Publish/ZIP/Instalador)";
        this.Load += new System.EventHandler(this.Form1_Load);
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.grpConfiguracion.ResumeLayout(false);
        this.tableLayoutPanel2.ResumeLayout(false);
        this.tableLayoutPanel2.PerformLayout();
        this.pnlAcciones.ResumeLayout(false);
        this.grpLog.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
}
