using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class State : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Destination> Destinations { get; set; }
        public List<Order> Orders { get; set; }
        public List<Payment> Payments { get; set; }
        public List<User> Users { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<WalletActivity> WalletActivities { get; set; }
        public List<UserLocation> UserLocations { get; set; }
    }
}
