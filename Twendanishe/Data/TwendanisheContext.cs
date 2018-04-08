using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twendanishe.Models;

namespace Twendanishe.Data
{
    public class TwendanisheContext : DbContext
    {
        public TwendanisheContext(DbContextOptions<TwendanisheContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<WalletActivity> WalletActivities { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
    }
}
