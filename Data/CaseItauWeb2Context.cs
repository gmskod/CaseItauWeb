#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CaseItauWeb2.Models;

namespace CaseItauWeb2.Data
{
    public class CaseItauWeb2Context : DbContext
    {
        public CaseItauWeb2Context (DbContextOptions<CaseItauWeb2Context> options)
            : base(options)
        {
        }

        public DbSet<CaseItauWeb2.Models.Fundo> Fundo { get; set; }
    }
}
