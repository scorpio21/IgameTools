using System;
using System.Drawing;
using System.Windows.Forms;

namespace IgameToolsWinForms;

public partial class FormAyuda : Form
{
    private RichTextBox richTextBoxAyuda = null!;

    public FormAyuda()
    {
        InitializeComponent();
        CargarContenidoAyuda();
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAyuda));
        richTextBoxAyuda = new RichTextBox();
        SuspendLayout();
        // 
        // richTextBoxAyuda
        // 
        richTextBoxAyuda.BackColor = Color.White;
        richTextBoxAyuda.Dock = DockStyle.Fill;
        richTextBoxAyuda.Font = new Font("Segoe UI", 9F);
        richTextBoxAyuda.Location = new Point(0, 0);
        richTextBoxAyuda.Margin = new Padding(10);
        richTextBoxAyuda.Name = "richTextBoxAyuda";
        richTextBoxAyuda.ReadOnly = true;
        richTextBoxAyuda.ScrollBars = RichTextBoxScrollBars.Vertical;
        richTextBoxAyuda.Size = new Size(578, 444);
        richTextBoxAyuda.TabIndex = 0;
        richTextBoxAyuda.Text = "";
        // 
        // FormAyuda
        // 
        ClientSize = new Size(578, 444);
        Controls.Add(richTextBoxAyuda);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormAyuda";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Ayuda - IgameTools by Scorpio21";
        ResumeLayout(false);
    }

    private void CargarContenidoAyuda()
    {
        var contenido = @"*** ADVERTENCIA ***

IGame Tool solo soporta listas de juegos basadas en CSV. Puedes saber si la lista es correcta por el hecho de que se llamará 'gameslist.csv'. 
Si tu lista de juegos no tiene '.csv' al final del nombre del archivo, entonces tienes una versión antigua de IGame.
Por favor, actualiza a la última versión en https://github.com/MrZammler/iGame/releases y vuelve a escanear tus repositorios.

*** Acerca de ***

IGame Tool es una pequeña utilidad que usa una pequeña base de datos para mejorar los nombres y añadir géneros de juegos a archivos de lista de juegos Amiga IGame. IGame Tool no es perfecto y 
no es lo suficientemente inteligente como para encontrar algunos archivos y todavía duplicará algunas entradas, pero sigue siendo mejor que la lista predeterminada. Hay alguna edición básica 
que se puede hacer en las entradas para ayudar a reparar cualquier error.

*** Instrucciones ***

1. Copia el archivo gameslist.csv de tu drawer Amiga IGame a tu PC. También... ¡HAZ UNA COPIA DE SEGURIDAD!
2. Presiona el botón 'Load CSV' para abrir tu lista de juegos IGame.
3. Presiona el botón 'Fix List' para arreglar los nombres de los juegos y añadir géneros.
4. Haz cualquier otro cambio necesario.
5. Presiona el botón 'Save CSV' para guardar la nueva lista de juegos. Puedes sobrescribir la lista de juegos antigua o guardar como un nuevo archivo.
6. Copia la nueva lista y el archivo genres suministrado de vuelta al drawer IGame en tu unidad Amiga.

*** Lista de Juegos ***

Las entradas duplicadas se resaltan en rojo y las entradas desconocidas se resaltan en azul. Las entradas faltantes solo se resaltarán después de haber presionado el botón 'Fix List'.

*** Edición ***

Para editar un nombre, haz doble clic en la entrada de la lista y cambia su nombre en la nueva ventana.

'Quick Tag' te permite añadir múltiples etiquetas a las entradas de la lista. Simplemente escribe el nombre de la etiqueta en la nueva ventana y se añadirá al final del nombre del juego.
Puedes reducir fácilmente las entradas duplicadas usando este botón. Quick Tag funcionará con múltiples entradas seleccionadas. Usa Ctrl o Shift cuando hagas clic
en la lista para seleccionar múltiples entradas.

'Undo' revertirá el último cambio que se haya realizado.

*** Base de Datos ***

'Keep Data' mantiene los datos de juego del archivo CSV original

'Use Short Names' reemplaza el nombre del juego con una versión corta de 26 caracteres.

*** Filtro ***

'Show Duplicates' filtra la lista y muestra entradas duplicadas.

'Show Unknown' filtra la lista y muestra entradas desconocidas. Si una entrada está marcada como desconocida, puede valer la pena verificar si el slave ha sido actualizado.

'Title Case' establece el caso de los nombres de los títulos en el archivo CSV de salida. Las opciones disponibles son 'Camel Case', 'lower case' y 'UPPER CASE'. Selecciona de este menú para guardar los nombres de los títulos como desees.";

        richTextBoxAyuda.Text = contenido;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            this.Close();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }
}
