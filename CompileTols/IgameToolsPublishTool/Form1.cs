namespace IgameToolsPublishTool;

public partial class Form1 : Form
{
    private CancellationTokenSource? _cts;

    private int _pasoActual = 1;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        var rutaRepo = DetectarRutaRepoDesdeEjecucion();
        if (!string.IsNullOrWhiteSpace(rutaRepo))
        {
            txtRutaProyecto.Text = rutaRepo;
        }

        txtVersion.Text = "0.2.0";
        chkLimpiarPublish.Checked = true;

        var iscc = PublicadorIgameTools.DetectarRutaIscc();
        if (!string.IsNullOrWhiteSpace(iscc))
        {
            txtRutaIscc.Text = iscc;
        }

        pnlAcciones.AutoScroll = true;

        _pasoActual = 1;
        ActualizarBotonesPorPaso();

        EscribirLogSincrono("Herramienta lista.");
    }

    private void menuAyudaPasos_Click(object sender, EventArgs e)
    {
        var texto =
            "Guía paso a paso\n\n" +
            "1) Preparar cambios (código y recursos)\n" +
            "   - Revisa img/ y csv/ y que el proyecto compila.\n" +
            "   - En la herramienta: usa Paso 1 (Build).\n\n" +
            "2) Publicar binarios (Release, self-contained, single-file)\n" +
            "   - En la herramienta: Paso 2 (Publish).\n\n" +
            "3) Generar el instalador con Inno Setup\n" +
            "   - Requiere Inno Setup 6 (ISCC.exe).\n" +
            "   - En la herramienta: Paso 3 (Instalador).\n\n" +
            "3b) Crear versión portable (ZIP)\n" +
            "   - En la herramienta: Paso 3b (ZIP portable).\n\n" +
            "3c) Crear ZIP win-x64 (single-file)\n" +
            "   - En la herramienta: Paso 3c (ZIP single-file).\n\n" +
            "4) Verificar que el instalador incluya recursos\n" +
            "   - Comprueba que en publish\\win-x64-singlefile existen img/ y csv/.\n" +
            "   - En la herramienta: Paso 4 (Verificar recursos).\n\n" +
            "5) Fallback: limpieza completa si algo queda obsoleto\n" +
            "   - Esto es un plan B (no es el paso 1).\n" +
            "   - Solo úsalo si sospechas que hay archivos antiguos: limpia publish, clean, restore, publish, instalador.\n" +
            "   - En la herramienta: Paso 5 (Fallback limpieza).";

        MessageBox.Show(this, texto, "Ayuda - Guía paso a paso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static string? DetectarRutaRepoDesdeEjecucion()
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;
            var dir = new DirectoryInfo(baseDir);

            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "IgameToolsWinForms.csproj")))
                {
                    return dir.FullName;
                }

                dir = dir.Parent;
            }
        }
        catch
        {
        }

        return null;
    }

    private async void btnSeleccionarRutaProyecto_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description = "Selecciona la carpeta raíz del proyecto IgameTools",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false,
        };

        if (Directory.Exists(txtRutaProyecto.Text))
        {
            dlg.SelectedPath = txtRutaProyecto.Text;
        }

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            txtRutaProyecto.Text = dlg.SelectedPath;
            await EscribirLogAsync($"Ruta proyecto: {dlg.SelectedPath}");
        }
    }

    private async Task EjecutarPasoAsync(int paso, Func<PublicadorIgameTools, CancellationToken, Task> accionAsync)
    {
        if (_pasoActual != paso)
        {
            await EscribirLogAsync($"Paso no disponible. Actual: {_pasoActual}");
            return;
        }

        await EjecutarAsync(async (pub, ct) =>
        {
            await accionAsync(pub, ct);

            _pasoActual++;
            ActualizarBotonesPorPaso();
        });
    }

    private async void btnBuscarIscc_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Selecciona ISCC.exe (Inno Setup)",
            Filter = "ISCC.exe|ISCC.exe|Ejecutable (*.exe)|*.exe|Todos (*.*)|*.*",
            CheckFileExists = true,
        };

        if (File.Exists(txtRutaIscc.Text))
        {
            dlg.InitialDirectory = Path.GetDirectoryName(txtRutaIscc.Text);
            dlg.FileName = Path.GetFileName(txtRutaIscc.Text);
        }

        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            txtRutaIscc.Text = dlg.FileName;
            await EscribirLogAsync($"ISCC: {dlg.FileName}");
        }
    }

    private async void btnBuild_Click(object sender, EventArgs e)
    {
        await EjecutarPasoAsync(1, async (pub, ct) =>
        {
            await pub.EjecutarBuildAsync(EscribirLogAsync, ct);
        });
    }

    private async void btnLimpiarPublish_Click(object sender, EventArgs e)
    {
        await EjecutarAsync(async (pub, ct) =>
        {
            await pub.LimpiarPublishAsync(EscribirLogAsync, ct);
        });
    }

    private async void btnPublish_Click(object sender, EventArgs e)
    {
        await EjecutarPasoAsync(2, async (pub, ct) =>
        {
            await pub.EjecutarPublishSingleFileAsync(chkLimpiarPublish.Checked, EscribirLogAsync, ct);
            await pub.CopiarRecursosAsync(EscribirLogAsync, ct);
        });
    }

    private async void btnCopiarRecursos_Click(object sender, EventArgs e)
    {
        await EjecutarAsync(async (pub, ct) =>
        {
            await pub.CopiarRecursosAsync(EscribirLogAsync, ct);
        });
    }

    private async void btnZipPortable_Click(object sender, EventArgs e)
    {
        await EjecutarPasoAsync(4, async (pub, ct) =>
        {
            var version = ObtenerVersion();
            await pub.CrearZipPortableAsync(version, EscribirLogAsync, ct);
        });
    }

    private async void btnZipSingleFile_Click(object sender, EventArgs e)
    {
        await EjecutarPasoAsync(5, async (pub, ct) =>
        {
            var version = ObtenerVersion();
            await pub.CrearZipSingleFileAsync(version, EscribirLogAsync, ct);
        });
    }

    private async void btnInstalador_Click(object sender, EventArgs e)
    {
        await EjecutarPasoAsync(3, async (pub, ct) =>
        {
            var version = ObtenerVersion();
            var iscc = ObtenerRutaIscc();
            await pub.CrearInstaladorAsync(iscc, version, EscribirLogAsync, ct);
        });
    }

    private async void btnVerificarRecursos_Click(object sender, EventArgs e)
    {
        await EjecutarPasoAsync(6, async (pub, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            var carpeta = Path.Combine(pub.RutaProyecto, "publish", "win-x64-singlefile");
            var exe = Path.Combine(carpeta, "IgameToolsWinForms.exe");
            var img = Path.Combine(carpeta, "img");
            var csv = Path.Combine(carpeta, "csv");

            var falta = new List<string>();
            if (!Directory.Exists(carpeta)) falta.Add($"No existe: {carpeta}");
            if (!File.Exists(exe)) falta.Add("Falta IgameToolsWinForms.exe en la carpeta publicada");
            if (!Directory.Exists(img)) falta.Add("Falta img/ dentro de la carpeta publicada");
            if (!Directory.Exists(csv)) falta.Add("Falta csv/ dentro de la carpeta publicada");

            if (falta.Count > 0)
            {
                await EscribirLogAsync("Verificación: FALLÓ");
                foreach (var f in falta)
                {
                    await EscribirLogAsync($"- {f}");
                }

                MessageBox.Show(this, string.Join(Environment.NewLine, falta), "Verificar recursos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await EscribirLogAsync("Verificación: OK (exe + img + csv presentes)");
            MessageBox.Show(this, "OK: La publicación incluye IgameToolsWinForms.exe, img/ y csv/.", "Verificar recursos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private async void btnFallbackLimpieza_Click(object sender, EventArgs e)
    {
        if (_pasoActual != 7)
        {
            MessageBox.Show(this, "Este paso es el plan B. Completa primero los pasos anteriores.", "Fallback limpieza", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            this,
            "Esto borrará el contenido de publish/ y ejecutará clean/restore/publish. ¿Continuar?",
            "Fallback limpieza",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        await EjecutarPasoAsync(7, async (pub, ct) =>
        {
            var version = ObtenerVersion();
            var iscc = ObtenerRutaIscc();

            await pub.LimpiarPublishAsync(EscribirLogAsync, ct);
            await EjecutarProcesoDotnetAsync(pub.RutaProyecto, "clean", ct);
            await EjecutarProcesoDotnetAsync(pub.RutaProyecto, "restore", ct);
            await pub.EjecutarPublishSingleFileAsync(limpiarAntes: false, EscribirLogAsync, ct);
            await pub.CopiarRecursosAsync(EscribirLogAsync, ct);
            await pub.CrearInstaladorAsync(iscc, version, EscribirLogAsync, ct);
        });
    }

    private async void btnEjecutarTodo_Click(object sender, EventArgs e)
    {
        await EjecutarAsync(async (pub, ct) =>
        {
            var version = ObtenerVersion();
            var iscc = ObtenerRutaIscc();

            progressBar1.Style = ProgressBarStyle.Marquee;

            if (chkLimpiarPublish.Checked)
            {
                await pub.LimpiarPublishAsync(EscribirLogAsync, ct);
            }

            await pub.EjecutarBuildAsync(EscribirLogAsync, ct);
            await pub.EjecutarPublishSingleFileAsync(limpiarAntes: false, EscribirLogAsync, ct);
            await pub.CopiarRecursosAsync(EscribirLogAsync, ct);
            await pub.CrearInstaladorAsync(iscc, version, EscribirLogAsync, ct);
            await pub.CrearZipPortableAsync(version, EscribirLogAsync, ct);
            await pub.CrearZipSingleFileAsync(version, EscribirLogAsync, ct);
        });
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
        _cts?.Cancel();
    }

    private string ObtenerVersion()
    {
        var version = txtVersion.Text.Trim();
        if (string.IsNullOrWhiteSpace(version))
        {
            throw new InvalidOperationException("La versión no puede estar vacía.");
        }

        return version;
    }

    private string ObtenerRutaProyecto()
    {
        var ruta = txtRutaProyecto.Text.Trim();
        if (string.IsNullOrWhiteSpace(ruta) || !Directory.Exists(ruta))
        {
            throw new InvalidOperationException("La ruta del proyecto no es válida.");
        }

        if (!File.Exists(Path.Combine(ruta, "IgameToolsWinForms.csproj")))
        {
            throw new InvalidOperationException("No se encontró IgameToolsWinForms.csproj en la ruta indicada.");
        }

        return ruta;
    }

    private string ObtenerRutaIscc()
    {
        var ruta = txtRutaIscc.Text.Trim();
        if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
        {
            throw new InvalidOperationException("ISCC.exe no es válido. Instala Inno Setup 6 o selecciona ISCC.exe.");
        }

        return ruta;
    }

    private async Task EjecutarAsync(Func<PublicadorIgameTools, CancellationToken, Task> accionAsync)
    {
        if (_cts != null)
        {
            await EscribirLogAsync("Ya hay una operación en curso.");
            return;
        }

        try
        {
            var rutaProyecto = ObtenerRutaProyecto();
            var publicador = new PublicadorIgameTools(rutaProyecto);
            _cts = new CancellationTokenSource();

            SetEstadoEjecutando(true);
            progressBar1.Style = ProgressBarStyle.Marquee;

            await accionAsync(publicador, _cts.Token);

            await EscribirLogAsync("OK");
        }
        catch (OperationCanceledException)
        {
            await EscribirLogAsync("Cancelado");
        }
        catch (Exception ex)
        {
            await EscribirLogAsync($"ERROR: {ex.Message}");
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
            progressBar1.Style = ProgressBarStyle.Blocks;
            SetEstadoEjecutando(false);
        }
    }

    private void SetEstadoEjecutando(bool ejecutando)
    {
        btnLimpiarPublish.Enabled = !ejecutando;
        btnBuild.Enabled = !ejecutando;
        btnPublish.Enabled = !ejecutando;
        btnInstalador.Enabled = !ejecutando;
        btnZipPortable.Enabled = !ejecutando;
        btnZipSingleFile.Enabled = !ejecutando;
        btnVerificarRecursos.Enabled = !ejecutando;
        btnFallbackLimpieza.Enabled = !ejecutando;
        btnEjecutarTodo.Enabled = !ejecutando;
        btnSeleccionarRutaProyecto.Enabled = !ejecutando;
        btnBuscarIscc.Enabled = !ejecutando;
        btnCancelar.Enabled = ejecutando;

        menuAyuda.Enabled = !ejecutando;

        if (!ejecutando)
        {
            ActualizarBotonesPorPaso();
        }
    }

    private void ActualizarBotonesPorPaso()
    {
        btnBuild.Enabled = _pasoActual == 1;
        btnPublish.Enabled = _pasoActual == 2;
        btnInstalador.Enabled = _pasoActual == 3;
        btnZipPortable.Enabled = _pasoActual == 4;
        btnZipSingleFile.Enabled = _pasoActual == 5;
        btnVerificarRecursos.Enabled = _pasoActual == 6;
        btnFallbackLimpieza.Enabled = _pasoActual == 7;

        btnCopiarRecursos.Enabled = false;
        btnEjecutarTodo.Enabled = true;

        btnLimpiarPublish.Enabled = _pasoActual <= 2;
    }

    private async Task EjecutarProcesoDotnetAsync(string rutaProyecto, string argumentos, CancellationToken cancellationToken)
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = argumentos,
            WorkingDirectory = rutaProyecto,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
            StandardErrorEncoding = System.Text.Encoding.UTF8,
        };

        using var proceso = new System.Diagnostics.Process { StartInfo = psi };

        await EscribirLogAsync($"> dotnet {argumentos}");

        if (!proceso.Start())
        {
            throw new InvalidOperationException("No se pudo iniciar dotnet.");
        }

        var salidaTask = proceso.StandardOutput.ReadToEndAsync(cancellationToken);
        var errorTask = proceso.StandardError.ReadToEndAsync(cancellationToken);

        await proceso.WaitForExitAsync(cancellationToken);

        var salida = await salidaTask;
        var error = await errorTask;

        if (!string.IsNullOrWhiteSpace(salida))
        {
            foreach (var linea in salida.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                await EscribirLogAsync(linea);
            }
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            foreach (var linea in error.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                await EscribirLogAsync(linea);
            }
        }

        if (proceso.ExitCode != 0)
        {
            throw new InvalidOperationException($"dotnet {argumentos} falló con ExitCode={proceso.ExitCode}");
        }
    }

    private void EscribirLogSincrono(string mensaje)
    {
        if (txtLog.TextLength > 200_000)
        {
            txtLog.Clear();
        }

        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {mensaje}{Environment.NewLine}");
        txtLog.SelectionStart = txtLog.TextLength;
        txtLog.ScrollToCaret();
    }

    private Task EscribirLogAsync(string mensaje)
    {
        if (IsDisposed)
        {
            return Task.CompletedTask;
        }

        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => EscribirLogSincrono(mensaje)));
            return Task.CompletedTask;
        }

        EscribirLogSincrono(mensaje);
        return Task.CompletedTask;
    }
}
