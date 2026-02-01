using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IgameToolsWinForms.Modelos;

namespace IgameToolsWinForms
{
    public partial class FormDownloadWindow : Form
    {
        private List<DownData> _downloadList;
        private bool _confirmed = false;
        private Button btnStart;
        private Button btnCancel;
        private Button btnSaveTxt;

        public bool Confirmed => _confirmed;
        public bool Files255Selected => chk255Files.Checked;
        public bool ExpandTreeSelected => chkExpandTree.Checked;
        public bool SaveTxtClicked { get; private set; }

        public FormDownloadWindow()
        {
            _downloadList = new List<DownData>();
            InitializeComponent();
        }

        public FormDownloadWindow(List<DownData> downloadList)
        {
            _downloadList = downloadList ?? new List<DownData>();
            InitializeComponent();
            Text = $"File Download ({_downloadList.Count} Files)";
            LoadDownloadList();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FormDownloadWindow));
            treeDownload = new TreeView();
            chk255Files = new CheckBox();
            chkExpandTree = new CheckBox();
            btnStart = new Button();
            btnCancel = new Button();
            btnSaveTxt = new Button();
            SuspendLayout();
            // 
            // treeDownload
            // 
            treeDownload.CheckBoxes = true;
            treeDownload.Location = new Point(10, 10);
            treeDownload.Name = "treeDownload";
            treeDownload.Size = new Size(300, 400);
            treeDownload.TabIndex = 0;
            treeDownload.AfterCheck += TreeDownload_AfterCheck;
            // 
            // chk255Files
            // 
            chk255Files.Location = new Point(15, 412);
            chk255Files.Name = "chk255Files";
            chk255Files.Size = new Size(200, 30);
            chk255Files.TabIndex = 1;
            chk255Files.Text = "255 Files Per Folder (FAT32)";
            chk255Files.UseVisualStyleBackColor = true;
            // 
            // chkExpandTree
            // 
            chkExpandTree.Location = new Point(220, 412);
            chkExpandTree.Name = "chkExpandTree";
            chkExpandTree.Size = new Size(95, 30);
            chkExpandTree.TabIndex = 2;
            chkExpandTree.Text = "Expand Tree";
            chkExpandTree.UseVisualStyleBackColor = true;
            chkExpandTree.CheckedChanged += ChkExpandTree_CheckedChanged;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(15, 445);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(95, 30);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += BtnStart_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(115, 445);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(95, 30);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnSaveTxt
            // 
            btnSaveTxt.Location = new Point(215, 445);
            btnSaveTxt.Name = "btnSaveTxt";
            btnSaveTxt.Size = new Size(90, 30);
            btnSaveTxt.TabIndex = 5;
            btnSaveTxt.Text = "Save Text";
            btnSaveTxt.UseVisualStyleBackColor = true;
            btnSaveTxt.Click += BtnSaveTxt_Click;
            // 
            // FormDownloadWindow
            // 
            AcceptButton = btnStart;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(325, 485);
            Controls.Add(treeDownload);
            Controls.Add(chk255Files);
            Controls.Add(chkExpandTree);
            Controls.Add(btnStart);
            Controls.Add(btnCancel);
            Controls.Add(btnSaveTxt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormDownloadWindow";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterParent;
            Text = "File Download";
            ResumeLayout(false);
        }

        private TreeView treeDownload;
        private CheckBox chk255Files;
        private CheckBox chkExpandTree;

        private static Task<T> EjecutarEnHiloStaAsync<T>(Func<T> funcion)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);

            var hilo = new Thread(() =>
            {
                try
                {
                    var resultado = funcion();
                    tcs.TrySetResult(resultado);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            hilo.IsBackground = true;
            hilo.SetApartmentState(ApartmentState.STA);
            hilo.Start();

            return tcs.Task;
        }

        private Task<string?> SeleccionarArchivoGuardarAsync(string titulo, string nombrePorDefecto)
        {
            return EjecutarEnHiloStaAsync(() =>
            {
                using var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FileName = nombrePorDefecto;
                saveFileDialog.Title = titulo;
                saveFileDialog.DefaultExt = "txt";

                var resultado = saveFileDialog.ShowDialog();
                return resultado == DialogResult.OK ? saveFileDialog.FileName : null;
            });
        }

        private void LoadDownloadList()
        {
            treeDownload.Nodes.Clear();
            
            // Crear las categorías principales exactamente como el original PureBasic
            var gameNode = treeDownload.Nodes.Add("+GAMES");
            var demosNode = treeDownload.Nodes.Add("+DEMOS");
            var betaGameNode = treeDownload.Nodes.Add("+BETA-GAME");
            var betaDemoNode = treeDownload.Nodes.Add("+BETA-DEMO");
            var magazinesNode = treeDownload.Nodes.Add("+MAGAZINES");
            
            // Agrupar archivos por categoría exactamente como el original
            foreach (var item in _downloadList)
            {
                TreeNode parentNode = null;
                
                switch (item.DownType)
                {
                    case "Beta-Game":
                        parentNode = betaGameNode;
                        break;
                    case "Beta-Demo":
                        parentNode = betaDemoNode;
                        break;
                    case "Demo":
                        parentNode = demosNode;
                        break;
                    case "Magazine":
                        parentNode = magazinesNode;
                        break;
                    default:
                        parentNode = gameNode;
                        break;
                }
                
                if (parentNode != null)
                {
                    var childNode = parentNode.Nodes.Add(item.DownName);
                    childNode.Tag = item;
                    childNode.Checked = true; // Marcar como seleccionado por defecto
                }
            }
            
            // Actualizar contador
            UpdateFileCount();
        }
        
        private void ChkExpandTree_CheckedChanged(object sender, EventArgs e)
        {
            // Replicar exactamente el comportamiento del original PureBasic
            if (chkExpandTree.Checked)
            {
                // Expandir todos los nodos - TreeExpandAllItems(#DOWNLOAD_LIST)
                foreach (TreeNode node in treeDownload.Nodes)
                {
                    node.ExpandAll();
                }
            }
            else
            {
                // Colapsar todos los nodos - TreeCollapseAllItems(#DOWNLOAD_LIST)
                foreach (TreeNode node in treeDownload.Nodes)
                {
                    node.Collapse();
                }
            }
        }

        private void TreeDownload_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // Evitar recursión infinita
            treeDownload.AfterCheck -= TreeDownload_AfterCheck;
            
            try
            {
                // Si es un nodo de carpeta, aplicar a todos los hijos
                if (e.Node.Nodes.Count > 0)
                {
                    foreach (TreeNode childNode in e.Node.Nodes)
                    {
                        childNode.Checked = e.Node.Checked;
                    }
                }
                
                UpdateFileCount();
            }
            finally
            {
                treeDownload.AfterCheck += TreeDownload_AfterCheck;
            }
        }

        private void UpdateFileCount()
        {
            int totalFiles = 0;
            int checkedFiles = 0;
            
            foreach (TreeNode folderNode in treeDownload.Nodes)
            {
                foreach (TreeNode fileNode in folderNode.Nodes)
                {
                    totalFiles++;
                    if (fileNode.Checked) checkedFiles++;
                }
            }
            
            // Actualizar etiqueta de información
            var lblInfo = this.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Found"));
            if (lblInfo != null)
            {
                lblInfo.Text = $"Found {totalFiles} files to download. ({checkedFiles} selected)";
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            _confirmed = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public List<DownData> GetSelectedDownloadList()
        {
            var seleccionados = new List<DownData>();

            foreach (TreeNode folderNode in treeDownload.Nodes)
            {
                foreach (TreeNode fileNode in folderNode.Nodes)
                {
                    if (!fileNode.Checked)
                    {
                        continue;
                    }

                    if (fileNode.Tag is DownData item)
                    {
                        seleccionados.Add(item);
                    }
                }
            }

            return seleccionados;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _confirmed = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void BtnSaveTxt_Click(object sender, EventArgs e)
        {
            SaveTxtClicked = true;
            
            try
            {
                if (btnSaveTxt != null)
                {
                    btnSaveTxt.Enabled = false;
                }

                Cursor.Current = Cursors.WaitCursor;
                string? filePath;
                try
                {
                    filePath = await SeleccionarArchivoGuardarAsync("Save Download List", "download_list.txt");
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return;
                }

                // Snapshot del TreeView en hilo UI
                var snapshot = treeDownload.Nodes
                    .Cast<TreeNode>()
                    .Select(folderNode => new
                    {
                        Folder = folderNode.Text,
                        Files = folderNode.Nodes.Cast<TreeNode>().Select(fileNode => new
                        {
                            Checked = fileNode.Checked,
                            Text = fileNode.Text
                        }).ToList()
                    })
                    .ToList();

                var totalArchivos = _downloadList.Count;

                // Generar el texto en background
                var lines = await Task.Run(() =>
                {
                    var salida = new List<string>
                    {
                        "WHDLoad Download List",
                        "======================",
                        $"Total Files: {totalArchivos}",
                        $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                        ""
                    };

                    foreach (var folder in snapshot)
                    {
                        salida.Add($"[{folder.Folder}]");
                        foreach (var file in folder.Files)
                        {
                            var status = file.Checked ? "✓" : "-";
                            salida.Add($"  {status} {file.Text}");
                        }
                        salida.Add("");
                    }

                    return salida;
                });

                await File.WriteAllLinesAsync(filePath, lines);

                MessageBox.Show($"File saved successfully to:\n{filePath}",
                              "Save Complete",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file:\n{ex.Message}", 
                              "Save Error", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Error);
            }
            finally
            {
                if (btnSaveTxt != null)
                {
                    btnSaveTxt.Enabled = true;
                }
                Cursor.Current = Cursors.Default;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Dibujar borde decorativo
            using (var pen = new Pen(Color.FromArgb(0, 0, 128), 2))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(10, 10, this.ClientSize.Width - 20, this.ClientSize.Height - 20));
            }
        }
    }
}
