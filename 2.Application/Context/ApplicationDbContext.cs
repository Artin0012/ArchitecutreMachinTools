using _2.Application.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Application.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
      public  DbSet<Car> Cars { get; set; }
      public  DbSet<CarFeature> Features { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Car>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cars)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Features)
                .WithMany(u => u.Cars)
                .UsingEntity(c => c.ToTable("CarFeatureMappings"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
