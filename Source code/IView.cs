using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synchronizer_MVP_pattern
{

    /*-----------------------------------------------------------------*/

    interface IView
    {

        /*-----------------------------------------------------------------*/

        Control.ControlCollection TakeControls();

        /*-----------------------------------------------------------------*/

        #region Server

        /*-----------------------------------------------------------------*/

        string GetServerIP();

        void SetDefaultServerIP(string _ip);

        string GetServerPort();

        void SetDefaultServerPort(string _port);

        ListView GetServerLogs();

        FileSystemWatcher getServerWatcher();

        /*-----------------------------------------------------------------*/

        event EventHandler<EventArgs> StartServerHandler;

        event EventHandler<EventArgs> ShutDownServerHandler;

        event EventHandler<EventArgs> ChooseServerFolderHandler;

        event EventHandler<EventArgs> FilterServerLogs;

        event EventHandler<EventArgs> ClearServerLogs;

        event EventHandler<FileSystemEventArgs> ServerSystemWatcherChanged;

        event EventHandler<FileSystemEventArgs> ServerSystemWatcherCreated;

        event EventHandler<RenamedEventArgs> ServerSystemWatcherRenamed;

        event EventHandler<FileSystemEventArgs> ServerSystemWatcherDeleted;

        #endregion

        /*-----------------------------------------------------------------*/

        #region Client

        /*-----------------------------------------------------------------*/

        string GetClientTextBoxIP();
        
        void SetDefaultClientTextBoxIP(string _ip);

        string GetClientTextBoxPort();

        ListView GetClientLogs();

        FolderBrowserDialog GetClientFolderDialog();

        /*-----------------------------------------------------------------*/

        event EventHandler<EventArgs> StartClientHandler;

        event EventHandler<EventArgs> ShutDownClientHandler;

        event EventHandler<EventArgs> ChooseClientFolderHandler;

        event EventHandler<EventArgs> FilterClientLogs;

        event EventHandler<EventArgs> ClearClientLogs;

        /*-----------------------------------------------------------------*/

        #endregion

        /*-----------------------------------------------------------------*/

    }

    /*-----------------------------------------------------------------*/

}
