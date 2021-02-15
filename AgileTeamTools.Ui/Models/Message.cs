using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Models
{
    public class Message
    {
        public Message(string username, string body, bool mine)
        {
            Username = username;
            Body = body;
            Mine = mine;
        }

        public string Username { get; set; }
        public string Body { get; set; }
        public bool Mine { get; set; }
    }
}
