﻿using ChatWebServer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Web.Http;

namespace ChatWebServer.Controllers
{
    public class MessageController : ApiController
    {
        private const string MessageListFileName = "MessageList.json";
        public List<Message> MessageList { get; set; } = new List<Message>();

        // GET: api/Message
        public List<Message> Get()
        {
            Load();
            return MessageList;
        }

        // GET: api/Message/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Message
        public void Post([FromBody]string value)
        {
            var message = JsonSerializer.Deserialize<Message>(value);

            Load();
            MessageList.Add(message);
            Save();
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
        }

        private void Save()
        {
            var jsonMessageList = JsonSerializer.Serialize(MessageList);
            

            File.WriteAllText(GetPath(), jsonMessageList);
        }

        private void Load()
        {
            if (!File.Exists(GetPath()))
            {
                var initialMessage = new Message()
                {
                    PropCreationDateTime = DateTime.Now,
                    PropUsername = "Server",
                    PropMessage = "Initial Message"
                };
                MessageList.Add(initialMessage);
                Save(); 
            }

            var jsonMessageList = File.ReadAllText(GetPath());

            MessageList = JsonSerializer.Deserialize<List<Message>>(jsonMessageList);
        }

        private string GetPath()
        {
            return $"{Environment.GetEnvironmentVariable("USERPROFILE")}/{MessageListFileName}";
        }
    }
}
