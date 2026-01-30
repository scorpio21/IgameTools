namespace IgameToolsWinForms
{
    partial class FormDialogoCargarCsv
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDialogoCargarCsv));
            lblInstruccion = new Label();
            cmbCarpetasRapidas = new ComboBox();
            txtRuta = new TextBox();
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
            lblInstruccion.Size = new Size(265, 28);
            lblInstruccion.TabIndex = 0;
            lblInstruccion.Text = "Selecciona un archivo CSV:";
            // 
            // cmbCarpetasRapidas
            // 
            cmbCarpetasRapidas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCarpetasRapidas.Font = new Font("Segoe UI", 9F);
            cmbCarpetasRapidas.Location = new Point(29, 86);
            cmbCarpetasRapidas.Margin = new Padding(4, 5, 4, 5);
            cmbCarpetasRapidas.Name = "cmbCarpetasRapidas";
            cmbCarpetasRapidas.Size = new Size(1070, 33);
            cmbCarpetasRapidas.TabIndex = 1;
            // 
            // txtRuta
            // 
            txtRuta.Font = new Font("Segoe UI", 9F);
            txtRuta.Location = new Point(29, 148);
            txtRuta.Margin = new Padding(4, 5, 4, 5);
            txtRuta.Name = "txtRuta";
            txtRuta.ReadOnly = true;
            txtRuta.Size = new Size(1070, 31);
            txtRuta.TabIndex = 2;
            // 
            // lstArchivos
            // 
            lstArchivos.Font = new Font("Segoe UI", 9F);
            lstArchivos.FormattingEnabled = true;
            lstArchivos.Location = new Point(29, 203);
            lstArchivos.Margin = new Padding(4, 5, 4, 5);
            lstArchivos.Name = "lstArchivos";
            lstArchivos.Size = new Size(1070, 604);
            lstArchivos.TabIndex = 3;
            // 
            // btnAceptar
            // 
            btnAceptar.Font = new Font("Segoe UI", 9F);
            btnAceptar.Location = new Point(29, 844);
            btnAceptar.Margin = new Padding(4, 5, 4, 5);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(143, 55);
            btnAceptar.TabIndex = 4;
            btnAceptar.Text = "Cargar";
            btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Font = new Font("Segoe UI", 9F);
            btnCancelar.Location = new Point(186, 844);
            btnCancelar.Margin = new Padding(4, 5, 4, 5);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(114, 55);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormDialogoCargarCsv
            // 
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(1129, 917);
            Controls.Add(lstArchivos);
            Controls.Add(txtRuta);
            Controls.Add(cmbCarpetasRapidas);
            Controls.Add(lblInstruccion);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(1133, 906);
            Name = "FormDialogoCargarCsv";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Seleccionar Archivo CSV";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInstruccion;
        private System.Windows.Forms.ComboBox cmbCarpetasRapidas;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.ListBox lstArchivos;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
