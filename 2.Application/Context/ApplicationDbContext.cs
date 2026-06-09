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
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {   
      public  DbSet<Car> Cars { get; set; }
      public  DbSet<CarFeature> Features { get; set; }

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
