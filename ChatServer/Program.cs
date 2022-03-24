using Net;

namespace ChatServer
{
    internal class Program
    {
        private static Listener _listener;
        static void Main(string[] args)
        {
            _listener = new Listener();
        }
    }
}