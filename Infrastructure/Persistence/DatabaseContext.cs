using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(
            DbContextOptions options) : base(options)
        {
        }

        public DbSet<IncomeTax> IncomeTax { get; set; }
     
    }
}
