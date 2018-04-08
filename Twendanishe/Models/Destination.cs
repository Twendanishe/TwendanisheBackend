using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class Destination : Base
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TypeId { get; set; }
        public virtual Type Type { get; set; }
        public string SourceCoordinates { get; set; }
        public string SourceName { get; set; }
        public string DestinationCoordinates { get; set; }
        public string DestinationName { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
        /// <summary>
        /// The available space for customers.
        /// When a driver accepts to pick a passenger, this is updated.
        /// </summary>
        public int AvailableCapacity { get; set; }

        public int VehicleId { get; set; }
        /// <summary>
        /// The active vehicle for this driver or passenger
        /// </summary>
        public virtual Vehicle Vehicle { get; set; }

        public List<Order> Orders { get; set; }
    }
}
