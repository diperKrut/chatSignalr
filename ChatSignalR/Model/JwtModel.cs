using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Model
{
    public class JwtModel
    {
        public string accesToken { get; set; }

        public string refreshToken { get; set; }
    }
}
