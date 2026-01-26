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
            this.lblInstruccion = new System.Windows.Forms.Label();
            this.cmbCarpetasRapidas = new System.Windows.Forms.ComboBox();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.lstArchivos = new System.Windows.Forms.ListBox();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInstruccion
            // 
            this.lblInstruccion.AutoSize = true;
            this.lblInstruccion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInstruccion.Location = new System.Drawing.Point(20, 20);
            this.lblInstruccion.Name = "lblInstruccion";
            this.lblInstruccion.Size = new System.Drawing.Size(300, 25);
            this.lblInstruccion.TabIndex = 0;
            this.lblInstruccion.Text = "Selecciona un archivo CSV:";
            // 
            // cmbCarpetasRapidas
            // 
            this.cmbCarpetasRapidas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCarpetasRapidas.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCarpetasRapidas.Location = new System.Drawing.Point(20, 55);
            this.cmbCarpetasRapidas.Name = "cmbCarpetasRapidas";
            this.cmbCarpetasRapidas.Size = new System.Drawing.Size(750, 25);
            this.cmbCarpetasRapidas.TabIndex = 1;
            // 
            // txtRuta
            // 
            this.txtRuta.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRuta.Location = new System.Drawing.Point(20, 95);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(750, 25);
            this.txtRuta.TabIndex = 2;
            // 
            // lstArchivos
            // 
            this.lstArchivos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstArchivos.FormattingEnabled = true;
            this.lstArchivos.ItemHeight = 16;
            this.lstArchivos.Location = new System.Drawing.Point(20, 130);
            this.lstArchivos.Name = "lstArchivos";
            this.lstArchivos.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.lstArchivos.Size = new System.Drawing.Size(750, 400);
            this.lstArchivos.TabIndex = 3;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAceptar.Location = new System.Drawing.Point(20, 540);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(100, 35);
            this.btnAceptar.TabIndex = 4;
            this.btnAceptar.Text = "Cargar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancelar.Location = new System.Drawing.Point(130, 540);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(80, 35);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // FormDialogoCargarCsv
            // 
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(790, 587);
            this.Controls.Add(this.lstArchivos);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.cmbCarpetasRapidas);
            this.Controls.Add(this.lblInstruccion);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FormDialogoCargarCsv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Archivo CSV";
            this.ResumeLayout(false);
            this.PerformLayout();

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
