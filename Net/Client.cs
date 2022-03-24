using Model;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Net
{
    public class Client
    {
        public UserModel UserModel { get; set; } = new UserModel();

        private TcpClient _tcpClient;
        private PacketReader _packetReader;


        public Client()
        {
            _tcpClient = new TcpClient();
        }

        public Client(TcpClient tcpClient, Data data)
        {
            _tcpClient = tcpClient;

            _packetReader = new PacketReader(_tcpClient.GetStream());

            Task.Run(() => ProcessPacket(data));
        }

        public void ProcessPacket(Data data)
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
                        Console.WriteLine($"{messageModel.PropCreationDateTime} {user.Username}: {messageModel.PropMessage}");
                    }
                }
                else Console.WriteLine("opcode not exist");

                var jsonData = JsonSerializer.Serialize(data);
                File.WriteAllText("Data.txt", jsonData);
                Send(OpCode.SynchronizeData, jsonData);
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
