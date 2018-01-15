using System.Net.Sockets;

namespace Synchronizer_MVP_pattern
{
    class User
    {
        /*-----------------------------------------------------------------*/

        public TcpClient TcpClient { get; private set; }

        public string IP { get; private set; }

        /*-----------------------------------------------------------------*/


        public User(TcpClient _tcpClient, string _IP)
        {
            TcpClient = _tcpClient;
            IP = _IP;
        }

        /*-----------------------------------------------------------------*/

        public void CreateTCPClient()
        {
            if (TcpClient != null)
                TcpClient.Close();

            TcpClient = new TcpClient();
        }

        /*-----------------------------------------------------------------*/

    }
}
