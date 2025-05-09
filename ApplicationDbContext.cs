using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem
{
    namespace HotelSystem
    {
        public class ApplicationDbContext : DbContext
        {
            public DbSet<Room> Rooms { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Booking> Bookings { get; set; }

            public ApplicationDbContext() { }

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options) { }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
                    var connectionString = configuration.GetConnectionString("DefaultConnection");
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }
    }
}