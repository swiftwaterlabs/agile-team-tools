using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Models
{
    public class Message
    {
        public Message(string username, string body)
        {
            Username = username;
            Body = body;
        }

        public string Username { get; set; }
        public string Body { get; set; }
    }
}
