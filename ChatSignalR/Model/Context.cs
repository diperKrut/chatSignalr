using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Model
{
    public class Context: DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Massage> Massages { get; set; }

        public DbSet<ConnectrionUser> ConnectrionsUsers { get; set; }

        public Context(DbContextOptions<Context> options)
            :base(options)
        {

        }
    }
}
