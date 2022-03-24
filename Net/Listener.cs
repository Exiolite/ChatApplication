using Model;
using System.Net;
using System.Net.Sockets;

namespace Net
{
    public class Listener
    {
        private TcpListener _listener;
        private List<Client> _clients;

        public Listener(Data data)
        {
            _listener = new TcpListener(IPAddress.Parse(NetSettings.ServerIp), NetSettings.ServerPort);
            _listener.Start();


            _clients = new List<Client>();
            while (true)
            {
                var tcpClient = _listener.AcceptTcpClient();
                _clients.Add(new Client(tcpClient, data));
            }
        }
    }
}
