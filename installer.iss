; Script de Inno Setup para IgameToolsWinForms
; Requiere Inno Setup 6.x

#define MyAppName "IgameToolsWinForms"
#ifndef MyAppVersion
 #define MyAppVersion "0.1.8"
#endif
#define MyAppPublisher "scorpio21"
#define MyAppExeName "IgameToolsWinForms.exe"

; Carpeta de publicación del ejecutable single-file
#define PublishDir "publish\\win-x64-singlefile"

[Setup]
AppId={{D86A59F8-4D35-4F5D-BD0E-9F4E7B8F63F1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={commonpf64}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=no
DisableProgramGroupPage=no
OutputBaseFilename=IgameTools_{#MyAppVersion}_Setup
OutputDir=.\\publish
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64os

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear icono en el escritorio"; GroupDescription: "Tareas adicionales:"; Flags: unchecked

[Files]
; Archivos de la aplicación (single-file publish)
Source: "{#PublishDir}\\*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion

; Carpeta img con todas sus subcarpetas y archivos
Source: "img\\*"; DestDir: "{app}\\img"; Flags: recursesubdirs createallsubdirs ignoreversion

; Carpeta csv (datos de ejemplo y/o ficheros auxiliares)
Source: "csv\\*"; DestDir: "{app}\\csv"; Flags: recursesubdirs createallsubdirs ignoreversion

; Carpeta Json (configuración y validación)
Source: "Json\\*"; DestDir: "{app}\\Json"; Flags: recursesubdirs createallsubdirs ignoreversion

[Icons]
Name: "{group}\\{#MyAppName}"; Filename: "{app}\\{#MyAppExeName}"
Name: "{group}\\Desinstalar {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\\{#MyAppName}"; Filename: "{app}\\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\\{#MyAppExeName}"; Description: "Ejecutar {#MyAppName}"; Flags: nowait postinstall skipifsilent
