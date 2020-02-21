using AutoMapper;
using ChatSignalR.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.AutoMapper
{
    public class ProfileMapper:Profile
    {
        public ProfileMapper()
        {
            CreateMap<User, UserMapper>();
            CreateMap<Massage, MassageMapper>();
        }
    }
}
