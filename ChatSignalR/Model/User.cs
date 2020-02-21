using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public List<Massage> Messages { get; set; }
        public List<ConnectrionUser> Connections { get; set; }

        public bool IsOnLine { get; set; } 
    }
}
