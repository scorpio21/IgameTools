namespace IgameToolsWinForms
{
    partial class FormDialogoGuardarCsv
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDialogoGuardarCsv));
            lblInstruccion = new Label();
            txtNombreArchivo = new TextBox();
            txtRutaCompleta = new TextBox();
            cmbCarpetasRapidas = new ComboBox();
            lstArchivos = new ListBox();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // lblInstruccion
            // 
            lblInstruccion.AutoSize = true;
            lblInstruccion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblInstruccion.Location = new Point(29, 31);
            lblInstruccion.Margin = new Padding(4, 0, 4, 0);
            lblInstruccion.Name = "lblInstruccion";
            lblInstruccion.Size = new Size(269, 28);
            lblInstruccion.TabIndex = 0;
            lblInstruccion.Text = "Guardar archivo CSV como:";
            // 
            // txtNombreArchivo
            // 
            txtNombreArchivo.Font = new Font("Segoe UI", 9F);
            txtNombreArchivo.Location = new Point(29, 86);
            txtNombreArchivo.Margin = new Padding(4, 5, 4, 5);
            txtNombreArchivo.Name = "txtNombreArchivo";
            txtNombreArchivo.Size = new Size(1070, 31);
            txtNombreArchivo.TabIndex = 1;
            txtNombreArchivo.Text = "gameslist.csv";
            // 
            // txtRutaCompleta
            // 
            txtRutaCompleta.Font = new Font("Segoe UI", 9F);
            txtRutaCompleta.Location = new Point(29, 148);
            txtRutaCompleta.Margin = new Padding(4, 5, 4, 5);
            txtRutaCompleta.Name = "txtRutaCompleta";
            txtRutaCompleta.ReadOnly = true;
            txtRutaCompleta.Size = new Size(1070, 31);
            txtRutaCompleta.TabIndex = 2;
            // 
            // cmbCarpetasRapidas
            // 
            cmbCarpetasRapidas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCarpetasRapidas.Font = new Font("Segoe UI", 9F);
            cmbCarpetasRapidas.Location = new Point(29, 203);
            cmbCarpetasRapidas.Margin = new Padding(4, 5, 4, 5);
            cmbCarpetasRapidas.Name = "cmbCarpetasRapidas";
            cmbCarpetasRapidas.Size = new Size(1070, 33);
            cmbCarpetasRapidas.TabIndex = 3;
            // 
            // lstArchivos
            // 
            lstArchivos.Font = new Font("Segoe UI", 9F);
            lstArchivos.FormattingEnabled = true;
            lstArchivos.Location = new Point(29, 258);
            lstArchivos.Margin = new Padding(4, 5, 4, 5);
            lstArchivos.Name = "lstArchivos";
            lstArchivos.Size = new Size(1070, 629);
            lstArchivos.TabIndex = 4;
            // 
            // btnAceptar
            // 
            btnAceptar.Font = new Font("Segoe UI", 9F);
            btnAceptar.Location = new Point(29, 914);
            btnAceptar.Margin = new Padding(4, 5, 4, 5);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(143, 62);
            btnAceptar.TabIndex = 5;
            btnAceptar.Text = "Guardar";
            btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Font = new Font("Segoe UI", 9F);
            btnCancelar.Location = new Point(186, 914);
            btnCancelar.Margin = new Padding(4, 5, 4, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(114, 62);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormDialogoGuardarCsv
            // 
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(1129, 995);
            Controls.Add(lstArchivos);
            Controls.Add(cmbCarpetasRapidas);
            Controls.Add(txtRutaCompleta);
            Controls.Add(txtNombreArchivo);
            Controls.Add(lblInstruccion);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1133, 953);
            Name = "FormDialogoGuardarCsv";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Guardar Archivo CSV";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInstruccion;
        private System.Windows.Forms.TextBox txtNombreArchivo;
        private System.Windows.Forms.TextBox txtRutaCompleta;
        private System.Windows.Forms.ComboBox cmbCarpetasRapidas;
        private System.Windows.Forms.ListBox lstArchivos;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
