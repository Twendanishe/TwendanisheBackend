using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class User : Base
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal WalletBalance { get; set; }
        public int TypeId { get; set; }
        public virtual Type Type { get; set; }
        public string Gender { get; set; }
        public string DriverPrefence { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }

        public List<Destination> Destinations { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<WalletActivity> WalletActivities { get; set; }
        public List<UserLocation> UserLocations { get; set; }
    }
}
