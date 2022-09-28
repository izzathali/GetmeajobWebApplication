using Getmeajob.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getmeajob.Data
{
    public class GetmeajobDbContext : DbContext
    {
        public GetmeajobDbContext(DbContextOptions<GetmeajobDbContext>options ) : base(options)
        {

        }
        public DbSet<CompanyM> Companies { get; set; }
        public DbSet<JobM> Jobs { get; set; }
        public DbSet<UserM> Users { get; set; }

    }
}
