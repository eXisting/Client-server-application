using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synchronizer_MVP_pattern
{
    public partial class ClientServerView : Form, IView
    {

        public ClientServerView()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
        }

        #region IView implementation

        #region Server
        public event EventHandler<EventArgs> StartServerHandler;
        public event EventHandler<EventArgs> ShutDownServerHandler;
        public event EventHandler<EventArgs> ChooseServerFolderHandler;
        public event EventHandler<EventArgs> FilterServerLogs;
        public event EventHandler<EventArgs> ClearServerLogs;

        public event EventHandler<FileSystemEventArgs> ServerSystemWatcherChanged;
        public event EventHandler<FileSystemEventArgs> ServerSystemWatcherCreated;
        public event EventHandler<RenamedEventArgs> ServerSystemWatcherRenamed;
        public event EventHandler<FileSystemEventArgs> ServerSystemWatcherDeleted;

        #endregion

        #region Client

        public event EventHandler<EventArgs> StartClientHandler;
        public event EventHandler<EventArgs> ShutDownClientHandler;
        public event EventHandler<EventArgs> ChooseClientFolderHandler;
        public event EventHandler<EventArgs> FilterClientLogs;
        public event EventHandler<EventArgs> ClearClientLogs;

        #endregion

        ListView IView.GetServerLogs()
        {
            return serverLogs;
        }

        public string GetServerIP()
        {
            return serverIP.Text.ToString();
        }

        public void SetDefaultServerIP(string _ip)
        {
            serverIP.Text = _ip;
        }

        Control.ControlCollection IView.TakeControls()
        {
            return Controls;
        }

        public string GetServerPort()
        {
            return serverPort.Text.ToString();
        }

        public void SetDefaultServerPort(string _port)
        {
            serverPort.Text = _port;
        }
        
        public FileSystemWatcher getServerWatcher()
        {
            return serverSystemWatcher;
        }

        public string GetClientTextBoxIP()
        {
            return clientIPBox.Text.ToString();
        }

        public void SetDefaultClientTextBoxIP(string _ip)
        {
            clientIPBox.Text = _ip;
        }

        public string GetClientTextBoxPort()
        {
            return clientPortBox.Text.ToString();
        }

        public ListView GetClientLogs()
        {
            return clientLogs;
        }

        public FolderBrowserDialog GetClientFolderDialog()
        {
            return clientFolderDialog;
        }

        #endregion

        private void StartServer_Click(object sender, EventArgs e)
        {
            StartServerHandler(this, e);
        }

        private void shutDownServer_Click(object sender, EventArgs e)
        {
            ShutDownServerHandler(this, e);
        }

        private void chooseServerFolder_Click(object sender, EventArgs e)
        {
            ChooseServerFolderHandler(this, e);
        }

        private void serverLogsFilter_Click(object sender, EventArgs e)
        {
            FilterServerLogs(this, e);
        }

        private void clearServerLogs_Click(object sender, EventArgs e)
        {
            ClearServerLogs(this, e);
        }

        private void serverSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ServerSystemWatcherChanged(this, e);
        }

        private void serverSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            ServerSystemWatcherCreated(this, e);
        }

        private void serverSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            ServerSystemWatcherDeleted(this, e);
        }

        private void serverSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            ServerSystemWatcherRenamed(this, e);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            StartClientHandler(this, e);
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            ShutDownClientHandler(this, e);
        }

        private void chooseClientFolder_Click(object sender, EventArgs e)
        {
            ChooseClientFolderHandler(this, e);
        }

        private void clientFilter_Click(object sender, EventArgs e)
        {
            FilterClientLogs(this, e);
        }

        private void clientClear_Click(object sender, EventArgs e)
        {
            ClearClientLogs(this, e);
        }
    }
}
