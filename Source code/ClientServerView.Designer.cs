namespace Synchronizer_MVP_pattern
{
    partial class ClientServerView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.serverSide = new System.Windows.Forms.GroupBox();
            this.serverLogs = new System.Windows.Forms.ListView();
            this.clearServerLogs = new System.Windows.Forms.Button();
            this.serverLogsFilter = new System.Windows.Forms.Button();
            this.chooseServerFolder = new System.Windows.Forms.Button();
            this.shutDownServer = new System.Windows.Forms.Button();
            this.startServer = new System.Windows.Forms.Button();
            this.serverPort = new System.Windows.Forms.TextBox();
            this.serverIP = new System.Windows.Forms.TextBox();
            this.serverSystemWatcher = new System.IO.FileSystemWatcher();
            this.clientSide = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.clientClear = new System.Windows.Forms.Button();
            this.clientFilter = new System.Windows.Forms.Button();
            this.chooseClientFolder = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.clientPortBox = new System.Windows.Forms.TextBox();
            this.clientIPBox = new System.Windows.Forms.TextBox();
            this.clientLogs = new System.Windows.Forms.ListView();
            this.clientFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.serverSide.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverSystemWatcher)).BeginInit();
            this.clientSide.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverSide
            // 
            this.serverSide.Controls.Add(this.serverLogs);
            this.serverSide.Controls.Add(this.clearServerLogs);
            this.serverSide.Controls.Add(this.serverLogsFilter);
            this.serverSide.Controls.Add(this.chooseServerFolder);
            this.serverSide.Controls.Add(this.shutDownServer);
            this.serverSide.Controls.Add(this.startServer);
            this.serverSide.Controls.Add(this.serverPort);
            this.serverSide.Controls.Add(this.serverIP);
            this.serverSide.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.serverSide.Location = new System.Drawing.Point(57, 12);
            this.serverSide.Name = "serverSide";
            this.serverSide.Size = new System.Drawing.Size(1285, 412);
            this.serverSide.TabIndex = 1;
            this.serverSide.TabStop = false;
            this.serverSide.Text = "Server side";
            // 
            // serverLogs
            // 
            this.serverLogs.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.serverLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.serverLogs.GridLines = true;
            this.serverLogs.Location = new System.Drawing.Point(254, 27);
            this.serverLogs.Name = "serverLogs";
            this.serverLogs.Size = new System.Drawing.Size(1004, 366);
            this.serverLogs.TabIndex = 10;
            this.serverLogs.TileSize = new System.Drawing.Size(288, 200);
            this.serverLogs.UseCompatibleStateImageBehavior = false;
            this.serverLogs.View = System.Windows.Forms.View.List;
            // 
            // clearServerLogs
            // 
            this.clearServerLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearServerLogs.Location = new System.Drawing.Point(17, 352);
            this.clearServerLogs.Name = "clearServerLogs";
            this.clearServerLogs.Size = new System.Drawing.Size(204, 41);
            this.clearServerLogs.TabIndex = 18;
            this.clearServerLogs.Text = "Clear logs";
            this.clearServerLogs.UseVisualStyleBackColor = true;
            this.clearServerLogs.Click += new System.EventHandler(this.clearServerLogs_Click);
            // 
            // serverLogsFilter
            // 
            this.serverLogsFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.serverLogsFilter.Location = new System.Drawing.Point(17, 303);
            this.serverLogsFilter.Name = "serverLogsFilter";
            this.serverLogsFilter.Size = new System.Drawing.Size(204, 41);
            this.serverLogsFilter.TabIndex = 17;
            this.serverLogsFilter.Text = "Filter logs";
            this.serverLogsFilter.UseVisualStyleBackColor = true;
            this.serverLogsFilter.Click += new System.EventHandler(this.serverLogsFilter_Click);
            // 
            // chooseServerFolder
            // 
            this.chooseServerFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chooseServerFolder.Location = new System.Drawing.Point(16, 244);
            this.chooseServerFolder.Name = "chooseServerFolder";
            this.chooseServerFolder.Size = new System.Drawing.Size(204, 41);
            this.chooseServerFolder.TabIndex = 16;
            this.chooseServerFolder.Text = "Choose folder";
            this.chooseServerFolder.UseVisualStyleBackColor = true;
            this.chooseServerFolder.Click += new System.EventHandler(this.chooseServerFolder_Click);
            // 
            // shutDownServer
            // 
            this.shutDownServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.shutDownServer.Location = new System.Drawing.Point(17, 185);
            this.shutDownServer.Name = "shutDownServer";
            this.shutDownServer.Size = new System.Drawing.Size(204, 41);
            this.shutDownServer.TabIndex = 15;
            this.shutDownServer.Text = "Shut down";
            this.shutDownServer.UseVisualStyleBackColor = true;
            this.shutDownServer.Click += new System.EventHandler(this.shutDownServer_Click);
            // 
            // startServer
            // 
            this.startServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startServer.Location = new System.Drawing.Point(16, 128);
            this.startServer.Name = "startServer";
            this.startServer.Size = new System.Drawing.Size(204, 41);
            this.startServer.TabIndex = 14;
            this.startServer.Text = "Start";
            this.startServer.UseVisualStyleBackColor = true;
            this.startServer.Click += new System.EventHandler(this.StartServer_Click);
            // 
            // serverPort
            // 
            this.serverPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.serverPort.Location = new System.Drawing.Point(78, 67);
            this.serverPort.Name = "serverPort";
            this.serverPort.Size = new System.Drawing.Size(90, 30);
            this.serverPort.TabIndex = 13;
            this.serverPort.Text = "10000";
            this.serverPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // serverIP
            // 
            this.serverIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.serverIP.Location = new System.Drawing.Point(16, 27);
            this.serverIP.Name = "serverIP";
            this.serverIP.Size = new System.Drawing.Size(205, 34);
            this.serverIP.TabIndex = 12;
            this.serverIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // serverSystemWatcher
            // 
            this.serverSystemWatcher.EnableRaisingEvents = true;
            this.serverSystemWatcher.SynchronizingObject = this;
            this.serverSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.serverSystemWatcher_Changed);
            this.serverSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.serverSystemWatcher_Created);
            this.serverSystemWatcher.Deleted += new System.IO.FileSystemEventHandler(this.serverSystemWatcher_Deleted);
            this.serverSystemWatcher.Renamed += new System.IO.RenamedEventHandler(this.serverSystemWatcher_Renamed);
            // 
            // clientSide
            // 
            this.clientSide.Controls.Add(this.connectButton);
            this.clientSide.Controls.Add(this.clientClear);
            this.clientSide.Controls.Add(this.clientFilter);
            this.clientSide.Controls.Add(this.chooseClientFolder);
            this.clientSide.Controls.Add(this.disconnectButton);
            this.clientSide.Controls.Add(this.clientPortBox);
            this.clientSide.Controls.Add(this.clientIPBox);
            this.clientSide.Controls.Add(this.clientLogs);
            this.clientSide.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientSide.Location = new System.Drawing.Point(57, 430);
            this.clientSide.Name = "clientSide";
            this.clientSide.Size = new System.Drawing.Size(1285, 426);
            this.clientSide.TabIndex = 2;
            this.clientSide.TabStop = false;
            this.clientSide.Text = "Client side";
            // 
            // connectButton
            // 
            this.connectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.connectButton.Location = new System.Drawing.Point(18, 134);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(204, 41);
            this.connectButton.TabIndex = 26;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // clientClear
            // 
            this.clientClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientClear.Location = new System.Drawing.Point(18, 359);
            this.clientClear.Name = "clientClear";
            this.clientClear.Size = new System.Drawing.Size(204, 41);
            this.clientClear.TabIndex = 25;
            this.clientClear.Text = "Clear logs";
            this.clientClear.UseVisualStyleBackColor = true;
            this.clientClear.Click += new System.EventHandler(this.clientClear_Click);
            // 
            // clientFilter
            // 
            this.clientFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientFilter.Location = new System.Drawing.Point(18, 310);
            this.clientFilter.Name = "clientFilter";
            this.clientFilter.Size = new System.Drawing.Size(204, 41);
            this.clientFilter.TabIndex = 24;
            this.clientFilter.Text = "Filter logs";
            this.clientFilter.UseVisualStyleBackColor = true;
            this.clientFilter.Click += new System.EventHandler(this.clientFilter_Click);
            // 
            // chooseClientFolder
            // 
            this.chooseClientFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chooseClientFolder.Location = new System.Drawing.Point(17, 251);
            this.chooseClientFolder.Name = "chooseClientFolder";
            this.chooseClientFolder.Size = new System.Drawing.Size(204, 41);
            this.chooseClientFolder.TabIndex = 23;
            this.chooseClientFolder.Text = "Choose folder";
            this.chooseClientFolder.UseVisualStyleBackColor = true;
            this.chooseClientFolder.Click += new System.EventHandler(this.chooseClientFolder_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.disconnectButton.Location = new System.Drawing.Point(18, 192);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(204, 41);
            this.disconnectButton.TabIndex = 22;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // clientPortBox
            // 
            this.clientPortBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientPortBox.Location = new System.Drawing.Point(79, 74);
            this.clientPortBox.Name = "clientPortBox";
            this.clientPortBox.Size = new System.Drawing.Size(90, 30);
            this.clientPortBox.TabIndex = 20;
            this.clientPortBox.Text = "10000";
            this.clientPortBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clientIPBox
            // 
            this.clientIPBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientIPBox.Location = new System.Drawing.Point(17, 34);
            this.clientIPBox.Name = "clientIPBox";
            this.clientIPBox.Size = new System.Drawing.Size(205, 34);
            this.clientIPBox.TabIndex = 19;
            this.clientIPBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clientLogs
            // 
            this.clientLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clientLogs.Location = new System.Drawing.Point(254, 27);
            this.clientLogs.Name = "clientLogs";
            this.clientLogs.Size = new System.Drawing.Size(1004, 373);
            this.clientLogs.TabIndex = 10;
            this.clientLogs.UseCompatibleStateImageBehavior = false;
            this.clientLogs.View = System.Windows.Forms.View.List;
            // 
            // ClientServerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1382, 902);
            this.Controls.Add(this.clientSide);
            this.Controls.Add(this.serverSide);
            this.Name = "ClientServerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientServerView";
            this.serverSide.ResumeLayout(false);
            this.serverSide.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverSystemWatcher)).EndInit();
            this.clientSide.ResumeLayout(false);
            this.clientSide.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox serverSide;
        private System.Windows.Forms.Button chooseServerFolder;
        private System.Windows.Forms.Button shutDownServer;
        private System.Windows.Forms.Button startServer;
        private System.Windows.Forms.TextBox serverPort;
        private System.Windows.Forms.TextBox serverIP;
        private System.Windows.Forms.Button serverLogsFilter;
        private System.Windows.Forms.Button clearServerLogs;
        private System.IO.FileSystemWatcher serverSystemWatcher;
        private System.Windows.Forms.ListView serverLogs;
        private System.Windows.Forms.GroupBox clientSide;
        private System.Windows.Forms.ListView clientLogs;
        private System.Windows.Forms.Button clientClear;
        private System.Windows.Forms.Button clientFilter;
        private System.Windows.Forms.Button chooseClientFolder;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.TextBox clientPortBox;
        private System.Windows.Forms.TextBox clientIPBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.FolderBrowserDialog clientFolderDialog;
    }
}