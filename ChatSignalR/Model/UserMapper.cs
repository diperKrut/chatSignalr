using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Model
{
    public class UserMapper
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool IsOnLine { get; set; }
    }
}
