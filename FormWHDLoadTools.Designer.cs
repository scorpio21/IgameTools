namespace IgameToolsWinForms
{
    partial class FormWHDLoadTools
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private CheckBox chkEnglish;
        private CheckBox chkSpanish;
        private CheckBox chkFrench;
        private CheckBox chkGerman;
        private GroupBox groupBoxFilter;
        private GroupBox groupBoxFilterLanguage;
        private CheckBox chkCroatian;
        private CheckBox chkCzech;
        private CheckBox chkDanish;
        private CheckBox chkDutch;
        private CheckBox chkFinnish;
        private CheckBox chkGreek;
        private CheckBox chkItalian;
        private CheckBox chkMulti;
        private CheckBox chkPolish;
        private CheckBox chkSwedish;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWHDLoadTools));
            lstMain = new ListBox();
            statusBar = new StatusStrip();
            lblChipset = new ToolStripStatusLabel();
            lblSystem = new ToolStripStatusLabel();
            lblTVSystem = new ToolStripStatusLabel();
            lblLanguage = new ToolStripStatusLabel();
            lblType = new ToolStripStatusLabel();
            lblStatusInfo = new ToolStripStatusLabel();
            lblSize = new ToolStripStatusLabel();
            lblVersion = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            groupBoxServer = new GroupBox();
            panelServerScroll = new Panel();
            LeneaSeparacion = new Panel();
            lblUserName = new Label();
            txtFtpUser = new TextBox();
            lblPassword = new Label();
            txtFtpPass = new TextBox();
            lblServer = new Label();
            txtFtpServer = new TextBox();
            lblPort = new Label();
            txtFtpPort = new TextBox();
            lblFtpFolder = new Label();
            txtFtpFolder = new TextBox();
            lblHttpPath = new Label();
            txtHttpServer = new TextBox();
            lblGamePath = new Label();
            txtGamePath = new TextBox();
            lblDemoPath = new Label();
            txtDemoPath = new TextBox();
            lblBetaGamePath = new Label();
            txtBetaGamePath = new TextBox();
            lblBetaDemoPath = new Label();
            txtBetaDemoPath = new TextBox();
            lblMagsPath = new Label();
            txtMagsPath = new TextBox();
            groupBoxFolders = new GroupBox();
            panelFoldersScroll = new Panel();
            lblParent = new Label();
            txtWHDMain = new TextBox();
            btnOpenMain = new Button();
            btnSetMain = new Button();
            chkGames = new CheckBox();
            txtWHDGames = new TextBox();
            btnOpenGames = new Button();
            chkDemos = new CheckBox();
            txtWHDDemos = new TextBox();
            btnOpenDemos = new Button();
            chkBetaGames = new CheckBox();
            txtWHDBetaGames = new TextBox();
            btnOpenBetaGames = new Button();
            chkBetaDemos = new CheckBox();
            txtWHDBetaDemos = new TextBox();
            btnOpenBetaDemos = new Button();
            chkMagazines = new CheckBox();
            txtWHDMags = new TextBox();
            btnOpenMags = new Button();
            chkEnglish = new CheckBox();
            chkSpanish = new CheckBox();
            chkFrench = new CheckBox();
            chkGerman = new CheckBox();
            groupBoxFilter = new GroupBox();
            btnResetLang = new Button();
            btnCleaLan = new Button();
            btnLang = new Button();
            panelFilterScroll = new Panel();
            groupBoxFilterMisc = new GroupBox();
            chkCensored = new CheckBox();
            chkUnCensored = new CheckBox();
            chkGameDemo = new CheckBox();
            chkEnhanced = new CheckBox();
            chkPreviewMisc = new CheckBox();
            chkPreRelease = new CheckBox();
            chkNoIntro = new CheckBox();
            chkNoMovie = new CheckBox();
            chkLoRes = new CheckBox();
            chkHiRes = new CheckBox();
            chk4Disk = new CheckBox();
            chk3Disk = new CheckBox();
            chk2Disk = new CheckBox();
            chk1Disk = new CheckBox();
            chkImage = new CheckBox();
            chkFiles = new CheckBox();
            groupBoxFilterMemory = new GroupBox();
            chkLowMen = new CheckBox();
            chkSlowMm = new CheckBox();
            chk12MB = new CheckBox();
            chk8MB = new CheckBox();
            chk2MB = new CheckBox();
            chk15MB = new CheckBox();
            chk1MBChp = new CheckBox();
            chk1MB = new CheckBox();
            chk512KB = new CheckBox();
            chk512k = new CheckBox();
            chkChip = new CheckBox();
            chkFast = new CheckBox();
            groupBoxFilterSound = new GroupBox();
            chkMT32 = new CheckBox();
            chkNoVoice = new CheckBox();
            chkNoSpeech = new CheckBox();
            chkNoMusic = new CheckBox();
            groupBoxFilterChipset = new GroupBox();
            chkAGA = new CheckBox();
            chkECS = new CheckBox();
            chkNTSC = new CheckBox();
            chkPAL = new CheckBox();
            groupBoxFilterSystem = new GroupBox();
            chkAmiga = new CheckBox();
            chkArcadia = new CheckBox();
            chkCD32 = new CheckBox();
            chkCDTV = new CheckBox();
            chkCDROM = new CheckBox();
            groupBoxFilterLanguage = new GroupBox();
            chkCroatian = new CheckBox();
            chkCzech = new CheckBox();
            chkDanish = new CheckBox();
            chkDutch = new CheckBox();
            chkFinnish = new CheckBox();
            chkGreek = new CheckBox();
            chkItalian = new CheckBox();
            chkMulti = new CheckBox();
            chkPolish = new CheckBox();
            chkSwedish = new CheckBox();
            groupBoxSorting = new GroupBox();
            lblSorting = new Label();
            cmbSortType = new ComboBox();
            cmbLanguageSplit = new ComboBox();
            groupBoxActions = new GroupBox();
            btnScan = new Button();
            btnDownload = new Button();
            cmbDownloadType = new ComboBox();
            btnClear = new Button();
            btnSetPath = new Button();
            btnOpenPath = new Button();
            groupMisc = new GroupBox();
            btnAbout = new Button();
            btnHelp = new Button();
            btnSavePrefs = new Button();
            btnLoadPrefs = new Button();
            GroupData = new GroupBox();
            btnMakeFolder = new Button();
            btnClearFilter = new Button();
            groupBoxList = new GroupBox();
            btnEditlist = new Button();
            btnLoadlist = new Button();
            btnSavelist = new Button();
            btnAppendList = new Button();
            btnResetFilter = new Button();
            btnClearEdit = new Button();
            statusBar.SuspendLayout();
            groupBoxServer.SuspendLayout();
            panelServerScroll.SuspendLayout();
            groupBoxFolders.SuspendLayout();
            panelFoldersScroll.SuspendLayout();
            groupBoxFilter.SuspendLayout();
            panelFilterScroll.SuspendLayout();
            groupBoxFilterMisc.SuspendLayout();
            groupBoxFilterMemory.SuspendLayout();
            groupBoxFilterSound.SuspendLayout();
            groupBoxFilterChipset.SuspendLayout();
            groupBoxFilterSystem.SuspendLayout();
            groupBoxFilterLanguage.SuspendLayout();
            groupBoxSorting.SuspendLayout();
            groupBoxActions.SuspendLayout();
            groupMisc.SuspendLayout();
            GroupData.SuspendLayout();
            groupBoxList.SuspendLayout();
            SuspendLayout();
            // 
            // lstMain
            // 
            lstMain.FormattingEnabled = true;
            lstMain.ItemHeight = 25;
            lstMain.Location = new Point(7, 8);
            lstMain.Margin = new Padding(4, 5, 4, 5);
            lstMain.Name = "lstMain";
            lstMain.SelectionMode = SelectionMode.MultiExtended;
            lstMain.Size = new Size(621, 1004);
            lstMain.TabIndex = 1;
            // 
            // statusBar
            // 
            statusBar.ImageScalingSize = new Size(24, 24);
            statusBar.Items.AddRange(new ToolStripItem[] { lblChipset, lblSystem, lblTVSystem, lblLanguage, lblType, lblStatusInfo, lblSize, lblVersion, toolStripStatusLabel1, toolStripStatusLabel2 });
            statusBar.Location = new Point(0, 1010);
            statusBar.Name = "statusBar";
            statusBar.Padding = new Padding(1, 0, 20, 0);
            statusBar.Size = new Size(1353, 32);
            statusBar.TabIndex = 0;
            // 
            // lblChipset
            // 
            lblChipset.Name = "lblChipset";
            lblChipset.Size = new Size(71, 25);
            lblChipset.Text = "Chipset";
            // 
            // lblSystem
            // 
            lblSystem.Name = "lblSystem";
            lblSystem.Size = new Size(69, 25);
            lblSystem.Text = "System";
            // 
            // lblTVSystem
            // 
            lblTVSystem.Name = "lblTVSystem";
            lblTVSystem.Size = new Size(94, 25);
            lblTVSystem.Text = "TV System";
            // 
            // lblLanguage
            // 
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(89, 25);
            lblLanguage.Text = "Language";
            // 
            // lblType
            // 
            lblType.Name = "lblType";
            lblType.Size = new Size(49, 25);
            lblType.Text = "Type";
            // 
            // lblStatusInfo
            // 
            lblStatusInfo.Name = "lblStatusInfo";
            lblStatusInfo.Size = new Size(60, 25);
            lblStatusInfo.Text = "Status";
            // 
            // lblSize
            // 
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(43, 25);
            lblSize.Text = "Size";
            // 
            // lblVersion
            // 
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(70, 25);
            lblVersion.Text = "Version";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(179, 25);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(179, 25);
            toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // groupBoxServer
            // 
            groupBoxServer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxServer.Controls.Add(panelServerScroll);
            groupBoxServer.Location = new Point(636, 8);
            groupBoxServer.Margin = new Padding(4, 5, 4, 5);
            groupBoxServer.Name = "groupBoxServer";
            groupBoxServer.Padding = new Padding(4, 5, 4, 5);
            groupBoxServer.Size = new Size(533, 300);
            groupBoxServer.TabIndex = 2;
            groupBoxServer.TabStop = false;
            groupBoxServer.Text = "Server Settings";
            // 
            // panelServerScroll
            // 
            panelServerScroll.AutoScroll = true;
            panelServerScroll.Controls.Add(LeneaSeparacion);
            panelServerScroll.Controls.Add(lblUserName);
            panelServerScroll.Controls.Add(txtFtpUser);
            panelServerScroll.Controls.Add(lblPassword);
            panelServerScroll.Controls.Add(txtFtpPass);
            panelServerScroll.Controls.Add(lblServer);
            panelServerScroll.Controls.Add(txtFtpServer);
            panelServerScroll.Controls.Add(lblPort);
            panelServerScroll.Controls.Add(txtFtpPort);
            panelServerScroll.Controls.Add(lblFtpFolder);
            panelServerScroll.Controls.Add(txtFtpFolder);
            panelServerScroll.Controls.Add(lblHttpPath);
            panelServerScroll.Controls.Add(txtHttpServer);
            panelServerScroll.Controls.Add(lblGamePath);
            panelServerScroll.Controls.Add(txtGamePath);
            panelServerScroll.Controls.Add(lblDemoPath);
            panelServerScroll.Controls.Add(txtDemoPath);
            panelServerScroll.Controls.Add(lblBetaGamePath);
            panelServerScroll.Controls.Add(txtBetaGamePath);
            panelServerScroll.Controls.Add(lblBetaDemoPath);
            panelServerScroll.Controls.Add(txtBetaDemoPath);
            panelServerScroll.Controls.Add(lblMagsPath);
            panelServerScroll.Controls.Add(txtMagsPath);
            panelServerScroll.Location = new Point(7, 33);
            panelServerScroll.Margin = new Padding(4, 5, 4, 5);
            panelServerScroll.Name = "panelServerScroll";
            panelServerScroll.Size = new Size(518, 258);
            panelServerScroll.TabIndex = 0;
            // 
            // LeneaSeparacion
            // 
            LeneaSeparacion.BackColor = Color.Gray;
            LeneaSeparacion.Location = new Point(7, 219);
            LeneaSeparacion.Name = "LeneaSeparacion";
            LeneaSeparacion.Size = new Size(479, 2);
            LeneaSeparacion.TabIndex = 22;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(5, 12);
            lblUserName.Margin = new Padding(4, 0, 4, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(99, 25);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "User Name";
            // 
            // txtFtpUser
            // 
            txtFtpUser.Location = new Point(121, 6);
            txtFtpUser.Margin = new Padding(4, 5, 4, 5);
            txtFtpUser.Name = "txtFtpUser";
            txtFtpUser.Size = new Size(350, 31);
            txtFtpUser.TabIndex = 1;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(5, 53);
            lblPassword.Margin = new Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(87, 25);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password";
            // 
            // txtFtpPass
            // 
            txtFtpPass.Location = new Point(121, 48);
            txtFtpPass.Margin = new Padding(4, 5, 4, 5);
            txtFtpPass.Name = "txtFtpPass";
            txtFtpPass.Size = new Size(350, 31);
            txtFtpPass.TabIndex = 3;
            txtFtpPass.UseSystemPasswordChar = true;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(5, 95);
            lblServer.Margin = new Padding(4, 0, 4, 0);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(61, 25);
            lblServer.TabIndex = 4;
            lblServer.Text = "Server";
            // 
            // txtFtpServer
            // 
            txtFtpServer.Location = new Point(121, 90);
            txtFtpServer.Margin = new Padding(4, 5, 4, 5);
            txtFtpServer.Name = "txtFtpServer";
            txtFtpServer.Size = new Size(226, 31);
            txtFtpServer.TabIndex = 5;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(356, 93);
            lblPort.Margin = new Padding(4, 0, 4, 0);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(44, 25);
            lblPort.TabIndex = 6;
            lblPort.Text = "Port";
            // 
            // txtFtpPort
            // 
            txtFtpPort.Location = new Point(408, 89);
            txtFtpPort.Margin = new Padding(4, 5, 4, 5);
            txtFtpPort.Name = "txtFtpPort";
            txtFtpPort.Size = new Size(63, 31);
            txtFtpPort.TabIndex = 7;
            // 
            // lblFtpFolder
            // 
            lblFtpFolder.AutoSize = true;
            lblFtpFolder.Location = new Point(5, 137);
            lblFtpFolder.Margin = new Padding(4, 0, 4, 0);
            lblFtpFolder.Name = "lblFtpFolder";
            lblFtpFolder.Size = new Size(95, 25);
            lblFtpFolder.TabIndex = 8;
            lblFtpFolder.Text = "FTP Folder";
            // 
            // txtFtpFolder
            // 
            txtFtpFolder.Location = new Point(121, 131);
            txtFtpFolder.Margin = new Padding(4, 5, 4, 5);
            txtFtpFolder.Name = "txtFtpFolder";
            txtFtpFolder.Size = new Size(350, 31);
            txtFtpFolder.TabIndex = 9;
            // 
            // lblHttpPath
            // 
            lblHttpPath.AutoSize = true;
            lblHttpPath.Location = new Point(5, 178);
            lblHttpPath.Margin = new Padding(4, 0, 4, 0);
            lblHttpPath.Name = "lblHttpPath";
            lblHttpPath.Size = new Size(92, 25);
            lblHttpPath.TabIndex = 10;
            lblHttpPath.Text = "HTTP Path";
            // 
            // txtHttpServer
            // 
            txtHttpServer.Location = new Point(121, 173);
            txtHttpServer.Margin = new Padding(4, 5, 4, 5);
            txtHttpServer.Name = "txtHttpServer";
            txtHttpServer.Size = new Size(350, 31);
            txtHttpServer.TabIndex = 11;
            // 
            // lblGamePath
            // 
            lblGamePath.AutoSize = true;
            lblGamePath.Location = new Point(5, 243);
            lblGamePath.Margin = new Padding(4, 0, 4, 0);
            lblGamePath.Name = "lblGamePath";
            lblGamePath.Size = new Size(97, 25);
            lblGamePath.TabIndex = 12;
            lblGamePath.Text = "Game Path";
            // 
            // txtGamePath
            // 
            txtGamePath.Location = new Point(121, 238);
            txtGamePath.Margin = new Padding(4, 5, 4, 5);
            txtGamePath.Name = "txtGamePath";
            txtGamePath.Size = new Size(350, 31);
            txtGamePath.TabIndex = 13;
            // 
            // lblDemoPath
            // 
            lblDemoPath.AutoSize = true;
            lblDemoPath.Location = new Point(5, 285);
            lblDemoPath.Margin = new Padding(4, 0, 4, 0);
            lblDemoPath.Name = "lblDemoPath";
            lblDemoPath.Size = new Size(100, 25);
            lblDemoPath.TabIndex = 14;
            lblDemoPath.Text = "Demo Path";
            // 
            // txtDemoPath
            // 
            txtDemoPath.Location = new Point(121, 279);
            txtDemoPath.Margin = new Padding(4, 5, 4, 5);
            txtDemoPath.Name = "txtDemoPath";
            txtDemoPath.Size = new Size(350, 31);
            txtDemoPath.TabIndex = 15;
            // 
            // lblBetaGamePath
            // 
            lblBetaGamePath.AutoSize = true;
            lblBetaGamePath.Location = new Point(5, 326);
            lblBetaGamePath.Margin = new Padding(4, 0, 4, 0);
            lblBetaGamePath.Name = "lblBetaGamePath";
            lblBetaGamePath.Size = new Size(112, 25);
            lblBetaGamePath.TabIndex = 16;
            lblBetaGamePath.Text = "ß Game Path";
            // 
            // txtBetaGamePath
            // 
            txtBetaGamePath.Location = new Point(121, 321);
            txtBetaGamePath.Margin = new Padding(4, 5, 4, 5);
            txtBetaGamePath.Name = "txtBetaGamePath";
            txtBetaGamePath.Size = new Size(350, 31);
            txtBetaGamePath.TabIndex = 17;
            // 
            // lblBetaDemoPath
            // 
            lblBetaDemoPath.AutoSize = true;
            lblBetaDemoPath.Location = new Point(5, 368);
            lblBetaDemoPath.Margin = new Padding(4, 0, 4, 0);
            lblBetaDemoPath.Name = "lblBetaDemoPath";
            lblBetaDemoPath.Size = new Size(115, 25);
            lblBetaDemoPath.TabIndex = 18;
            lblBetaDemoPath.Text = "ß Demo Path";
            // 
            // txtBetaDemoPath
            // 
            txtBetaDemoPath.Location = new Point(121, 363);
            txtBetaDemoPath.Margin = new Padding(4, 5, 4, 5);
            txtBetaDemoPath.Name = "txtBetaDemoPath";
            txtBetaDemoPath.Size = new Size(350, 31);
            txtBetaDemoPath.TabIndex = 19;
            // 
            // lblMagsPath
            // 
            lblMagsPath.AutoSize = true;
            lblMagsPath.Location = new Point(5, 405);
            lblMagsPath.Margin = new Padding(4, 0, 4, 0);
            lblMagsPath.Name = "lblMagsPath";
            lblMagsPath.Size = new Size(95, 25);
            lblMagsPath.TabIndex = 20;
            lblMagsPath.Text = "Mags Path";
            // 
            // txtMagsPath
            // 
            txtMagsPath.Location = new Point(121, 402);
            txtMagsPath.Margin = new Padding(4, 5, 4, 5);
            txtMagsPath.Name = "txtMagsPath";
            txtMagsPath.Size = new Size(350, 31);
            txtMagsPath.TabIndex = 21;
            // 
            // groupBoxFolders
            // 
            groupBoxFolders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBoxFolders.Controls.Add(panelFoldersScroll);
            groupBoxFolders.Location = new Point(636, 309);
            groupBoxFolders.Margin = new Padding(4, 5, 4, 5);
            groupBoxFolders.Name = "groupBoxFolders";
            groupBoxFolders.Padding = new Padding(4, 5, 4, 5);
            groupBoxFolders.Size = new Size(533, 212);
            groupBoxFolders.TabIndex = 3;
            groupBoxFolders.TabStop = false;
            groupBoxFolders.Text = "Folder Settings";
            // 
            // panelFoldersScroll
            // 
            panelFoldersScroll.AutoScroll = true;
            panelFoldersScroll.Controls.Add(lblParent);
            panelFoldersScroll.Controls.Add(txtWHDMain);
            panelFoldersScroll.Controls.Add(btnOpenMain);
            panelFoldersScroll.Controls.Add(btnSetMain);
            panelFoldersScroll.Controls.Add(chkGames);
            panelFoldersScroll.Controls.Add(txtWHDGames);
            panelFoldersScroll.Controls.Add(btnOpenGames);
            panelFoldersScroll.Controls.Add(chkDemos);
            panelFoldersScroll.Controls.Add(txtWHDDemos);
            panelFoldersScroll.Controls.Add(btnOpenDemos);
            panelFoldersScroll.Controls.Add(chkBetaGames);
            panelFoldersScroll.Controls.Add(txtWHDBetaGames);
            panelFoldersScroll.Controls.Add(btnOpenBetaGames);
            panelFoldersScroll.Controls.Add(chkBetaDemos);
            panelFoldersScroll.Controls.Add(txtWHDBetaDemos);
            panelFoldersScroll.Controls.Add(btnOpenBetaDemos);
            panelFoldersScroll.Controls.Add(chkMagazines);
            panelFoldersScroll.Controls.Add(txtWHDMags);
            panelFoldersScroll.Controls.Add(btnOpenMags);
            panelFoldersScroll.Location = new Point(7, 33);
            panelFoldersScroll.Margin = new Padding(4, 5, 4, 5);
            panelFoldersScroll.Name = "panelFoldersScroll";
            panelFoldersScroll.Size = new Size(486, 149);
            panelFoldersScroll.TabIndex = 0;
            // 
            // lblParent
            // 
            lblParent.AutoSize = true;
            lblParent.Location = new Point(7, 11);
            lblParent.Margin = new Padding(4, 0, 4, 0);
            lblParent.Name = "lblParent";
            lblParent.Size = new Size(61, 25);
            lblParent.TabIndex = 0;
            lblParent.Text = "Parent";
            // 
            // txtWHDMain
            // 
            txtWHDMain.Location = new Point(70, 8);
            txtWHDMain.Margin = new Padding(4, 5, 4, 5);
            txtWHDMain.Name = "txtWHDMain";
            txtWHDMain.ReadOnly = true;
            txtWHDMain.Size = new Size(264, 31);
            txtWHDMain.TabIndex = 1;
            // 
            // btnOpenMain
            // 
            btnOpenMain.Location = new Point(337, 1);
            btnOpenMain.Margin = new Padding(4, 5, 4, 5);
            btnOpenMain.Name = "btnOpenMain";
            btnOpenMain.Size = new Size(57, 38);
            btnOpenMain.TabIndex = 2;
            btnOpenMain.Text = "Open";
            btnOpenMain.UseVisualStyleBackColor = true;
            // 
            // btnSetMain
            // 
            btnSetMain.Location = new Point(395, 0);
            btnSetMain.Margin = new Padding(4, 5, 4, 5);
            btnSetMain.Name = "btnSetMain";
            btnSetMain.Size = new Size(57, 38);
            btnSetMain.TabIndex = 3;
            btnSetMain.Text = "Set";
            btnSetMain.UseVisualStyleBackColor = true;
            // 
            // chkGames
            // 
            chkGames.AutoSize = true;
            chkGames.Checked = true;
            chkGames.CheckState = CheckState.Checked;
            chkGames.Location = new Point(7, 53);
            chkGames.Margin = new Padding(4, 5, 4, 5);
            chkGames.Name = "chkGames";
            chkGames.Size = new Size(92, 29);
            chkGames.TabIndex = 4;
            chkGames.Text = "Games";
            chkGames.UseVisualStyleBackColor = true;
            // 
            // txtWHDGames
            // 
            txtWHDGames.Location = new Point(112, 50);
            txtWHDGames.Margin = new Padding(4, 5, 4, 5);
            txtWHDGames.Name = "txtWHDGames";
            txtWHDGames.Size = new Size(277, 31);
            txtWHDGames.TabIndex = 5;
            // 
            // btnOpenGames
            // 
            btnOpenGames.Location = new Point(395, 50);
            btnOpenGames.Margin = new Padding(4, 5, 4, 5);
            btnOpenGames.Name = "btnOpenGames";
            btnOpenGames.Size = new Size(57, 38);
            btnOpenGames.TabIndex = 6;
            btnOpenGames.Text = "Open";
            btnOpenGames.UseVisualStyleBackColor = true;
            // 
            // chkDemos
            // 
            chkDemos.AutoSize = true;
            chkDemos.Checked = true;
            chkDemos.CheckState = CheckState.Checked;
            chkDemos.Location = new Point(7, 95);
            chkDemos.Margin = new Padding(4, 5, 4, 5);
            chkDemos.Name = "chkDemos";
            chkDemos.Size = new Size(95, 29);
            chkDemos.TabIndex = 7;
            chkDemos.Text = "Demos";
            chkDemos.UseVisualStyleBackColor = true;
            // 
            // txtWHDDemos
            // 
            txtWHDDemos.Location = new Point(112, 92);
            txtWHDDemos.Margin = new Padding(4, 5, 4, 5);
            txtWHDDemos.Name = "txtWHDDemos";
            txtWHDDemos.Size = new Size(277, 31);
            txtWHDDemos.TabIndex = 8;
            // 
            // btnOpenDemos
            // 
            btnOpenDemos.Location = new Point(395, 88);
            btnOpenDemos.Margin = new Padding(4, 5, 4, 5);
            btnOpenDemos.Name = "btnOpenDemos";
            btnOpenDemos.Size = new Size(57, 38);
            btnOpenDemos.TabIndex = 9;
            btnOpenDemos.Text = "Open";
            btnOpenDemos.UseVisualStyleBackColor = true;
            // 
            // chkBetaGames
            // 
            chkBetaGames.AutoSize = true;
            chkBetaGames.Checked = true;
            chkBetaGames.CheckState = CheckState.Checked;
            chkBetaGames.Location = new Point(7, 137);
            chkBetaGames.Margin = new Padding(4, 5, 4, 5);
            chkBetaGames.Name = "chkBetaGames";
            chkBetaGames.Size = new Size(94, 29);
            chkBetaGames.TabIndex = 10;
            chkBetaGames.Text = "Gameß";
            chkBetaGames.UseVisualStyleBackColor = true;
            // 
            // txtWHDBetaGames
            // 
            txtWHDBetaGames.Location = new Point(112, 133);
            txtWHDBetaGames.Margin = new Padding(4, 5, 4, 5);
            txtWHDBetaGames.Name = "txtWHDBetaGames";
            txtWHDBetaGames.Size = new Size(277, 31);
            txtWHDBetaGames.TabIndex = 11;
            // 
            // btnOpenBetaGames
            // 
            btnOpenBetaGames.Location = new Point(395, 133);
            btnOpenBetaGames.Margin = new Padding(4, 5, 4, 5);
            btnOpenBetaGames.Name = "btnOpenBetaGames";
            btnOpenBetaGames.Size = new Size(57, 38);
            btnOpenBetaGames.TabIndex = 12;
            btnOpenBetaGames.Text = "Open";
            btnOpenBetaGames.UseVisualStyleBackColor = true;
            // 
            // chkBetaDemos
            // 
            chkBetaDemos.AutoSize = true;
            chkBetaDemos.Checked = true;
            chkBetaDemos.CheckState = CheckState.Checked;
            chkBetaDemos.Location = new Point(7, 178);
            chkBetaDemos.Margin = new Padding(4, 5, 4, 5);
            chkBetaDemos.Name = "chkBetaDemos";
            chkBetaDemos.Size = new Size(97, 29);
            chkBetaDemos.TabIndex = 13;
            chkBetaDemos.Text = "Demoß";
            chkBetaDemos.UseVisualStyleBackColor = true;
            // 
            // txtWHDBetaDemos
            // 
            txtWHDBetaDemos.Location = new Point(112, 175);
            txtWHDBetaDemos.Margin = new Padding(4, 5, 4, 5);
            txtWHDBetaDemos.Name = "txtWHDBetaDemos";
            txtWHDBetaDemos.Size = new Size(277, 31);
            txtWHDBetaDemos.TabIndex = 14;
            // 
            // btnOpenBetaDemos
            // 
            btnOpenBetaDemos.Location = new Point(395, 172);
            btnOpenBetaDemos.Margin = new Padding(4, 5, 4, 5);
            btnOpenBetaDemos.Name = "btnOpenBetaDemos";
            btnOpenBetaDemos.Size = new Size(57, 38);
            btnOpenBetaDemos.TabIndex = 15;
            btnOpenBetaDemos.Text = "Open";
            btnOpenBetaDemos.UseVisualStyleBackColor = true;
            // 
            // chkMagazines
            // 
            chkMagazines.AutoSize = true;
            chkMagazines.Checked = true;
            chkMagazines.CheckState = CheckState.Checked;
            chkMagazines.Location = new Point(7, 220);
            chkMagazines.Margin = new Padding(4, 5, 4, 5);
            chkMagazines.Name = "chkMagazines";
            chkMagazines.Size = new Size(82, 29);
            chkMagazines.TabIndex = 16;
            chkMagazines.Text = "Mags";
            chkMagazines.UseVisualStyleBackColor = true;
            // 
            // txtWHDMags
            // 
            txtWHDMags.Location = new Point(112, 217);
            txtWHDMags.Margin = new Padding(4, 5, 4, 5);
            txtWHDMags.Name = "txtWHDMags";
            txtWHDMags.Size = new Size(277, 31);
            txtWHDMags.TabIndex = 17;
            // 
            // btnOpenMags
            // 
            btnOpenMags.Location = new Point(395, 217);
            btnOpenMags.Margin = new Padding(4, 5, 4, 5);
            btnOpenMags.Name = "btnOpenMags";
            btnOpenMags.Size = new Size(57, 38);
            btnOpenMags.TabIndex = 18;
            btnOpenMags.Text = "Open";
            btnOpenMags.UseVisualStyleBackColor = true;
            // 
            // chkEnglish
            // 
            chkEnglish.AutoSize = true;
            chkEnglish.Checked = true;
            chkEnglish.CheckState = CheckState.Checked;
            chkEnglish.Location = new Point(115, 163);
            chkEnglish.Margin = new Padding(4, 5, 4, 5);
            chkEnglish.Name = "chkEnglish";
            chkEnglish.Size = new Size(94, 29);
            chkEnglish.TabIndex = 23;
            chkEnglish.Text = "English";
            chkEnglish.UseVisualStyleBackColor = true;
            // 
            // chkSpanish
            // 
            chkSpanish.AutoSize = true;
            chkSpanish.Checked = true;
            chkSpanish.CheckState = CheckState.Checked;
            chkSpanish.Location = new Point(8, 247);
            chkSpanish.Margin = new Padding(4, 5, 4, 5);
            chkSpanish.Name = "chkSpanish";
            chkSpanish.Size = new Size(100, 29);
            chkSpanish.TabIndex = 24;
            chkSpanish.Text = "Spanish";
            chkSpanish.UseVisualStyleBackColor = true;
            // 
            // chkFrench
            // 
            chkFrench.AutoSize = true;
            chkFrench.Checked = true;
            chkFrench.CheckState = CheckState.Checked;
            chkFrench.Location = new Point(115, 247);
            chkFrench.Margin = new Padding(4, 5, 4, 5);
            chkFrench.Name = "chkFrench";
            chkFrench.Size = new Size(90, 29);
            chkFrench.TabIndex = 25;
            chkFrench.Text = "French";
            chkFrench.UseVisualStyleBackColor = true;
            // 
            // chkGerman
            // 
            chkGerman.AutoSize = true;
            chkGerman.Checked = true;
            chkGerman.CheckState = CheckState.Checked;
            chkGerman.Location = new Point(116, 292);
            chkGerman.Margin = new Padding(4, 5, 4, 5);
            chkGerman.Name = "chkGerman";
            chkGerman.Size = new Size(100, 29);
            chkGerman.TabIndex = 26;
            chkGerman.Text = "German";
            chkGerman.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilter
            // 
            groupBoxFilter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBoxFilter.Controls.Add(btnResetLang);
            groupBoxFilter.Controls.Add(btnCleaLan);
            groupBoxFilter.Controls.Add(btnLang);
            groupBoxFilter.Controls.Add(panelFilterScroll);
            groupBoxFilter.Controls.Add(groupBoxFilterLanguage);
            groupBoxFilter.Location = new Point(636, 582);
            groupBoxFilter.Margin = new Padding(4, 5, 4, 5);
            groupBoxFilter.Name = "groupBoxFilter";
            groupBoxFilter.Padding = new Padding(4, 5, 4, 5);
            groupBoxFilter.Size = new Size(533, 423);
            groupBoxFilter.TabIndex = 6;
            groupBoxFilter.TabStop = false;
            groupBoxFilter.Text = "Filter";
            // 
            // btnResetLang
            // 
            btnResetLang.Location = new Point(457, 373);
            btnResetLang.Name = "btnResetLang";
            btnResetLang.Size = new Size(68, 34);
            btnResetLang.TabIndex = 7;
            btnResetLang.Text = "Reset";
            btnResetLang.UseVisualStyleBackColor = true;
            // 
            // btnCleaLan
            // 
            btnCleaLan.Location = new Point(383, 372);
            btnCleaLan.Name = "btnCleaLan";
            btnCleaLan.Size = new Size(68, 34);
            btnCleaLan.TabIndex = 6;
            btnCleaLan.Tag = "";
            btnCleaLan.Text = "Clear";
            btnCleaLan.UseVisualStyleBackColor = true;
            // 
            // btnLang
            // 
            btnLang.Location = new Point(309, 372);
            btnLang.Name = "btnLang";
            btnLang.Size = new Size(68, 34);
            btnLang.TabIndex = 5;
            btnLang.Text = "Lang.";
            btnLang.UseVisualStyleBackColor = true;
            // 
            // panelFilterScroll
            // 
            panelFilterScroll.AutoScroll = true;
            panelFilterScroll.Controls.Add(groupBoxFilterMisc);
            panelFilterScroll.Controls.Add(groupBoxFilterMemory);
            panelFilterScroll.Controls.Add(groupBoxFilterSound);
            panelFilterScroll.Controls.Add(groupBoxFilterChipset);
            panelFilterScroll.Controls.Add(groupBoxFilterSystem);
            panelFilterScroll.Location = new Point(0, 24);
            panelFilterScroll.Margin = new Padding(4, 5, 4, 5);
            panelFilterScroll.Name = "panelFilterScroll";
            panelFilterScroll.Size = new Size(287, 412);
            panelFilterScroll.TabIndex = 4;
            // 
            // groupBoxFilterMisc
            // 
            groupBoxFilterMisc.Controls.Add(chkCensored);
            groupBoxFilterMisc.Controls.Add(chkUnCensored);
            groupBoxFilterMisc.Controls.Add(chkGameDemo);
            groupBoxFilterMisc.Controls.Add(chkEnhanced);
            groupBoxFilterMisc.Controls.Add(chkPreviewMisc);
            groupBoxFilterMisc.Controls.Add(chkPreRelease);
            groupBoxFilterMisc.Controls.Add(chkNoIntro);
            groupBoxFilterMisc.Controls.Add(chkNoMovie);
            groupBoxFilterMisc.Controls.Add(chkLoRes);
            groupBoxFilterMisc.Controls.Add(chkHiRes);
            groupBoxFilterMisc.Controls.Add(chk4Disk);
            groupBoxFilterMisc.Controls.Add(chk3Disk);
            groupBoxFilterMisc.Controls.Add(chk2Disk);
            groupBoxFilterMisc.Controls.Add(chk1Disk);
            groupBoxFilterMisc.Controls.Add(chkImage);
            groupBoxFilterMisc.Controls.Add(chkFiles);
            groupBoxFilterMisc.Location = new Point(7, 660);
            groupBoxFilterMisc.Name = "groupBoxFilterMisc";
            groupBoxFilterMisc.Size = new Size(237, 302);
            groupBoxFilterMisc.TabIndex = 7;
            groupBoxFilterMisc.TabStop = false;
            groupBoxFilterMisc.Text = "Misc";
            // 
            // chkCensored
            // 
            chkCensored.AutoSize = true;
            chkCensored.Checked = true;
            chkCensored.CheckState = CheckState.Checked;
            chkCensored.Location = new Point(115, 265);
            chkCensored.Name = "chkCensored";
            chkCensored.Size = new Size(97, 29);
            chkCensored.TabIndex = 15;
            chkCensored.Text = "Censor.";
            chkCensored.UseVisualStyleBackColor = true;
            // 
            // chkUnCensored
            // 
            chkUnCensored.AutoSize = true;
            chkUnCensored.Checked = true;
            chkUnCensored.CheckState = CheckState.Checked;
            chkUnCensored.Location = new Point(2, 265);
            chkUnCensored.Name = "chkUnCensored";
            chkUnCensored.Size = new Size(102, 29);
            chkUnCensored.TabIndex = 14;
            chkUnCensored.Text = "UnCens.";
            chkUnCensored.UseVisualStyleBackColor = true;
            // 
            // chkGameDemo
            // 
            chkGameDemo.AutoSize = true;
            chkGameDemo.Checked = true;
            chkGameDemo.CheckState = CheckState.Checked;
            chkGameDemo.Location = new Point(115, 230);
            chkGameDemo.Name = "chkGameDemo";
            chkGameDemo.Size = new Size(106, 29);
            chkGameDemo.TabIndex = 13;
            chkGameDemo.Text = "GmDmo";
            chkGameDemo.UseVisualStyleBackColor = true;
            // 
            // chkEnhanced
            // 
            chkEnhanced.AutoSize = true;
            chkEnhanced.Checked = true;
            chkEnhanced.CheckState = CheckState.Checked;
            chkEnhanced.Location = new Point(2, 230);
            chkEnhanced.Name = "chkEnhanced";
            chkEnhanced.Size = new Size(114, 29);
            chkEnhanced.TabIndex = 12;
            chkEnhanced.Text = "Enhanced";
            chkEnhanced.UseVisualStyleBackColor = true;
            // 
            // chkPreviewMisc
            // 
            chkPreviewMisc.AutoSize = true;
            chkPreviewMisc.Checked = true;
            chkPreviewMisc.CheckState = CheckState.Checked;
            chkPreviewMisc.Location = new Point(113, 195);
            chkPreviewMisc.Name = "chkPreviewMisc";
            chkPreviewMisc.Size = new Size(98, 29);
            chkPreviewMisc.TabIndex = 11;
            chkPreviewMisc.Text = "Preview";
            chkPreviewMisc.UseVisualStyleBackColor = true;
            // 
            // chkPreRelease
            // 
            chkPreRelease.AutoSize = true;
            chkPreRelease.Checked = true;
            chkPreRelease.CheckState = CheckState.Checked;
            chkPreRelease.Location = new Point(2, 195);
            chkPreRelease.Name = "chkPreRelease";
            chkPreRelease.Size = new Size(90, 29);
            chkPreRelease.TabIndex = 10;
            chkPreRelease.Text = "PreRel.";
            chkPreRelease.UseVisualStyleBackColor = true;
            // 
            // chkNoIntro
            // 
            chkNoIntro.AutoSize = true;
            chkNoIntro.Checked = true;
            chkNoIntro.CheckState = CheckState.Checked;
            chkNoIntro.Location = new Point(115, 155);
            chkNoIntro.Name = "chkNoIntro";
            chkNoIntro.Size = new Size(100, 29);
            chkNoIntro.TabIndex = 9;
            chkNoIntro.Text = "NoIntro";
            chkNoIntro.UseVisualStyleBackColor = true;
            // 
            // chkNoMovie
            // 
            chkNoMovie.AutoSize = true;
            chkNoMovie.Checked = true;
            chkNoMovie.CheckState = CheckState.Checked;
            chkNoMovie.Location = new Point(2, 158);
            chkNoMovie.Margin = new Padding(4, 5, 4, 5);
            chkNoMovie.Name = "chkNoMovie";
            chkNoMovie.Size = new Size(116, 29);
            chkNoMovie.TabIndex = 4;
            chkNoMovie.Text = "No Movie";
            chkNoMovie.UseVisualStyleBackColor = true;
            // 
            // chkLoRes
            // 
            chkLoRes.AutoSize = true;
            chkLoRes.Checked = true;
            chkLoRes.CheckState = CheckState.Checked;
            chkLoRes.Location = new Point(113, 121);
            chkLoRes.Name = "chkLoRes";
            chkLoRes.Size = new Size(84, 29);
            chkLoRes.TabIndex = 7;
            chkLoRes.Text = "LoRes";
            chkLoRes.UseVisualStyleBackColor = true;
            // 
            // chkHiRes
            // 
            chkHiRes.AutoSize = true;
            chkHiRes.Checked = true;
            chkHiRes.CheckState = CheckState.Checked;
            chkHiRes.Location = new Point(2, 121);
            chkHiRes.Name = "chkHiRes";
            chkHiRes.Size = new Size(82, 29);
            chkHiRes.TabIndex = 6;
            chkHiRes.Text = "HiRes";
            chkHiRes.UseVisualStyleBackColor = true;
            // 
            // chk4Disk
            // 
            chk4Disk.AutoSize = true;
            chk4Disk.Checked = true;
            chk4Disk.CheckState = CheckState.Checked;
            chk4Disk.Location = new Point(113, 86);
            chk4Disk.Name = "chk4Disk";
            chk4Disk.Size = new Size(87, 29);
            chk4Disk.TabIndex = 5;
            chk4Disk.Text = "4 Disk";
            chk4Disk.UseVisualStyleBackColor = true;
            // 
            // chk3Disk
            // 
            chk3Disk.AutoSize = true;
            chk3Disk.Checked = true;
            chk3Disk.CheckState = CheckState.Checked;
            chk3Disk.Location = new Point(2, 86);
            chk3Disk.Name = "chk3Disk";
            chk3Disk.Size = new Size(87, 29);
            chk3Disk.TabIndex = 4;
            chk3Disk.Text = "3 Disk";
            chk3Disk.UseVisualStyleBackColor = true;
            // 
            // chk2Disk
            // 
            chk2Disk.AutoSize = true;
            chk2Disk.Checked = true;
            chk2Disk.CheckState = CheckState.Checked;
            chk2Disk.Location = new Point(113, 55);
            chk2Disk.Name = "chk2Disk";
            chk2Disk.Size = new Size(87, 29);
            chk2Disk.TabIndex = 3;
            chk2Disk.Text = "2 Disk";
            chk2Disk.UseVisualStyleBackColor = true;
            // 
            // chk1Disk
            // 
            chk1Disk.AutoSize = true;
            chk1Disk.Checked = true;
            chk1Disk.CheckState = CheckState.Checked;
            chk1Disk.Location = new Point(1, 55);
            chk1Disk.Name = "chk1Disk";
            chk1Disk.Size = new Size(87, 29);
            chk1Disk.TabIndex = 2;
            chk1Disk.Text = "1 Disk";
            chk1Disk.UseVisualStyleBackColor = true;
            // 
            // chkImage
            // 
            chkImage.AutoSize = true;
            chkImage.Checked = true;
            chkImage.CheckState = CheckState.Checked;
            chkImage.Location = new Point(113, 23);
            chkImage.Name = "chkImage";
            chkImage.Size = new Size(88, 29);
            chkImage.TabIndex = 1;
            chkImage.Text = "Image";
            chkImage.UseVisualStyleBackColor = true;
            // 
            // chkFiles
            // 
            chkFiles.AutoSize = true;
            chkFiles.Checked = true;
            chkFiles.CheckState = CheckState.Checked;
            chkFiles.Location = new Point(2, 23);
            chkFiles.Name = "chkFiles";
            chkFiles.Size = new Size(72, 29);
            chkFiles.TabIndex = 0;
            chkFiles.Text = "Files";
            chkFiles.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilterMemory
            // 
            groupBoxFilterMemory.Controls.Add(chkLowMen);
            groupBoxFilterMemory.Controls.Add(chkSlowMm);
            groupBoxFilterMemory.Controls.Add(chk12MB);
            groupBoxFilterMemory.Controls.Add(chk8MB);
            groupBoxFilterMemory.Controls.Add(chk2MB);
            groupBoxFilterMemory.Controls.Add(chk15MB);
            groupBoxFilterMemory.Controls.Add(chk1MBChp);
            groupBoxFilterMemory.Controls.Add(chk1MB);
            groupBoxFilterMemory.Controls.Add(chk512KB);
            groupBoxFilterMemory.Controls.Add(chk512k);
            groupBoxFilterMemory.Controls.Add(chkChip);
            groupBoxFilterMemory.Controls.Add(chkFast);
            groupBoxFilterMemory.Location = new Point(7, 384);
            groupBoxFilterMemory.Name = "groupBoxFilterMemory";
            groupBoxFilterMemory.Size = new Size(245, 270);
            groupBoxFilterMemory.TabIndex = 4;
            groupBoxFilterMemory.TabStop = false;
            groupBoxFilterMemory.Text = "Memory";
            // 
            // chkLowMen
            // 
            chkLowMen.AutoSize = true;
            chkLowMen.Checked = true;
            chkLowMen.CheckState = CheckState.Checked;
            chkLowMen.Location = new Point(8, 230);
            chkLowMen.Margin = new Padding(4, 5, 4, 5);
            chkLowMen.Name = "chkLowMen";
            chkLowMen.Size = new Size(111, 29);
            chkLowMen.TabIndex = 12;
            chkLowMen.Text = "LowMem";
            chkLowMen.UseVisualStyleBackColor = true;
            // 
            // chkSlowMm
            // 
            chkSlowMm.AutoSize = true;
            chkSlowMm.Checked = true;
            chkSlowMm.CheckState = CheckState.Checked;
            chkSlowMm.Location = new Point(122, 230);
            chkSlowMm.Margin = new Padding(4, 5, 4, 5);
            chkSlowMm.Name = "chkSlowMm";
            chkSlowMm.Size = new Size(117, 29);
            chkSlowMm.TabIndex = 11;
            chkSlowMm.Text = "SlowMem";
            chkSlowMm.UseVisualStyleBackColor = true;
            // 
            // chk12MB
            // 
            chk12MB.AutoSize = true;
            chk12MB.Checked = true;
            chk12MB.CheckState = CheckState.Checked;
            chk12MB.Location = new Point(122, 191);
            chk12MB.Margin = new Padding(4, 5, 4, 5);
            chk12MB.Name = "chk12MB";
            chk12MB.Size = new Size(84, 29);
            chk12MB.TabIndex = 10;
            chk12MB.Text = "12MB";
            chk12MB.UseVisualStyleBackColor = true;
            // 
            // chk8MB
            // 
            chk8MB.AutoSize = true;
            chk8MB.Checked = true;
            chk8MB.CheckState = CheckState.Checked;
            chk8MB.Location = new Point(8, 191);
            chk8MB.Margin = new Padding(4, 5, 4, 5);
            chk8MB.Name = "chk8MB";
            chk8MB.Size = new Size(74, 29);
            chk8MB.TabIndex = 9;
            chk8MB.Text = "8MB";
            chk8MB.UseVisualStyleBackColor = true;
            // 
            // chk2MB
            // 
            chk2MB.AutoSize = true;
            chk2MB.Checked = true;
            chk2MB.CheckState = CheckState.Checked;
            chk2MB.Location = new Point(122, 152);
            chk2MB.Margin = new Padding(4, 5, 4, 5);
            chk2MB.Name = "chk2MB";
            chk2MB.Size = new Size(74, 29);
            chk2MB.TabIndex = 8;
            chk2MB.Text = "2MB";
            chk2MB.UseVisualStyleBackColor = true;
            // 
            // chk15MB
            // 
            chk15MB.AutoSize = true;
            chk15MB.Checked = true;
            chk15MB.CheckState = CheckState.Checked;
            chk15MB.Location = new Point(8, 152);
            chk15MB.Margin = new Padding(4, 5, 4, 5);
            chk15MB.Name = "chk15MB";
            chk15MB.Size = new Size(88, 29);
            chk15MB.TabIndex = 7;
            chk15MB.Text = "1.5MB";
            chk15MB.UseVisualStyleBackColor = true;
            // 
            // chk1MBChp
            // 
            chk1MBChp.AutoSize = true;
            chk1MBChp.Checked = true;
            chk1MBChp.CheckState = CheckState.Checked;
            chk1MBChp.Location = new Point(122, 113);
            chk1MBChp.Margin = new Padding(4, 5, 4, 5);
            chk1MBChp.Name = "chk1MBChp";
            chk1MBChp.Size = new Size(106, 29);
            chk1MBChp.TabIndex = 6;
            chk1MBChp.Text = "1MBChp";
            chk1MBChp.UseVisualStyleBackColor = true;
            // 
            // chk1MB
            // 
            chk1MB.AutoSize = true;
            chk1MB.Checked = true;
            chk1MB.CheckState = CheckState.Checked;
            chk1MB.Location = new Point(8, 113);
            chk1MB.Margin = new Padding(4, 5, 4, 5);
            chk1MB.Name = "chk1MB";
            chk1MB.Size = new Size(74, 29);
            chk1MB.TabIndex = 5;
            chk1MB.Text = "1MB";
            chk1MB.UseVisualStyleBackColor = true;
            // 
            // chk512KB
            // 
            chk512KB.AutoSize = true;
            chk512KB.Checked = true;
            chk512KB.CheckState = CheckState.Checked;
            chk512KB.Location = new Point(122, 74);
            chk512KB.Margin = new Padding(4, 5, 4, 5);
            chk512KB.Name = "chk512KB";
            chk512KB.Size = new Size(88, 29);
            chk512KB.TabIndex = 4;
            chk512KB.Text = "512KB";
            chk512KB.UseVisualStyleBackColor = true;
            // 
            // chk512k
            // 
            chk512k.AutoSize = true;
            chk512k.Checked = true;
            chk512k.CheckState = CheckState.Checked;
            chk512k.Location = new Point(8, 74);
            chk512k.Margin = new Padding(4, 5, 4, 5);
            chk512k.Name = "chk512k";
            chk512k.Size = new Size(77, 29);
            chk512k.TabIndex = 3;
            chk512k.Text = "512k";
            chk512k.UseVisualStyleBackColor = true;
            // 
            // chkChip
            // 
            chkChip.AutoSize = true;
            chkChip.Checked = true;
            chkChip.CheckState = CheckState.Checked;
            chkChip.Location = new Point(8, 35);
            chkChip.Margin = new Padding(4, 5, 4, 5);
            chkChip.Name = "chkChip";
            chkChip.Size = new Size(74, 29);
            chkChip.TabIndex = 2;
            chkChip.Text = "Chip";
            chkChip.UseVisualStyleBackColor = true;
            // 
            // chkFast
            // 
            chkFast.AutoSize = true;
            chkFast.Checked = true;
            chkFast.CheckState = CheckState.Checked;
            chkFast.Location = new Point(122, 35);
            chkFast.Margin = new Padding(4, 5, 4, 5);
            chkFast.Name = "chkFast";
            chkFast.Size = new Size(69, 29);
            chkFast.TabIndex = 1;
            chkFast.Text = "Fast";
            chkFast.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilterSound
            // 
            groupBoxFilterSound.Controls.Add(chkMT32);
            groupBoxFilterSound.Controls.Add(chkNoVoice);
            groupBoxFilterSound.Controls.Add(chkNoSpeech);
            groupBoxFilterSound.Controls.Add(chkNoMusic);
            groupBoxFilterSound.Location = new Point(7, 276);
            groupBoxFilterSound.Margin = new Padding(4, 5, 4, 5);
            groupBoxFilterSound.Name = "groupBoxFilterSound";
            groupBoxFilterSound.Padding = new Padding(4, 5, 4, 5);
            groupBoxFilterSound.Size = new Size(245, 106);
            groupBoxFilterSound.TabIndex = 3;
            groupBoxFilterSound.TabStop = false;
            groupBoxFilterSound.Text = "Sound";
            // 
            // chkMT32
            // 
            chkMT32.AutoSize = true;
            chkMT32.Checked = true;
            chkMT32.CheckState = CheckState.Checked;
            chkMT32.Location = new Point(8, 37);
            chkMT32.Margin = new Padding(4, 5, 4, 5);
            chkMT32.Name = "chkMT32";
            chkMT32.Size = new Size(83, 29);
            chkMT32.TabIndex = 0;
            chkMT32.Text = "MT32";
            chkMT32.UseVisualStyleBackColor = true;
            // 
            // chkNoVoice
            // 
            chkNoVoice.AutoSize = true;
            chkNoVoice.Checked = true;
            chkNoVoice.CheckState = CheckState.Checked;
            chkNoVoice.Location = new Point(122, 72);
            chkNoVoice.Margin = new Padding(4, 5, 4, 5);
            chkNoVoice.Name = "chkNoVoice";
            chkNoVoice.Size = new Size(109, 29);
            chkNoVoice.TabIndex = 1;
            chkNoVoice.Text = "No Voice";
            chkNoVoice.UseVisualStyleBackColor = true;
            // 
            // chkNoSpeech
            // 
            chkNoSpeech.AutoSize = true;
            chkNoSpeech.Checked = true;
            chkNoSpeech.CheckState = CheckState.Checked;
            chkNoSpeech.Location = new Point(8, 70);
            chkNoSpeech.Margin = new Padding(4, 5, 4, 5);
            chkNoSpeech.Name = "chkNoSpeech";
            chkNoSpeech.Size = new Size(119, 29);
            chkNoSpeech.TabIndex = 2;
            chkNoSpeech.Text = "NoSpeech";
            chkNoSpeech.UseVisualStyleBackColor = true;
            // 
            // chkNoMusic
            // 
            chkNoMusic.AutoSize = true;
            chkNoMusic.Checked = true;
            chkNoMusic.CheckState = CheckState.Checked;
            chkNoMusic.Location = new Point(122, 36);
            chkNoMusic.Margin = new Padding(4, 5, 4, 5);
            chkNoMusic.Name = "chkNoMusic";
            chkNoMusic.Size = new Size(113, 29);
            chkNoMusic.TabIndex = 3;
            chkNoMusic.Text = "No Music";
            chkNoMusic.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilterChipset
            // 
            groupBoxFilterChipset.Controls.Add(chkAGA);
            groupBoxFilterChipset.Controls.Add(chkECS);
            groupBoxFilterChipset.Controls.Add(chkNTSC);
            groupBoxFilterChipset.Controls.Add(chkPAL);
            groupBoxFilterChipset.Location = new Point(7, 148);
            groupBoxFilterChipset.Margin = new Padding(4, 5, 4, 5);
            groupBoxFilterChipset.Name = "groupBoxFilterChipset";
            groupBoxFilterChipset.Padding = new Padding(4, 5, 4, 5);
            groupBoxFilterChipset.Size = new Size(245, 118);
            groupBoxFilterChipset.TabIndex = 2;
            groupBoxFilterChipset.TabStop = false;
            groupBoxFilterChipset.Text = "Chipset";
            // 
            // chkAGA
            // 
            chkAGA.AutoSize = true;
            chkAGA.Checked = true;
            chkAGA.CheckState = CheckState.Checked;
            chkAGA.Location = new Point(8, 37);
            chkAGA.Margin = new Padding(4, 5, 4, 5);
            chkAGA.Name = "chkAGA";
            chkAGA.Size = new Size(74, 29);
            chkAGA.TabIndex = 19;
            chkAGA.Text = "AGA";
            chkAGA.UseVisualStyleBackColor = true;
            // 
            // chkECS
            // 
            chkECS.AutoSize = true;
            chkECS.Checked = true;
            chkECS.CheckState = CheckState.Checked;
            chkECS.Location = new Point(122, 37);
            chkECS.Margin = new Padding(4, 5, 4, 5);
            chkECS.Name = "chkECS";
            chkECS.Size = new Size(68, 29);
            chkECS.TabIndex = 20;
            chkECS.Text = "ECS";
            chkECS.UseVisualStyleBackColor = true;
            // 
            // chkNTSC
            // 
            chkNTSC.AutoSize = true;
            chkNTSC.Checked = true;
            chkNTSC.CheckState = CheckState.Checked;
            chkNTSC.Location = new Point(8, 76);
            chkNTSC.Margin = new Padding(4, 5, 4, 5);
            chkNTSC.Name = "chkNTSC";
            chkNTSC.Size = new Size(81, 29);
            chkNTSC.TabIndex = 21;
            chkNTSC.Text = "NTSC";
            chkNTSC.UseVisualStyleBackColor = true;
            // 
            // chkPAL
            // 
            chkPAL.AutoSize = true;
            chkPAL.Checked = true;
            chkPAL.CheckState = CheckState.Checked;
            chkPAL.Location = new Point(122, 76);
            chkPAL.Margin = new Padding(4, 5, 4, 5);
            chkPAL.Name = "chkPAL";
            chkPAL.Size = new Size(67, 29);
            chkPAL.TabIndex = 22;
            chkPAL.Text = "PAL";
            chkPAL.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilterSystem
            // 
            groupBoxFilterSystem.Controls.Add(chkAmiga);
            groupBoxFilterSystem.Controls.Add(chkArcadia);
            groupBoxFilterSystem.Controls.Add(chkCD32);
            groupBoxFilterSystem.Controls.Add(chkCDTV);
            groupBoxFilterSystem.Controls.Add(chkCDROM);
            groupBoxFilterSystem.Location = new Point(7, 6);
            groupBoxFilterSystem.Margin = new Padding(4, 5, 4, 5);
            groupBoxFilterSystem.Name = "groupBoxFilterSystem";
            groupBoxFilterSystem.Padding = new Padding(4, 5, 4, 5);
            groupBoxFilterSystem.Size = new Size(245, 144);
            groupBoxFilterSystem.TabIndex = 1;
            groupBoxFilterSystem.TabStop = false;
            groupBoxFilterSystem.Text = "System";
            // 
            // chkAmiga
            // 
            chkAmiga.AutoSize = true;
            chkAmiga.Checked = true;
            chkAmiga.CheckState = CheckState.Checked;
            chkAmiga.Location = new Point(8, 37);
            chkAmiga.Margin = new Padding(4, 5, 4, 5);
            chkAmiga.Name = "chkAmiga";
            chkAmiga.Size = new Size(90, 29);
            chkAmiga.TabIndex = 0;
            chkAmiga.Text = "Amiga";
            chkAmiga.UseVisualStyleBackColor = true;
            // 
            // chkArcadia
            // 
            chkArcadia.AutoSize = true;
            chkArcadia.Checked = true;
            chkArcadia.CheckState = CheckState.Checked;
            chkArcadia.Location = new Point(122, 34);
            chkArcadia.Margin = new Padding(4, 5, 4, 5);
            chkArcadia.Name = "chkArcadia";
            chkArcadia.Size = new Size(97, 29);
            chkArcadia.TabIndex = 1;
            chkArcadia.Text = "Arcadia";
            chkArcadia.UseVisualStyleBackColor = true;
            // 
            // chkCD32
            // 
            chkCD32.AutoSize = true;
            chkCD32.Checked = true;
            chkCD32.CheckState = CheckState.Checked;
            chkCD32.Location = new Point(8, 76);
            chkCD32.Margin = new Padding(4, 5, 4, 5);
            chkCD32.Name = "chkCD32";
            chkCD32.Size = new Size(82, 29);
            chkCD32.TabIndex = 2;
            chkCD32.Text = "CD32";
            chkCD32.UseVisualStyleBackColor = true;
            // 
            // chkCDTV
            // 
            chkCDTV.AutoSize = true;
            chkCDTV.Checked = true;
            chkCDTV.CheckState = CheckState.Checked;
            chkCDTV.Location = new Point(122, 76);
            chkCDTV.Margin = new Padding(4, 5, 4, 5);
            chkCDTV.Name = "chkCDTV";
            chkCDTV.Size = new Size(81, 29);
            chkCDTV.TabIndex = 3;
            chkCDTV.Text = "CDTV";
            chkCDTV.UseVisualStyleBackColor = true;
            // 
            // chkCDROM
            // 
            chkCDROM.AutoSize = true;
            chkCDROM.Checked = true;
            chkCDROM.CheckState = CheckState.Checked;
            chkCDROM.Location = new Point(8, 115);
            chkCDROM.Margin = new Padding(4, 5, 4, 5);
            chkCDROM.Name = "chkCDROM";
            chkCDROM.Size = new Size(103, 29);
            chkCDROM.TabIndex = 4;
            chkCDROM.Text = "CDROM";
            chkCDROM.UseVisualStyleBackColor = true;
            // 
            // groupBoxFilterLanguage
            // 
            groupBoxFilterLanguage.Controls.Add(chkEnglish);
            groupBoxFilterLanguage.Controls.Add(chkSpanish);
            groupBoxFilterLanguage.Controls.Add(chkFrench);
            groupBoxFilterLanguage.Controls.Add(chkGerman);
            groupBoxFilterLanguage.Controls.Add(chkCroatian);
            groupBoxFilterLanguage.Controls.Add(chkCzech);
            groupBoxFilterLanguage.Controls.Add(chkDanish);
            groupBoxFilterLanguage.Controls.Add(chkDutch);
            groupBoxFilterLanguage.Controls.Add(chkFinnish);
            groupBoxFilterLanguage.Controls.Add(chkGreek);
            groupBoxFilterLanguage.Controls.Add(chkItalian);
            groupBoxFilterLanguage.Controls.Add(chkMulti);
            groupBoxFilterLanguage.Controls.Add(chkPolish);
            groupBoxFilterLanguage.Controls.Add(chkSwedish);
            groupBoxFilterLanguage.Location = new Point(309, 24);
            groupBoxFilterLanguage.Margin = new Padding(4, 5, 4, 5);
            groupBoxFilterLanguage.Name = "groupBoxFilterLanguage";
            groupBoxFilterLanguage.Padding = new Padding(4, 5, 4, 5);
            groupBoxFilterLanguage.Size = new Size(216, 331);
            groupBoxFilterLanguage.TabIndex = 3;
            groupBoxFilterLanguage.TabStop = false;
            groupBoxFilterLanguage.Text = "Language";
            // 
            // chkCroatian
            // 
            chkCroatian.AutoSize = true;
            chkCroatian.Checked = true;
            chkCroatian.CheckState = CheckState.Checked;
            chkCroatian.Location = new Point(8, 31);
            chkCroatian.Margin = new Padding(4, 5, 4, 5);
            chkCroatian.Name = "chkCroatian";
            chkCroatian.Size = new Size(104, 29);
            chkCroatian.TabIndex = 4;
            chkCroatian.Text = "Croatian";
            chkCroatian.UseVisualStyleBackColor = true;
            // 
            // chkCzech
            // 
            chkCzech.AutoSize = true;
            chkCzech.Checked = true;
            chkCzech.CheckState = CheckState.Checked;
            chkCzech.Location = new Point(116, 31);
            chkCzech.Margin = new Padding(4, 5, 4, 5);
            chkCzech.Name = "chkCzech";
            chkCzech.Size = new Size(84, 29);
            chkCzech.TabIndex = 5;
            chkCzech.Text = "Czech";
            chkCzech.UseVisualStyleBackColor = true;
            // 
            // chkDanish
            // 
            chkDanish.AutoSize = true;
            chkDanish.Checked = true;
            chkDanish.CheckState = CheckState.Checked;
            chkDanish.Location = new Point(116, 71);
            chkDanish.Margin = new Padding(4, 5, 4, 5);
            chkDanish.Name = "chkDanish";
            chkDanish.Size = new Size(92, 29);
            chkDanish.TabIndex = 6;
            chkDanish.Text = "Danish";
            chkDanish.UseVisualStyleBackColor = true;
            // 
            // chkDutch
            // 
            chkDutch.AutoSize = true;
            chkDutch.Checked = true;
            chkDutch.CheckState = CheckState.Checked;
            chkDutch.Location = new Point(116, 118);
            chkDutch.Margin = new Padding(4, 5, 4, 5);
            chkDutch.Name = "chkDutch";
            chkDutch.Size = new Size(85, 29);
            chkDutch.TabIndex = 7;
            chkDutch.Text = "Dutch";
            chkDutch.UseVisualStyleBackColor = true;
            // 
            // chkFinnish
            // 
            chkFinnish.AutoSize = true;
            chkFinnish.Checked = true;
            chkFinnish.CheckState = CheckState.Checked;
            chkFinnish.Location = new Point(116, 205);
            chkFinnish.Margin = new Padding(4, 5, 4, 5);
            chkFinnish.Name = "chkFinnish";
            chkFinnish.Size = new Size(93, 29);
            chkFinnish.TabIndex = 8;
            chkFinnish.Text = "Finnish";
            chkFinnish.UseVisualStyleBackColor = true;
            // 
            // chkGreek
            // 
            chkGreek.AutoSize = true;
            chkGreek.Checked = true;
            chkGreek.CheckState = CheckState.Checked;
            chkGreek.Location = new Point(8, 70);
            chkGreek.Margin = new Padding(4, 5, 4, 5);
            chkGreek.Name = "chkGreek";
            chkGreek.Size = new Size(83, 29);
            chkGreek.TabIndex = 9;
            chkGreek.Text = "Greek";
            chkGreek.UseVisualStyleBackColor = true;
            // 
            // chkItalian
            // 
            chkItalian.AutoSize = true;
            chkItalian.Checked = true;
            chkItalian.CheckState = CheckState.Checked;
            chkItalian.Location = new Point(8, 117);
            chkItalian.Margin = new Padding(4, 5, 4, 5);
            chkItalian.Name = "chkItalian";
            chkItalian.Size = new Size(85, 29);
            chkItalian.TabIndex = 10;
            chkItalian.Text = "Italian";
            chkItalian.UseVisualStyleBackColor = true;
            // 
            // chkMulti
            // 
            chkMulti.AutoSize = true;
            chkMulti.Checked = true;
            chkMulti.CheckState = CheckState.Checked;
            chkMulti.Location = new Point(7, 163);
            chkMulti.Margin = new Padding(4, 5, 4, 5);
            chkMulti.Name = "chkMulti";
            chkMulti.Size = new Size(78, 29);
            chkMulti.TabIndex = 11;
            chkMulti.Text = "Multi";
            chkMulti.UseVisualStyleBackColor = true;
            // 
            // chkPolish
            // 
            chkPolish.AutoSize = true;
            chkPolish.Checked = true;
            chkPolish.CheckState = CheckState.Checked;
            chkPolish.Location = new Point(7, 205);
            chkPolish.Margin = new Padding(4, 5, 4, 5);
            chkPolish.Name = "chkPolish";
            chkPolish.Size = new Size(84, 29);
            chkPolish.TabIndex = 12;
            chkPolish.Text = "Polish";
            chkPolish.UseVisualStyleBackColor = true;
            // 
            // chkSwedish
            // 
            chkSwedish.AutoSize = true;
            chkSwedish.Checked = true;
            chkSwedish.CheckState = CheckState.Checked;
            chkSwedish.Location = new Point(9, 292);
            chkSwedish.Margin = new Padding(4, 5, 4, 5);
            chkSwedish.Name = "chkSwedish";
            chkSwedish.Size = new Size(103, 29);
            chkSwedish.TabIndex = 13;
            chkSwedish.Text = "Swedish";
            chkSwedish.UseVisualStyleBackColor = true;
            // 
            // groupBoxSorting
            // 
            groupBoxSorting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxSorting.Controls.Add(lblSorting);
            groupBoxSorting.Controls.Add(cmbSortType);
            groupBoxSorting.Controls.Add(cmbLanguageSplit);
            groupBoxSorting.Location = new Point(636, 509);
            groupBoxSorting.Margin = new Padding(4, 5, 4, 5);
            groupBoxSorting.Name = "groupBoxSorting";
            groupBoxSorting.Padding = new Padding(4, 5, 4, 5);
            groupBoxSorting.Size = new Size(533, 63);
            groupBoxSorting.TabIndex = 4;
            groupBoxSorting.TabStop = false;
            // 
            // lblSorting
            // 
            lblSorting.AutoSize = true;
            lblSorting.Location = new Point(7, 25);
            lblSorting.Margin = new Padding(4, 0, 4, 0);
            lblSorting.Name = "lblSorting";
            lblSorting.Size = new Size(74, 25);
            lblSorting.TabIndex = 0;
            lblSorting.Text = "Sorting:";
            // 
            // cmbSortType
            // 
            cmbSortType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortType.Items.AddRange(new object[] { "No Sorting", "Alphabetical", "Category", "Category (0-Z)" });
            cmbSortType.Location = new Point(93, 22);
            cmbSortType.Margin = new Padding(4, 5, 4, 5);
            cmbSortType.Name = "cmbSortType";
            cmbSortType.Size = new Size(170, 33);
            cmbSortType.TabIndex = 1;
            // 
            // cmbLanguageSplit
            // 
            cmbLanguageSplit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguageSplit.Items.AddRange(new object[] { "Ignore Languages", "Split Languages" });
            cmbLanguageSplit.Location = new Point(279, 22);
            cmbLanguageSplit.Margin = new Padding(4, 5, 4, 5);
            cmbLanguageSplit.Name = "cmbLanguageSplit";
            cmbLanguageSplit.Size = new Size(246, 33);
            cmbLanguageSplit.TabIndex = 2;
            // 
            // groupBoxActions
            // 
            groupBoxActions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBoxActions.Controls.Add(btnScan);
            groupBoxActions.Controls.Add(btnDownload);
            groupBoxActions.Controls.Add(cmbDownloadType);
            groupBoxActions.Location = new Point(1177, 14);
            groupBoxActions.Margin = new Padding(4, 5, 4, 5);
            groupBoxActions.Name = "groupBoxActions";
            groupBoxActions.Padding = new Padding(4, 5, 4, 5);
            groupBoxActions.Size = new Size(129, 174);
            groupBoxActions.TabIndex = 5;
            groupBoxActions.TabStop = false;
            groupBoxActions.Text = "FTP Actions";
            // 
            // btnScan
            // 
            btnScan.Location = new Point(7, 33);
            btnScan.Margin = new Padding(4, 5, 4, 5);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(118, 36);
            btnScan.TabIndex = 0;
            btnScan.Text = "Load Data";
            btnScan.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(7, 80);
            btnDownload.Margin = new Padding(4, 5, 4, 5);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(118, 36);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            // 
            // cmbDownloadType
            // 
            cmbDownloadType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDownloadType.Items.AddRange(new object[] { "FTP", "HTTP" });
            cmbDownloadType.Location = new Point(7, 126);
            cmbDownloadType.Margin = new Padding(4, 5, 4, 5);
            cmbDownloadType.Name = "cmbDownloadType";
            cmbDownloadType.Size = new Size(118, 33);
            cmbDownloadType.TabIndex = 2;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(1182, 816);
            btnClear.Margin = new Padding(4, 5, 4, 5);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(118, 36);
            btnClear.TabIndex = 7;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click_1;
            // 
            // btnSetPath
            // 
            btnSetPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSetPath.Location = new Point(1182, 855);
            btnSetPath.Margin = new Padding(4, 5, 4, 5);
            btnSetPath.Name = "btnSetPath";
            btnSetPath.Size = new Size(118, 36);
            btnSetPath.TabIndex = 9;
            btnSetPath.Text = "Set Path";
            btnSetPath.UseVisualStyleBackColor = true;
            // 
            // btnOpenPath
            // 
            btnOpenPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenPath.Location = new Point(1182, 892);
            btnOpenPath.Margin = new Padding(4, 5, 4, 5);
            btnOpenPath.Name = "btnOpenPath";
            btnOpenPath.Size = new Size(118, 36);
            btnOpenPath.TabIndex = 10;
            btnOpenPath.Text = "Open Path";
            btnOpenPath.UseVisualStyleBackColor = true;
            // 
            // groupMisc
            // 
            groupMisc.Controls.Add(btnAbout);
            groupMisc.Controls.Add(btnHelp);
            groupMisc.Controls.Add(btnSavePrefs);
            groupMisc.Controls.Add(btnLoadPrefs);
            groupMisc.Location = new Point(1177, 598);
            groupMisc.Name = "groupMisc";
            groupMisc.Size = new Size(136, 210);
            groupMisc.TabIndex = 15;
            groupMisc.TabStop = false;
            groupMisc.Text = "Misc";
            // 
            // btnAbout
            // 
            btnAbout.Location = new Point(13, 166);
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new Size(114, 36);
            btnAbout.TabIndex = 18;
            btnAbout.Text = "About";
            btnAbout.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            btnHelp.Location = new Point(9, 121);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(114, 36);
            btnHelp.TabIndex = 17;
            btnHelp.Text = "Help";
            btnHelp.UseVisualStyleBackColor = true;
            // 
            // btnSavePrefs
            // 
            btnSavePrefs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSavePrefs.Location = new Point(8, 32);
            btnSavePrefs.Margin = new Padding(4, 5, 4, 5);
            btnSavePrefs.Name = "btnSavePrefs";
            btnSavePrefs.Size = new Size(114, 36);
            btnSavePrefs.TabIndex = 16;
            btnSavePrefs.Text = "Save Prefs";
            btnSavePrefs.UseVisualStyleBackColor = true;
            // 
            // btnLoadPrefs
            // 
            btnLoadPrefs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadPrefs.Location = new Point(8, 78);
            btnLoadPrefs.Margin = new Padding(4, 5, 4, 5);
            btnLoadPrefs.Name = "btnLoadPrefs";
            btnLoadPrefs.Size = new Size(114, 36);
            btnLoadPrefs.TabIndex = 15;
            btnLoadPrefs.Text = "Load Prefs";
            btnLoadPrefs.UseVisualStyleBackColor = true;
            // 
            // GroupData
            // 
            GroupData.Controls.Add(btnResetFilter);
            GroupData.Controls.Add(btnMakeFolder);
            GroupData.Controls.Add(btnClearFilter);
            GroupData.Location = new Point(1177, 436);
            GroupData.Name = "GroupData";
            GroupData.Size = new Size(135, 150);
            GroupData.TabIndex = 16;
            GroupData.TabStop = false;
            GroupData.Text = "Data";
            // 
            // btnMakeFolder
            // 
            btnMakeFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMakeFolder.Location = new Point(8, 106);
            btnMakeFolder.Margin = new Padding(4, 5, 4, 5);
            btnMakeFolder.Name = "btnMakeFolder";
            btnMakeFolder.Size = new Size(118, 36);
            btnMakeFolder.TabIndex = 13;
            btnMakeFolder.Text = "Make Folder";
            btnMakeFolder.TextAlign = ContentAlignment.MiddleLeft;
            btnMakeFolder.UseVisualStyleBackColor = true;
            // 
            // btnClearFilter
            // 
            btnClearFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearFilter.Location = new Point(8, 26);
            btnClearFilter.Margin = new Padding(4, 5, 4, 5);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(118, 36);
            btnClearFilter.TabIndex = 12;
            btnClearFilter.Text = "Clean Files";
            btnClearFilter.UseVisualStyleBackColor = true;
            // 
            // groupBoxList
            // 
            groupBoxList.Controls.Add(btnClearEdit);
            groupBoxList.Controls.Add(btnAppendList);
            groupBoxList.Controls.Add(btnSavelist);
            groupBoxList.Controls.Add(btnLoadlist);
            groupBoxList.Controls.Add(btnEditlist);
            groupBoxList.Location = new Point(1177, 196);
            groupBoxList.Name = "groupBoxList";
            groupBoxList.Size = new Size(129, 234);
            groupBoxList.TabIndex = 17;
            groupBoxList.TabStop = false;
            groupBoxList.Text = "Lists";
            // 
            // btnEditlist
            // 
            btnEditlist.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditlist.Location = new Point(7, 22);
            btnEditlist.Margin = new Padding(4, 5, 4, 5);
            btnEditlist.Name = "btnEditlist";
            btnEditlist.Size = new Size(118, 36);
            btnEditlist.TabIndex = 13;
            btnEditlist.Text = "Edit list";
            btnEditlist.UseVisualStyleBackColor = true;
            // 
            // btnLoadlist
            // 
            btnLoadlist.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadlist.Location = new Point(7, 66);
            btnLoadlist.Margin = new Padding(4, 5, 4, 5);
            btnLoadlist.Name = "btnLoadlist";
            btnLoadlist.Size = new Size(118, 36);
            btnLoadlist.TabIndex = 14;
            btnLoadlist.Text = "Load list";
            btnLoadlist.UseVisualStyleBackColor = true;
            // 
            // btnSavelist
            // 
            btnSavelist.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSavelist.Location = new Point(7, 110);
            btnSavelist.Margin = new Padding(4, 5, 4, 5);
            btnSavelist.Name = "btnSavelist";
            btnSavelist.Size = new Size(118, 36);
            btnSavelist.TabIndex = 15;
            btnSavelist.Text = "Save list";
            btnSavelist.UseVisualStyleBackColor = true;
            // 
            // btnAppendList
            // 
            btnAppendList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAppendList.Location = new Point(7, 153);
            btnAppendList.Margin = new Padding(4, 5, 4, 5);
            btnAppendList.Name = "btnAppendList";
            btnAppendList.Size = new Size(118, 36);
            btnAppendList.TabIndex = 16;
            btnAppendList.Text = "Append list";
            btnAppendList.UseVisualStyleBackColor = true;
            // 
            // btnResetFilter
            // 
            btnResetFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResetFilter.Location = new Point(8, 68);
            btnResetFilter.Margin = new Padding(4, 5, 4, 5);
            btnResetFilter.Name = "btnResetFilter";
            btnResetFilter.Size = new Size(118, 36);
            btnResetFilter.TabIndex = 14;
            btnResetFilter.Text = "Clear Data";
            btnResetFilter.UseVisualStyleBackColor = true;
            // 
            // btnClearEdit
            // 
            btnClearEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearEdit.Location = new Point(7, 192);
            btnClearEdit.Margin = new Padding(4, 5, 4, 5);
            btnClearEdit.Name = "btnClearEdit";
            btnClearEdit.Size = new Size(118, 36);
            btnClearEdit.TabIndex = 17;
            btnClearEdit.Text = "Clear Edit";
            btnClearEdit.UseVisualStyleBackColor = true;
            // 
            // FormWHDLoadTools
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1353, 1042);
            Controls.Add(groupBoxList);
            Controls.Add(GroupData);
            Controls.Add(groupMisc);
            Controls.Add(lstMain);
            Controls.Add(statusBar);
            Controls.Add(groupBoxServer);
            Controls.Add(groupBoxFolders);
            Controls.Add(groupBoxSorting);
            Controls.Add(groupBoxFilter);
            Controls.Add(groupBoxActions);
            Controls.Add(btnClear);
            Controls.Add(btnSetPath);
            Controls.Add(btnOpenPath);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormWHDLoadTools";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WHDLoad Download Tool";
            FormClosed += FormWHDLoadTools_FormClosed;
            statusBar.ResumeLayout(false);
            statusBar.PerformLayout();
            groupBoxServer.ResumeLayout(false);
            panelServerScroll.ResumeLayout(false);
            panelServerScroll.PerformLayout();
            groupBoxFolders.ResumeLayout(false);
            panelFoldersScroll.ResumeLayout(false);
            panelFoldersScroll.PerformLayout();
            groupBoxFilter.ResumeLayout(false);
            panelFilterScroll.ResumeLayout(false);
            groupBoxFilterMisc.ResumeLayout(false);
            groupBoxFilterMisc.PerformLayout();
            groupBoxFilterMemory.ResumeLayout(false);
            groupBoxFilterMemory.PerformLayout();
            groupBoxFilterSound.ResumeLayout(false);
            groupBoxFilterSound.PerformLayout();
            groupBoxFilterChipset.ResumeLayout(false);
            groupBoxFilterChipset.PerformLayout();
            groupBoxFilterSystem.ResumeLayout(false);
            groupBoxFilterSystem.PerformLayout();
            groupBoxFilterLanguage.ResumeLayout(false);
            groupBoxFilterLanguage.PerformLayout();
            groupBoxSorting.ResumeLayout(false);
            groupBoxSorting.PerformLayout();
            groupBoxActions.ResumeLayout(false);
            groupMisc.ResumeLayout(false);
            GroupData.ResumeLayout(false);
            groupBoxList.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // Declaración de controles
        private System.Windows.Forms.ListBox lstMain;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel lblSystem;
        private System.Windows.Forms.ToolStripStatusLabel lblChipset;
        private System.Windows.Forms.ToolStripStatusLabel lblTVSystem;
        private System.Windows.Forms.ToolStripStatusLabel lblLanguage;
        private System.Windows.Forms.ToolStripStatusLabel lblType;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusInfo;
        private System.Windows.Forms.ToolStripStatusLabel lblSize;
        private System.Windows.Forms.ToolStripStatusLabel lblVersion;
        
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.Panel panelServerScroll;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtFtpUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtFtpPass;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtFtpServer;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtFtpPort;
        private System.Windows.Forms.Label lblFtpFolder;
        private System.Windows.Forms.TextBox txtFtpFolder;
        private System.Windows.Forms.Label lblHttpPath;
        private System.Windows.Forms.TextBox txtHttpServer;
        private System.Windows.Forms.Label lblGamePath;
        private System.Windows.Forms.TextBox txtGamePath;
        private System.Windows.Forms.Label lblDemoPath;
        private System.Windows.Forms.TextBox txtDemoPath;
        private System.Windows.Forms.Label lblBetaGamePath;
        private System.Windows.Forms.TextBox txtBetaGamePath;
        private System.Windows.Forms.Label lblBetaDemoPath;
        private System.Windows.Forms.TextBox txtBetaDemoPath;
        private System.Windows.Forms.Label lblMagsPath;
        private System.Windows.Forms.TextBox txtMagsPath;
        
        private System.Windows.Forms.GroupBox groupBoxFolders;
        private System.Windows.Forms.Panel panelFoldersScroll;
        private System.Windows.Forms.Label lblParent;
        private System.Windows.Forms.TextBox txtWHDMain;
        private System.Windows.Forms.Button btnOpenMain;
        private System.Windows.Forms.Button btnSetMain;
        private System.Windows.Forms.CheckBox chkGames;
        private System.Windows.Forms.TextBox txtWHDGames;
        private System.Windows.Forms.Button btnOpenGames;
        private System.Windows.Forms.CheckBox chkDemos;
        private System.Windows.Forms.TextBox txtWHDDemos;
        private System.Windows.Forms.Button btnOpenDemos;
        private System.Windows.Forms.CheckBox chkBetaGames;
        private System.Windows.Forms.TextBox txtWHDBetaGames;
        private System.Windows.Forms.Button btnOpenBetaGames;
        private System.Windows.Forms.CheckBox chkBetaDemos;
        private System.Windows.Forms.TextBox txtWHDBetaDemos;
        private System.Windows.Forms.Button btnOpenBetaDemos;
        private System.Windows.Forms.CheckBox chkMagazines;
        private System.Windows.Forms.TextBox txtWHDMags;
        private System.Windows.Forms.Button btnOpenMags;
        
        private System.Windows.Forms.GroupBox groupBoxSorting;
        private System.Windows.Forms.Label lblSorting;
        private System.Windows.Forms.ComboBox cmbSortType;
        private System.Windows.Forms.ComboBox cmbLanguageSplit;
        
        private System.Windows.Forms.GroupBox groupBoxActions;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ComboBox cmbDownloadType;
        
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.Button btnOpenPath;
        private GroupBox groupMisc;
        private Button btnAbout;
        private Button btnHelp;
        private Button btnSavePrefs;
        private Button btnLoadPrefs;
        private GroupBox GroupData;
        private Button btnMakeFolder;
        private Button btnClearFilter;
        private Panel panelFilterScroll;
        private GroupBox groupBoxFilterSound;
        private CheckBox chkMT32;
        private CheckBox chkNoVoice;
        private CheckBox chkNoSpeech;
        private CheckBox chkNoMusic;
        private GroupBox groupBoxFilterChipset;
        private CheckBox chkAGA;
        private CheckBox chkECS;
        private CheckBox chkNTSC;
        private CheckBox chkPAL;
        private GroupBox groupBoxFilterSystem;
        private CheckBox chkAmiga;
        private CheckBox chkArcadia;
        private CheckBox chkCD32;
        private CheckBox chkCDTV;
        private CheckBox chkCDROM;
        private GroupBox groupBoxFilterMemory;
        private CheckBox chkChip;
        private CheckBox chkFast;
        private CheckBox chk15MB;
        private CheckBox chk2MB;
        private CheckBox chk8MB;
        private CheckBox chk12MB;
        private CheckBox chk1MBChp;
        private CheckBox chk1MB;
        private CheckBox chk512KB;
        private CheckBox chk512k;
        private CheckBox chkLowMen;
        private CheckBox chkSlowMm;
        private Panel LeneaSeparacion;
        private Button btnResetLang;
        private Button btnCleaLan;
        private Button btnLang;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private GroupBox groupBoxFilterMisc;
        private CheckBox chkCensored;
        private CheckBox chkUnCensored;
        private CheckBox chkGameDemo;
        private CheckBox chkEnhanced;
        private CheckBox chkPreviewMisc;
        private CheckBox chkPreRelease;
        private CheckBox chkNoIntro;
        private CheckBox chkNoMovie;
        private CheckBox chkLoRes;
        private CheckBox chkHiRes;
        private CheckBox chk4Disk;
        private CheckBox chk3Disk;
        private CheckBox chk2Disk;
        private CheckBox chk1Disk;
        private CheckBox chkImage;
        private CheckBox chkFiles;
        private GroupBox groupBoxList;
        private Button btnEditlist;
        private Button btnSavelist;
        private Button btnLoadlist;
        private Button btnAppendList;
        private Button btnResetFilter;
        private Button btnClearEdit;

        // Los demás controles se agregarán en las siguientes partes...
    }
}
