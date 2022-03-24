using System.Net;
using System.Net.Sockets;

namespace Net
{
    public class Client
    {
        public string Username { get; set; }
        public Guid Guid { get; set; }

        private TcpClient _tcpClient;
        private PacketReader _packetReader;

        public Client()
        {
            _tcpClient = new TcpClient();
        }

        public Client(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            Guid = Guid.NewGuid();

            _packetReader = new PacketReader(_tcpClient.GetStream());

            Task.Run(() => PacketProcessor.Process(_packetReader, this));
        }

        public void ConnectToServer(string username)
        {
            if (!_tcpClient.Connected)
            {
                _tcpClient.Connect(IPAddress.Parse(NetSettings.ServerIp), NetSettings.ServerPort);

                var connectPacket = new PacketBuilder()
                    .WriteOpCode(0)
                    .WriteString(username)
                    .ToBytesArray();

                _tcpClient.Client.Send(connectPacket);
            }
        }

        public void SendMessage(string str)
        {
            var messagePacket = new PacketBuilder()
                .WriteOpCode(5)
                .WriteString(str)
                .ToBytesArray();

            _tcpClient.Client.Send(messagePacket);
        }
    }
}
