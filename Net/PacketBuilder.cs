using System.Text;

namespace Net
{
    public class PacketBuilder
    {
        private MemoryStream _memoryStream;


        public PacketBuilder()
        {
            _memoryStream = new MemoryStream();
        }


        public PacketBuilder WriteOpCode(byte opcode)
        {
            _memoryStream.WriteByte(opcode);
            return this;
        }

        public PacketBuilder WriteString(string str)
        {
            _memoryStream.Write(BitConverter.GetBytes(str.Length));
            _memoryStream.Write(Encoding.ASCII.GetBytes(str));
            return this;
        }

        public byte[] ToBytesArray()
        {
            return _memoryStream.ToArray();
        }
    }
}
