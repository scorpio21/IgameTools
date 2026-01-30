using System;
using System.Collections.Generic;
using System.IO;

namespace IgameToolsWinForms.Modelos
{
    public class GameData
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string FileCrc { get; set; } = string.Empty;
        public string FileArchiveCrc { get; set; } = string.Empty;
        public bool FileInvalidCrc { get; set; }
        public string FileSubFolder { get; set; } = string.Empty;
        public string FileGenre { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string FileBetaType { get; set; } = string.Empty;
        public bool FileBetaGame { get; set; }
        public bool FileBetaDemo { get; set; }
        public string FileLanguage { get; set; } = string.Empty;
        public bool FileChip { get; set; }
        public bool FileFast { get; set; }
        public bool FileAga { get; set; }
        public bool FileCd32 { get; set; }
        public bool FileCdtv { get; set; }
        public bool FileCdrom { get; set; }
        public bool FileAmiga { get; set; }
        public bool FileArcadia { get; set; }
        public bool FileNtsc { get; set; }
        public bool FileAvailable { get; set; }
        public bool FileFiltered { get; set; }
        public long FileSize { get; set; }
        public bool FileFiles { get; set; }
        public bool FileImage { get; set; }
        public string FileVersion { get; set; } = string.Empty;
        public bool FileIgnore { get; set; }
        public bool FileExtra { get; set; }

        // Memory requirements
        public bool File512K { get; set; }
        public bool File512KB { get; set; }
        public bool File1MB { get; set; }
        public bool File1_5MB { get; set; }
        public bool File1MBCHIP { get; set; }
        public bool File2MB { get; set; }
        public bool File8MB { get; set; }
        public bool File12MB { get; set; }
        public bool FileLowMem { get; set; }
        public bool FileSlowMem { get; set; }

        // Special flags
        public bool FileNoIntro { get; set; }
        public bool FileMT32 { get; set; }
        public bool FileNoVoice { get; set; }
        public bool FileNoSpeech { get; set; }
        public bool FileNoMusic { get; set; }
        public bool FileNoMovie { get; set; }

        // Disk count
        public bool File1Disk { get; set; }
        public bool File2Disk { get; set; }
        public bool File3Disk { get; set; }
        public bool File4Disk { get; set; }

        // Other flags
        public bool FileHiRes { get; set; }
        public bool FileLoRes { get; set; }
        public bool FileGameDemo { get; set; }
        public bool FilePreview { get; set; }
        public bool FilePreRelease { get; set; }
        public bool FileEnhanced { get; set; }
        public bool FileCensored { get; set; }
        public bool FileUnCensored { get; set; }
    }

    public class FilterData
    {
        // Content types
        public bool FGames { get; set; } = true;
        public bool FDemos { get; set; } = true;
        public bool FBetaGame { get; set; } = true;
        public bool FBetaDemo { get; set; } = true;
        public bool FMags { get; set; } = true;

        // System types
        public bool FAGA { get; set; } = true;
        public bool FECS { get; set; } = true;
        public bool FNTSC { get; set; } = true;
        public bool FPAL { get; set; } = true;
        public bool FAmiga { get; set; } = true;
        public bool FArcadia { get; set; } = true;

        // Hardware types
        public bool FFiles { get; set; } = true;
        public bool FImage { get; set; } = true;
        public bool FChip { get; set; } = true;
        public bool FFast { get; set; } = true;
        public bool FCDTV { get; set; } = true;
        public bool FCD32 { get; set; } = true;
        public bool FCDROM { get; set; } = true;

        // Languages
        public bool FCroatian { get; set; } = true;
        public bool FCzech { get; set; } = true;
        public bool FDanish { get; set; } = true;
        public bool FDutch { get; set; } = true;
        public bool FEnglish { get; set; } = true;
        public bool FFinnish { get; set; } = true;
        public bool FFrench { get; set; } = true;
        public bool FGerman { get; set; } = true;
        public bool FGreek { get; set; } = true;
        public bool FItalian { get; set; } = true;
        public bool FMulti { get; set; } = true;
        public bool FPolish { get; set; } = true;
        public bool FSpanish { get; set; } = true;
        public bool FSwedish { get; set; } = true;

        // Memory requirements
        public bool F512K { get; set; } = true;
        public bool F512KB { get; set; } = true;
        public bool F1MB { get; set; } = true;
        public bool F1_5MB { get; set; } = true;
        public bool F1MBCHIP { get; set; } = true;
        public bool F2MB { get; set; } = true;
        public bool F8MB { get; set; } = true;
        public bool F12MB { get; set; } = true;
        public bool FLowMem { get; set; } = true;
        public bool FSlowMem { get; set; } = true;

        // Special flags
        public bool FNoIntro { get; set; } = true;
        public bool FMT32 { get; set; } = true;
        public bool FNoVoice { get; set; } = true;
        public bool FNoSpeech { get; set; } = true;
        public bool FNoMusic { get; set; } = true;
        public bool FNoMovie { get; set; } = true;

        // Disk count
        public bool F1Disk { get; set; } = true;
        public bool F2Disk { get; set; } = true;
        public bool F3Disk { get; set; } = true;
        public bool F4Disk { get; set; } = true;

        // Other flags
        public bool FHiRes { get; set; } = true;
        public bool FLoRes { get; set; } = true;
        public bool FGameDemo { get; set; } = true;
        public bool FPreview { get; set; } = true;
        public bool FPreRelease { get; set; } = true;
        public bool FEnhanced { get; set; } = true;
        public bool FCensored { get; set; } = true;
        public bool FUnCensored { get; set; } = true;
    }

    public class DownData
    {
        public string DownName { get; set; } = string.Empty;
        public string DownType { get; set; } = string.Empty;
        public int DownIndex { get; set; }
        public string DownCrc { get; set; } = string.Empty;
        public string DownFtpCrc { get; set; } = string.Empty;
        public string DownGenre { get; set; } = string.Empty;
        public string DownFolder { get; set; } = string.Empty;
        public long DownSize { get; set; }
        public string DownFtpFolder { get; set; } = string.Empty;
        public string DownHttpFolder { get; set; } = string.Empty;
        public string DownSubFolder1 { get; set; } = string.Empty;
        public string DownSubFolder2 { get; set; } = string.Empty;
        public string DownSubFolder3 { get; set; } = string.Empty;
        public string DownPath { get; set; } = string.Empty;
        public string Down0toZ { get; set; } = string.Empty;
    }

    public class FileData
    {
        public long RFileSize { get; set; }
        public string RFileName { get; set; } = string.Empty;
        public string RFileFile { get; set; } = string.Empty;
        public string RFileCrc32 { get; set; } = string.Empty;
    }

    public class WhdLoadSettings
    {
        public string FtpFolder { get; set; } = "Retroplay WHDLoad Packs";
        public string FtpServer { get; set; } = "ftp2.grandis.nu";
        public string FtpUser { get; set; } = "ftp";
        public string FtpPass { get; set; } = "amiga";
        public bool FtpPassive { get; set; } = true;
        public int FtpPort { get; set; } = 21;
        public string HttpServer { get; set; } = "http://ftp2.grandis.nu/turran/FTP/Retroplay%20WHDLoad%20Packs";
        
        public string WhdFolder { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download"); // Home_Path + "Download\" en el original
        public string FtpGameFolder { get; set; } = "Commodore_Amiga_-_WHDLoad_-_Games";
        public string WhdGameFolder { get; set; } = "Games";
        public string FtpDemoFolder { get; set; } = "Commodore_Amiga_-_WHDLoad_-_Demos";
        public string WhdDemoFolder { get; set; } = "Demos";
        public string FtpBetaGameFolder { get; set; } = "Commodore_Amiga_-_WHDLoad_-_Games_-_Beta_&_Unofficial";
        public string WhdBetaGameFolder { get; set; } = "Beta-Game";
        public string FtpBetaDemoFolder { get; set; } = "Commodore_Amiga_-_WHDLoad_-_Demos_-_Beta_&_Unofficial";
        public string WhdBetaDemoFolder { get; set; } = "Beta-Demo";
        public string FtpMagsFolder { get; set; } = "Commodore_Amiga_-_WHDLoad_-_Magazines";
        public string WhdMagsFolder { get; set; } = "Magazines";

        public int DownloadType { get; set; } = 1; // 0=FTP, 1=HTTP
        public int SortType { get; set; } = 1; // 0=No sorting, 1=Alphabetical, 2=By category, 3=By category (0-Z)
        public int SplitLanguages { get; set; } = 0; // 0=Ignore, 1=Split
        public bool A500Mini { get; set; } = false;
        public string PrefsName { get; set; } = "default.prefs";
        public bool LangBool { get; set; } = true;
    }
}
