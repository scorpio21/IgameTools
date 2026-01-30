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
            this.lstPreview = new System.Windows.Forms.ListBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstPreview
            // 
            this.lstPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPreview.FormattingEnabled = true;
            this.lstPreview.ItemHeight = 16;
            this.lstPreview.Location = new System.Drawing.Point(12, 12);
            this.lstPreview.Name = "lstPreview";
            this.lstPreview.Size = new System.Drawing.Size(560, 276);
            this.lstPreview.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(12, 300);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(79, 17);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total archivos: 0";
            // 
            // lblSize
            // 
            this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(12, 327);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(75, 17);
            this.lblSize.TabIndex = 2;
            this.lblSize.Text = "Tamaño total: 0";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(497, 300);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Cerrar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // FormDownloadPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 357);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lstPreview);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FormDownloadPreview";
            this.Text = "Vista Previa de Descarga";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ListBox lstPreview;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Button btnClose;
    }
}
