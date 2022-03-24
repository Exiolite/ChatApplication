using System.Net.Sockets;
using System.Text;

namespace Net
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _networkStream;


        public PacketReader(NetworkStream memoryStream) : base(memoryStream)
        {
            _networkStream = memoryStream;
        }


        public override string ToString()
        {
            var lenght = ReadInt32();
            var memoryStream = new byte[lenght];

            _networkStream.Read(memoryStream);

            return Encoding.ASCII.GetString(memoryStream);
        }
    }
}
