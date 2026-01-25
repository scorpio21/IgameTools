# Changelog

## v0.1.7 - 2026-01-24

### Corregido
- **Carga CSV**: Corregido el formato de parsing para manejar correctamente el CSV con ID en primer campo.
- **Fix List**: Mejorado el procesamiento de IG_Data.dat con extracción correcta de slaves desde el path.
- **FTP**: Optimizada la conexión con credenciales correctas y manejo de errores mejorado.
- **Interfaz**: Eliminados archivos de debug para mayor limpieza del entorno.

### Mejorado
- **Nombres Cortos**: Implementada truncación inteligente en límites de palabras.
- **Resumen Fix List**: Ventana detallada con estadísticas de actualización y fecha.
- **Validación**: Sistema de validación CSV más robusto con mensajes claros.
- **Extracción de Slaves**: Método mejorado para extraer slaves de rutas tipo "Games:0/Juego/Juego.Slave".

### Cambiado
- El CSV ahora procesa correctamente el formato: ID;Nombre;Género;Path;Slave;Datos...
- Fix List presenta resumen detallado al finalizar con tasa de actualización y estadísticas.
- Eliminados archivos de debug temporales para mantener el entorno limpio.

---

## v0.1.6 - 2026-01-24

### Añadido
- Sistema de Undo/Redo robusto usando Command pattern:
  - Interfaz `ICommand` y comandos concretos para todas las acciones.
  - `EditarJuegoCommand` para edición individual de juegos.
  - `QuickTagCommand` para etiquetado rápido.
  - `FixListCommand` para Fix List deshacible.
  - `LimpiarListaCommand` para limpiar lista deshacible.
  - `UndoRedoManager` con historial de 50 comandos.
  - Atajos de teclado: Ctrl+Z (Undo), Ctrl+Y (Redo).
  - Botón Deshacer muestra descripción de la acción.
- Búsqueda rápida con TextBox anclado correctamente a la interfaz.

### Cambiado
- El TextBox de búsqueda ahora mantiene su posición al redimensionar la ventana.
- Todas las acciones principales ahora son deshacibles.

### Corregido
- El TextBox de búsqueda ya no se queda estático al mover la ventana.

## v0.1.5 - 2026-01-24

### Añadido
- Validación completa de CSV antes de cargar:
  - Verificación de formato, columnas mínimas (4) y tamaño del archivo.
  - Detección de líneas con errores de formato o campos insuficientes.
  - Mensajes de error específicos con números de línea.
  - Advertencias para archivos sin extensión .csv o muy grandes.
  - Resumen de carga (total líneas, válidas, ignoradas).

### Cambiado
- La carga de CSV ahora muestra errores claros y permite cancelar si hay advertencias.

---

## v0.1.4 - 2026-01-24

### Añadido
- Persistencia de preferencias de usuario:
  - Estado de checkboxes (Mantener Datos, Nombres Cortos, Ver Duplicados, Ver Desconocidos).
  - Selección de Title Case.
  - Tamaño y posición de la ventana (solo en estado Normal).

### Cambiado
- Las preferencias se cargan al iniciar y se guardan al salir.

## v0.1.3 - 2026-01-24

### Añadido
- Funcionalidad de archivos recientes.
- Recordar último CSV abierto.
- Menú Archivo con opciones: Abrir, Guardar, Guardar Como, Salir.

### Cambiado
- La aplicación recuerda los últimos archivos CSV utilizados.

## v0.1.2 - 2026-01-24

### Añadido
- Backup automático antes de sobrescribir archivos CSV.
- Los backups se guardan con timestamp: `filename_YYYYMMDD_HHMMSS.csv`.

### Cambiado
- Al guardar CSV, se crea automáticamente un backup del archivo original.

## v0.1.1 - 2026-01-24

### Añadido
- Versión inicial de IgameTools WinForms.
- Carga y guardado de archivos CSV.
- Función Fix List con descarga FTP asíncrona.
- Interfaz básica con lista de juegos.
- Edición individual de juegos.
- Quick Tag para etiquetado múltiple.
- Filtros de visualización (duplicados, desconocidos).
- Opciones de Title Case al guardar. selección al ordenar.
- TextBox de búsqueda deshabilitado hasta que se carga una lista.

### Cambiado
- Ayuda (Help) completamente traducida al español.

## v0.1.0

### Añadido

- Ventana de estado durante **Fix List** (mensajes tipo "Checking database...", "Connected to FTP", etc.).
- Ejecución asíncrona de **Fix List** para evitar bloqueos de la UI.
- Ventana de **Help** con texto completo (similar al original) en un panel con scroll.
- Créditos al autor del proyecto original (referencia a `original/IGame_Tool07a.pb`).
- Archivo `LICENSE` (MIT).
- Workflow de GitHub Actions para build y release (`.github/workflows/build-and-release.yml`).
- Script de instalador Inno Setup (`installer.iss`).
- Guía de compilación/instalador (`Guia_Compilacion_Instalador_IgameToolsWinForms.txt`).

### Cambiado

- Fuente FTP configurada para usar `ftp://ftp.grandis.nu/~Uploads/mrv2k/`.

### Corregido

- Centrado de la ventana de estado respecto a la ventana principal.
- Eliminado el MessageBox final de "Fix List completado" (se muestra "Done." y se cierra automáticamente).
