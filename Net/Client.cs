using Model;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Net
{
    public class Client
    {
        public UserModel UserModel { get; set; } = new UserModel();

        private Data _data;
        private TcpClient _tcpClient;
        private PacketReader _packetReader;

        public Client()
        {
            _tcpClient = new TcpClient();
        }

        public Client(TcpClient tcpClient, Listener listener, Data data)
        {
            _tcpClient = tcpClient;
            UserModel.Guid = Guid.NewGuid();

            _packetReader = new PacketReader(_tcpClient.GetStream());

            Task.Run(() => ProcessPacket(listener, data));
        }

        public void ProcessPacket(Listener listener, Data data)
        {
            while (true)
            {
                var opcode = (OpCode)Enum.Parse(typeof(OpCode), _packetReader.ReadByte().ToString());

                if (opcode == OpCode.ConnectToServer)
                {
                    UserModel = new UserModel()
                    {
                        Guid = Guid.NewGuid(),
                        Username = _packetReader.ToString()
                    };
                    data.UserModelList.Add(UserModel);

                    Console.WriteLine($"{UserModel.Username} has connected to server");
                }
                else if (opcode == OpCode.SendMessage)
                {
                    var messageModel = new MessageModel()
                    {
                        PropUserGuid = UserModel.Guid,
                        PropCreationDateTime = DateTime.Now,
                        PropMessage = _packetReader.ToString()
                    };
                    data.MessageModelList.Add(messageModel);

                    var user = data.UserModelList.FirstOrDefault(o => o.Guid == messageModel.PropUserGuid);

                    if (user == null)
                    {
                        Console.WriteLine($"User with Guid: {messageModel.PropUserGuid} not found");
                    }
                    else
                    {
                        Console.WriteLine($"{messageModel.PropMessage} {user.Username}: {messageModel.PropMessage}");
                    }
                }
                else if (opcode == OpCode.SynchronizeData)
                {
                    listener.BroadcastData();
                }
                else Console.WriteLine("opcode not exist");

                File.WriteAllText("Data.txt", JsonSerializer.Serialize(data));

            }
        }

        public void Send(OpCode opcode, string str)
        {
            if (opcode == OpCode.ConnectToServer && !_tcpClient.Connected)
            {
                _tcpClient.Connect(IPAddress.Parse(NetSettings.ServerIp), NetSettings.ServerPort);
                _packetReader = new PacketReader(_tcpClient.GetStream());
            }

            var packet = new PacketBuilder()
                .WriteOpCode((byte)opcode)
                .WriteString(str)
                .ToBytesArray();

            _tcpClient.Client.Send(packet);
        }
    }
}
