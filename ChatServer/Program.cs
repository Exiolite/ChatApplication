using Model;
using Net;

namespace ChatServer
{
    public class Program
    {
        private static Listener _listener;
        private static Data _data;

        static void Main(string[] args)
        {
            _listener = new Listener(_data);
        }
    }
}