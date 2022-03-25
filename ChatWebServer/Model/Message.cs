using System;

namespace ChatWebServer.Model
{
    public class Message
    {
        public string PropUsername { get; set; }
        public string PropMessage { get; set; } = string.Empty;
        public DateTime PropCreationDateTime { get; set; }
    }
}