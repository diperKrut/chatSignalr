using AutoMapper;
using ChatSignalR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Hubs
{
    [Authorize]
    public class Chat: Hub
    {

        private Context con { get; set; }
        private IMapper mapper { get; set; }
      
        public Chat(Context _con, IMapper _mapper)
        {
            mapper = _mapper;
            con = _con;
        }
        public async Task Send(string massage)
        {
            var Login = Context.User.Identity.Name;
            
            User user = await con.Users
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Login == Login);
            Massage massageSave = new Massage()
            {
                User = user,
                Content = massage,
            };
            user.Messages.Add(massageSave);            
            con.Users.Update(user);
            await con.SaveChangesAsync();
            MassageMapper MassageMapper = mapper.Map<Massage, MassageMapper>(massageSave);
            await Clients.All.SendAsync("Send", MassageMapper);

        }


        public async Task DeleteMassage(int IdMassage)
        {
            var MassageInUser = AnyMassageInUser(Context.User.Identity.Name, IdMassage);
            if(MassageInUser)
            {
                await Clients.All.SendAsync("Delete", IdMassage);
                Massage mas = await con.Massages.FirstOrDefaultAsync(x => x.Id == IdMassage);
                con.Massages.Remove(mas);
                await con.SaveChangesAsync();
            }
            
        }

        public async Task ChangeMassage(int IdMassage, string newContent)
        {
            var MassageInUser = AnyMassageInUser(Context.User.Identity.Name, IdMassage);
            if (MassageInUser)
            {
                await Clients.All.SendAsync("Change", IdMassage, newContent);
                Massage mas = await con.Massages.FirstOrDefaultAsync(x => x.Id == IdMassage);
                mas.Content = newContent;
                con.Massages.Update(mas);
                await con.SaveChangesAsync();
            }
        }

        public bool AnyMassageInUser(string Login, int IdMassage)
        {
            if (con.Massages.Any(x => x.User.Login == Login && x.Id == IdMassage)) return true;
            return false;
            
        }

        public async override Task OnConnectedAsync()
        {
            User user = await con.Users
                .Include(x=>x.Connections)
                .FirstOrDefaultAsync(x => x.Login == Context.User.Identity.Name);
            user.Connections.Add(new ConnectrionUser() { ConnectionId = Context.ConnectionId });
            user.IsOnLine = true;
            UserMapper userMapper = mapper.Map<User, UserMapper>(user);
            await Clients.All.SendAsync("Online", userMapper);
            con.Users.Update(user);
            con.SaveChanges();
            

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectrionUser connectrion = await con.ConnectrionsUsers
                .FirstOrDefaultAsync(x => x.ConnectionId == Context.ConnectionId);
            con.ConnectrionsUsers.Remove(connectrion);
            await con.SaveChangesAsync();
            User user = await con.Users
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Login == Context.User.Identity.Name);
            if (user.Connections.Count == 0)
            {
                user.IsOnLine = false;
                UserMapper userMapper = mapper.Map<User, UserMapper>(user);
                await Clients.All.SendAsync("OffLine", userMapper);
                con.Users.Update(user);
                await con.SaveChangesAsync();
            }
            await base.OnDisconnectedAsync(exception);




        }

    }
}
