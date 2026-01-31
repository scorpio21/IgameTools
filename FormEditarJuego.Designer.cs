namespace IgameToolsWinForms;

partial class FormEditarJuego
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditarJuego));
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblNombreCorto = new Label();
        txtNombreCorto = new TextBox();
        lblSlave = new Label();
        txtSlave = new TextBox();
        lblGenero = new Label();
        cmbGenero = new ComboBox();
        btnOk = new Button();
        btnCancelar = new Button();
        SuspendLayout();
        // 
        // lblNombre
        // 
        lblNombre.Location = new Point(46, 21);
        lblNombre.Margin = new Padding(4, 0, 4, 0);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(86, 38);
        lblNombre.TabIndex = 0;
        lblNombre.Text = "Nombre";
        lblNombre.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombre
        // 
        txtNombre.Location = new Point(140, 25);
        txtNombre.Margin = new Padding(4, 5, 4, 5);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(433, 31);
        txtNombre.TabIndex = 1;
        // 
        // lblNombreCorto
        // 
        lblNombreCorto.Location = new Point(1, 76);
        lblNombreCorto.Margin = new Padding(4, 0, 4, 0);
        lblNombreCorto.Name = "lblNombreCorto";
        lblNombreCorto.Size = new Size(147, 38);
        lblNombreCorto.TabIndex = 2;
        lblNombreCorto.Text = "Nombre Corto";
        lblNombreCorto.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtNombreCorto
        // 
        txtNombreCorto.Location = new Point(140, 80);
        txtNombreCorto.Margin = new Padding(4, 5, 4, 5);
        txtNombreCorto.Name = "txtNombreCorto";
        txtNombreCorto.Size = new Size(433, 31);
        txtNombreCorto.TabIndex = 3;
        // 
        // lblSlave
        // 
        lblSlave.Location = new Point(1, 135);
        lblSlave.Margin = new Padding(4, 0, 4, 0);
        lblSlave.Name = "lblSlave";
        lblSlave.Size = new Size(131, 38);
        lblSlave.TabIndex = 4;
        lblSlave.Text = "Nombre .Slave";
        lblSlave.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtSlave
        // 
        txtSlave.Location = new Point(140, 139);
        txtSlave.Margin = new Padding(4, 5, 4, 5);
        txtSlave.Name = "txtSlave";
        txtSlave.Size = new Size(433, 31);
        txtSlave.TabIndex = 5;
        // 
        // lblGenero
        // 
        lblGenero.Location = new Point(50, 186);
        lblGenero.Margin = new Padding(4, 0, 4, 0);
        lblGenero.Name = "lblGenero";
        lblGenero.Size = new Size(71, 38);
        lblGenero.TabIndex = 6;
        lblGenero.Text = "Genero";
        lblGenero.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cmbGenero
        // 
        cmbGenero.FormattingEnabled = true;
        cmbGenero.Location = new Point(140, 190);
        cmbGenero.Margin = new Padding(4, 5, 4, 5);
        cmbGenero.Name = "cmbGenero";
        cmbGenero.Size = new Size(433, 33);
        cmbGenero.TabIndex = 7;
        // 
        // btnOk
        // 
        btnOk.Location = new Point(37, 233);
        btnOk.Margin = new Padding(4, 5, 4, 5);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(107, 45);
        btnOk.TabIndex = 8;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += btnOk_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.Location = new Point(466, 233);
        btnCancelar.Margin = new Padding(4, 5, 4, 5);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(107, 45);
        btnCancelar.TabIndex = 9;
        btnCancelar.Text = "Cancel";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // FormEditarJuego
        // 
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(592, 289);
        Controls.Add(btnCancelar);
        Controls.Add(btnOk);
        Controls.Add(cmbGenero);
        Controls.Add(lblGenero);
        Controls.Add(txtSlave);
        Controls.Add(lblSlave);
        Controls.Add(txtNombreCorto);
        Controls.Add(lblNombreCorto);
        Controls.Add(txtNombre);
        Controls.Add(lblNombre);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        Margin = new Padding(4, 5, 4, 5);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormEditarJuego";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Editar Datos";
        ResumeLayout(false);
        PerformLayout();

    }

    private System.Windows.Forms.Label lblNombre;
    private System.Windows.Forms.TextBox txtNombre;
    private System.Windows.Forms.Label lblNombreCorto;
    private System.Windows.Forms.TextBox txtNombreCorto;
    private System.Windows.Forms.Label lblSlave;
    private System.Windows.Forms.TextBox txtSlave;
    private System.Windows.Forms.Label lblGenero;
    private System.Windows.Forms.ComboBox cmbGenero;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancelar;
}
