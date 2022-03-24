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

            var opCode = _packetReader.ReadByte();
            Username = _packetReader.ToString();

            Console.WriteLine($"{Username} connected");
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

        public void SendMessage()
        {

        }
    }
}
