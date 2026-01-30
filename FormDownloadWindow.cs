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
        private Button btnSaveTxt;

        public bool Confirmed => _confirmed;
        public bool Files255Selected => chk255Files.Checked;
        public bool ExpandTreeSelected => chkExpandTree.Checked;
        public bool SaveTxtClicked { get; private set; }

        public FormDownloadWindow(List<DownData> downloadList)
        {
            _downloadList = downloadList ?? new List<DownData>();
            InitializeComponent();
            LoadDownloadList();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configuración del formulario - Ventana muy ancha para máximo espacio horizontal
            this.Text = $"File Download ({_downloadList.Count} Files)";
            this.Size = new Size(500, 550); // Mucho más ancha
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.ControlBox = true; // Tool window
            this.Padding = new Padding(10); // Padding generoso

            // TreeGadget - Usa todo el ancho disponible (10,10,480,380)
            treeDownload = new TreeView()
            {
                Location = new Point(10, 10),
                Size = new Size(480, 380), // Usa casi todo el ancho
                Font = new Font("Arial", 9),
                Scrollable = true,
                ShowPlusMinus = true,
                ShowLines = true,
                ShowRootLines = true,
                FullRowSelect = true,
                CheckBoxes = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            treeDownload.AfterCheck += TreeDownload_AfterCheck;
            this.Controls.Add(treeDownload);

            // CheckBoxGadget #DOWNLOAD_A500MINI - Más espacio horizontal (10,395,220,25)
            chk255Files = new CheckBox()
            {
                Text = "255 Files Per Folder (FAT32)",
                Location = new Point(10, 395), // Más abajo
                Size = new Size(220, 25), // Mucho más ancho
                Font = new Font("Arial", 8)
            };
            this.Controls.Add(chk255Files);

            // CheckBoxGadget #DOWNLOAD_EXPAND - Más espacio horizontal (240,395,250,25)
            chkExpandTree = new CheckBox()
            {
                Text = "Expand Tree",
                Location = new Point(240, 395), // Más a la derecha
                Size = new Size(250, 25), // Mucho más espacio
                Font = new Font("Arial", 8)
            };
            chkExpandTree.CheckedChanged += ChkExpandTree_CheckedChanged;
            this.Controls.Add(chkExpandTree);

            // ButtonGadget #DOWNLOAD_YES - Espacio horizontal generoso (10,425,140,30)
            var btnStart = new Button()
            {
                Text = "Start",
                Location = new Point(10, 425), // Más abajo
                Size = new Size(140, 30), // Mucho más ancho
                Font = new Font("Arial", 9),
                BackColor = Color.LightGreen,
                DialogResult = DialogResult.OK
            };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

            // ButtonGadget #DOWNLOAD_NO - Espacio horizontal generoso (160,425,140,30)
            var btnCancel = new Button()
            {
                Text = "Cancel",
                Location = new Point(160, 425), // 10px entre botones
                Size = new Size(140, 30), // Mucho más ancho
                Font = new Font("Arial", 9),
                BackColor = Color.LightCoral,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);

            // ButtonGadget #DOWNLOAD_SAVE - Espacio horizontal generoso (310,425,140,30)
            btnSaveTxt = new Button()
            {
                Text = "Save Text",
                Location = new Point(310, 425), // 10px entre botones
                Size = new Size(140, 30), // Mucho más ancho
                Font = new Font("Arial", 9),
                BackColor = Color.LightYellow
            };
            btnSaveTxt.Click += BtnSaveTxt_Click;
            this.Controls.Add(btnSaveTxt);

            // Botón por defecto
            this.AcceptButton = btnStart;
            this.CancelButton = btnCancel;

            this.ResumeLayout(false);
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
