using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IgameToolsWinForms.Modelos;

namespace IgameToolsWinForms
{
    public partial class FormDownloadPreview : Form
    {
        private readonly List<DownData> _downloadList;

        public FormDownloadPreview(List<DownData> downloadList)
        {
            InitializeComponent();
            _downloadList = downloadList;
            CargarLista();
        }

        private void CargarLista()
        {
            lstPreview.Items.Clear();
            
            foreach (var item in _downloadList)
            {
                var listItem = $"{item.DownName} ({item.DownType}) - {FormatFileSize(item.DownSize)}";
                lstPreview.Items.Add(listItem);
            }
            
            lblTotal.Text = $"Total archivos: {_downloadList.Count}";
            
            var totalSize = 0L;
            foreach (var item in _downloadList)
            {
                totalSize += item.DownSize;
            }
            
            lblSize.Text = $"Tamaño total: {FormatFileSize(totalSize)}";
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDownloadPreview));
            lstPreview = new ListBox();
            lblTotal = new Label();
            lblSize = new Label();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lstPreview
            // 
            lstPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstPreview.FormattingEnabled = true;
            lstPreview.ItemHeight = 25;
            lstPreview.Location = new Point(15, 19);
            lstPreview.Margin = new Padding(4, 5, 4, 5);
            lstPreview.Name = "lstPreview";
            lstPreview.Size = new Size(699, 429);
            lstPreview.TabIndex = 0;
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(15, 469);
            lblTotal.Margin = new Padding(4, 0, 4, 0);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(138, 25);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "Total archivos: 0";
            // 
            // lblSize
            // 
            lblSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSize.AutoSize = true;
            lblSize.Location = new Point(15, 511);
            lblSize.Margin = new Padding(4, 0, 4, 0);
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(134, 25);
            lblSize.TabIndex = 2;
            lblSize.Text = "Tamaño total: 0";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(621, 469);
            btnClose.Margin = new Padding(4, 5, 4, 5);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 47);
            btnClose.TabIndex = 3;
            btnClose.Text = "Cerrar";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // FormDownloadPreview
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(730, 558);
            Controls.Add(btnClose);
            Controls.Add(lblSize);
            Controls.Add(lblTotal);
            Controls.Add(lstPreview);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(494, 437);
            Name = "FormDownloadPreview";
            Text = "Vista Previa de Descarga";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ListBox lstPreview;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Button btnClose;
    }
}
