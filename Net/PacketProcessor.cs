namespace Net
{
    public static class PacketProcessor
    {
        public static void Process(PacketReader packetReader, Client client)
        {
            while (true)
            {
                var opcode = packetReader.ReadByte();

                if (opcode == 0)
                {
                    client.Username = packetReader.ToString();
                    Console.WriteLine($"{client.Username} has connected to server");
                }
                else if (opcode == 5) Console.WriteLine($"{DateTime.Now} {client.Username}: {packetReader.ToString()}");
                else Console.WriteLine("opcode not exist");
            }
        }
    }
}
