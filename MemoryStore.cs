using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    internal class MemoryStore
    {
        // username
        public string Username { get; set; }

        // internal memory
        private Dictionary<string, string> memory = new Dictionary<string, string>();

        // store value
        public void Store(string key, string value)
        {
            key = key.ToLower();

            if (memory.ContainsKey(key))
            {
                memory[key] = value;
            }
            else
            {
                memory.Add(key, value);
            }
        }

        //get value
        public string Recall(string key)
        {
            key = key.ToLower();
            if (memory.ContainsKey(key))
            {
                return memory[key];
            }
           
                return null;
            

        }
        // message
        public string GetPersonalisedOpener()
        { 
            string message = "";

            if (!string.IsNullOrEmpty(Username))
            {
                message += $"Welcome back, {Username}!\n";
            }
            string topic = Recall("favourite topic");
            
            if (!string.IsNullOrEmpty(topic))
            {
                message += "Welcome to the Cybersecurity Bot!\n";
            }
            return message;
        } 
    }
}
