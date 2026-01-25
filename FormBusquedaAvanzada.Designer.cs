namespace IgameToolsWinForms
{
    partial class FormBusquedaAvanzada
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBusquedaAvanzada));
            grpCriteriosBusqueda = new GroupBox();
            lblCampo = new Label();
            cmbCampo = new ComboBox();
            lblTipo = new Label();
            cmbTipo = new ComboBox();
            lblTermino = new Label();
            txtTermino = new TextBox();
            chkMayusculas = new CheckBox();
            chkRegex = new CheckBox();
            btnBuscar = new Button();
            btnLimpiar = new Button();
            btnSiguiente = new Button();
            lblResultados = new Label();
            lstResultados = new ListView();
            colNombre = new ColumnHeader();
            colGenero = new ColumnHeader();
            colSlave = new ColumnHeader();
            colRuta = new ColumnHeader();
            btnAnterior = new Button();
            btnSiguienteNav = new Button();
            btnSeleccionar = new Button();
            btnCerrar = new Button();
            grpCriteriosBusqueda.SuspendLayout();
            SuspendLayout();
            // 
            // grpCriteriosBusqueda
            // 
            grpCriteriosBusqueda.Controls.Add(lblCampo);
            grpCriteriosBusqueda.Controls.Add(cmbCampo);
            grpCriteriosBusqueda.Controls.Add(lblTipo);
            grpCriteriosBusqueda.Controls.Add(cmbTipo);
            grpCriteriosBusqueda.Controls.Add(lblTermino);
            grpCriteriosBusqueda.Controls.Add(txtTermino);
            grpCriteriosBusqueda.Controls.Add(chkMayusculas);
            grpCriteriosBusqueda.Controls.Add(chkRegex);
            grpCriteriosBusqueda.Controls.Add(btnBuscar);
            grpCriteriosBusqueda.Controls.Add(btnLimpiar);
            grpCriteriosBusqueda.Controls.Add(btnSiguiente);
            grpCriteriosBusqueda.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpCriteriosBusqueda.Location = new Point(12, 12);
            grpCriteriosBusqueda.Name = "grpCriteriosBusqueda";
            grpCriteriosBusqueda.Size = new Size(760, 140);
            grpCriteriosBusqueda.TabIndex = 0;
            grpCriteriosBusqueda.TabStop = false;
            grpCriteriosBusqueda.Text = "🔍 Criterios de Búsqueda";
            // 
            // lblCampo
            // 
            lblCampo.AutoSize = true;
            lblCampo.Location = new Point(15, 30);
            lblCampo.Name = "lblCampo";
            lblCampo.Size = new Size(76, 25);
            lblCampo.TabIndex = 0;
            lblCampo.Text = "Campo:";
            // 
            // cmbCampo
            // 
            cmbCampo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCampo.FormattingEnabled = true;
            cmbCampo.Items.AddRange(new object[] { "Nombre", "Nombre Corto", "Género", "Slave", "Ruta", "Todos" });
            cmbCampo.Location = new Point(101, 28);
            cmbCampo.Name = "cmbCampo";
            cmbCampo.Size = new Size(120, 33);
            cmbCampo.TabIndex = 1;
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(241, 30);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(55, 25);
            lblTipo.TabIndex = 2;
            lblTipo.Text = "Tipo:";
            // 
            // cmbTipo
            // 
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.FormattingEnabled = true;
            cmbTipo.Items.AddRange(new object[] { "Contiene", "Exacto", "Comienza con", "Termina con", "Regex", "No contiene" });
            cmbTipo.Location = new Point(306, 28);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.Size = new Size(150, 33);
            cmbTipo.TabIndex = 3;
            // 
            // lblTermino
            // 
            lblTermino.AutoSize = true;
            lblTermino.Location = new Point(15, 70);
            lblTermino.Name = "lblTermino";
            lblTermino.Size = new Size(86, 25);
            lblTermino.TabIndex = 4;
            lblTermino.Text = "Término:";
            // 
            // txtTermino
            // 
            txtTermino.Location = new Point(101, 68);
            txtTermino.Name = "txtTermino";
            txtTermino.PlaceholderText = "Ingrese el término de búsqueda...";
            txtTermino.Size = new Size(355, 31);
            txtTermino.TabIndex = 5;
            // 
            // chkMayusculas
            // 
            chkMayusculas.AutoSize = true;
            chkMayusculas.Location = new Point(15, 100);
            chkMayusculas.Name = "chkMayusculas";
            chkMayusculas.Size = new Size(224, 29);
            chkMayusculas.TabIndex = 6;
            chkMayusculas.Text = "Distinguir mayúsculas";
            chkMayusculas.UseVisualStyleBackColor = true;
            // 
            // chkRegex
            // 
            chkRegex.AutoSize = true;
            chkRegex.Enabled = false;
            chkRegex.Location = new Point(170, 100);
            chkRegex.Name = "chkRegex";
            chkRegex.Size = new Size(266, 29);
            chkRegex.TabIndex = 7;
            chkRegex.Text = "Usar expresiones regulares";
            chkRegex.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(593, 30);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(140, 31);
            btnBuscar.TabIndex = 8;
            btnBuscar.Text = "🔍 Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(593, 64);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(140, 31);
            btnLimpiar.TabIndex = 9;
            btnLimpiar.Text = "🗑️ Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            // 
            // btnSiguiente
            // 
            btnSiguiente.Enabled = false;
            btnSiguiente.Location = new Point(593, 98);
            btnSiguiente.Name = "btnSiguiente";
            btnSiguiente.Size = new Size(140, 36);
            btnSiguiente.TabIndex = 10;
            btnSiguiente.Text = "Siguiente ➡️";
            btnSiguiente.UseVisualStyleBackColor = true;
            // 
            // lblResultados
            // 
            lblResultados.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResultados.Location = new Point(12, 160);
            lblResultados.Name = "lblResultados";
            lblResultados.Size = new Size(200, 23);
            lblResultados.TabIndex = 11;
            lblResultados.Text = "📋 Resultados: 0 encontrados";
            // 
            // lstResultados
            // 
            lstResultados.Font = new Font("Segoe UI", 8F);
            lstResultados.FullRowSelect = true;
            lstResultados.GridLines = true;
            lstResultados.Location = new Point(12, 190);
            lstResultados.MultiSelect = false;
            lstResultados.Name = "lstResultados";
            lstResultados.Size = new Size(760, 350);
            lstResultados.TabIndex = 12;
            lstResultados.UseCompatibleStateImageBehavior = false;
            lstResultados.View = View.Details;
            // 
            // colNombre
            // 
            colNombre.Text = "Nombre";
            colNombre.Width = 250;
            // 
            // colGenero
            // 
            colGenero.Text = "Género";
            colGenero.Width = 150;
            // 
            // colSlave
            // 
            colSlave.Text = "Slave";
            colSlave.Width = 200;
            // 
            // colRuta
            // 
            colRuta.Text = "Ruta";
            colRuta.Width = 150;
            // 
            // btnAnterior
            // 
            btnAnterior.Enabled = false;
            btnAnterior.Location = new Point(12, 550);
            btnAnterior.Name = "btnAnterior";
            btnAnterior.Size = new Size(118, 45);
            btnAnterior.TabIndex = 13;
            btnAnterior.Text = "⬅️ Anterior";
            btnAnterior.UseVisualStyleBackColor = true;
            // 
            // btnSiguienteNav
            // 
            btnSiguienteNav.Enabled = false;
            btnSiguienteNav.Location = new Point(136, 550);
            btnSiguienteNav.Name = "btnSiguienteNav";
            btnSiguienteNav.Size = new Size(133, 45);
            btnSiguienteNav.TabIndex = 14;
            btnSiguienteNav.Text = "Siguiente ➡️";
            btnSiguienteNav.UseVisualStyleBackColor = true;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Enabled = false;
            btnSeleccionar.Location = new Point(495, 550);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(144, 45);
            btnSeleccionar.TabIndex = 15;
            btnSeleccionar.Text = "✅ Seleccionar";
            btnSeleccionar.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            btnCerrar.Location = new Point(645, 550);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(125, 45);
            btnCerrar.TabIndex = 16;
            btnCerrar.Text = "❌ Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // FormBusquedaAvanzada
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(800, 607);
            Controls.Add(btnCerrar);
            Controls.Add(btnSeleccionar);
            Controls.Add(btnSiguienteNav);
            Controls.Add(btnAnterior);
            Controls.Add(lstResultados);
            Controls.Add(lblResultados);
            Controls.Add(grpCriteriosBusqueda);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormBusquedaAvanzada";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Búsqueda Avanzada de Juegos";
            grpCriteriosBusqueda.ResumeLayout(false);
            grpCriteriosBusqueda.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCriteriosBusqueda;
        private System.Windows.Forms.Label lblCampo;
        private System.Windows.Forms.ComboBox cmbCampo;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblTermino;
        private System.Windows.Forms.TextBox txtTermino;
        private System.Windows.Forms.CheckBox chkMayusculas;
        private System.Windows.Forms.CheckBox chkRegex;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Label lblResultados;
        private System.Windows.Forms.ListView lstResultados;
        private System.Windows.Forms.ColumnHeader colNombre;
        private System.Windows.Forms.ColumnHeader colGenero;
        private System.Windows.Forms.ColumnHeader colSlave;
        private System.Windows.Forms.ColumnHeader colRuta;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguienteNav;
        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.Button btnCerrar;
    }
}
