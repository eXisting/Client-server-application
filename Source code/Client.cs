using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Synchronizer_MVP_pattern
{
    internal sealed class Client
    {

        /*-----------------------------------------------------------------*/

        #region Client

        /*-----------------------------------------------------------------*/

        public Thread downloadThread;

        public NetworkStream strRemote;

        public TcpClient tcpClient;

        public string path;

        public bool isFatalErrorOnServerSide = false;

        /*-----------------------------------------------------------------*/

        public StringBuilder[] logsStorage = new StringBuilder[Convert.ToInt32(MainModel.LogType.Count)];

        public ListView clientViewLogs = null;

        /*-----------------------------------------------------------------*/

        public FolderBrowserDialog clientFolderDialog = null;

        /*-----------------------------------------------------------------*/

        #endregion

        /*-----------------------------------------------------------------*/

    }
}
