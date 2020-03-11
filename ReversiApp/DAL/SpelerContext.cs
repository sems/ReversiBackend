using Microsoft.EntityFrameworkCore;
using ReversiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.DAL
{
    public class SpelerContext : DbContext
    {
        public SpelerContext(DbContextOptions<SpelerContext> options) : base(options)
        {
        }

        public DbSet<Speler> Speler { get; set; }
    }
}
