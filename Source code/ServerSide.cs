using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Synchronizer_MVP_pattern
{
    internal sealed class ServerSide
    {

        /*-----------------------------------------------------------------*/

        #region Server        

        /*-----------------------------------------------------------------*/

        public Thread listenerThread;

        public Thread connectionCheckerThread;

        public FileStream fileStream;

        public NetworkStream networkStream;

        public TcpListener tcpListener;

        public User user;

        public IPAddress Ip;

        /*-----------------------------------------------------------------*/

        public System.Windows.Forms.Label Label = new Label();

        public PictureBox Picture = new PictureBox();

        public DateTime TimeChange = new DateTime();

        /*-----------------------------------------------------------------*/

        public delegate void ControlsDelegate(Control _control);

        public ControlsDelegate InvokeMethod;

        /*-----------------------------------------------------------------*/
        
        public string Host = System.Net.Dns.GetHostName();

        public string Command;

        public bool StartServer;

        /*-----------------------------------------------------------------*/

        public StringBuilder[] logsStorage = new StringBuilder[Convert.ToInt32(MainModel.LogType.Count)];

        public ListView serverViewLogs = null;

        /*-----------------------------------------------------------------*/

        public FileSystemWatcher serverWatcher = null;

        /*-----------------------------------------------------------------*/

        #endregion
    }

    /*-----------------------------------------------------------------*/

}
