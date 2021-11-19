using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using demo2.Models;

namespace demo2.Data
{
    public class demo2Context : DbContext
    {
        public demo2Context (DbContextOptions<demo2Context> options)
            : base(options)
        {
        }

        public DbSet<demo2.Models.Account> Account { get; set; }

        public DbSet<demo2.Models.Profile> Profile { get; set; }
    }
}
