using Model;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Net
{
    public class Listener
    {
        private TcpListener _listener;
        private List<Client> _clients;
        private Data _data;

        public Listener(Data data)
        {
            _data = data;

            _listener = new TcpListener(IPAddress.Parse(NetSettings.ServerIp), NetSettings.ServerPort);
            _listener.Start();


            _clients = new List<Client>();
            while (true)
            {
                var tcpClient = _listener.AcceptTcpClient();
                _clients.Add(new Client(tcpClient, this, data));
            }
        }

        public void BroadcastData()
        {
            var jsonData = JsonSerializer.Serialize(_data);

            foreach (var client in _clients)
            {
                client.Send(OpCode.SynchronizeData, jsonData);
            }
        }
    }
}
