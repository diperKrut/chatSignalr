using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatSignalR.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR.Controllers
{
    [Authorize]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private Context con;
        private IMapper mapper;
        public ChatController(Context _con, IMapper _mapper)
        {
            con = _con;
            mapper = _mapper;
        }

        [HttpGet("/chat/initiliazation")]
        public IActionResult Init()
        {
            List<Massage> list = con.Massages
                 .AsNoTracking()
                 .Include(x=>x.User)
                 .ToList();
            var MassagesMapper = mapper.Map<List<Massage>, List<MassageMapper>>(list);

            var users = con.Users
                .AsNoTracking()
                .ToList();
            var usersMapper = mapper.Map<List<User>, List<UserMapper>>(users);

            return Ok(new
            {
                Login= HttpContext.User.Identity.Name,
                Users = usersMapper,
                Massages = MassagesMapper
            });

        }
    }
}