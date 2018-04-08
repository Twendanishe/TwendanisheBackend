using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class UserLocation : Base
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int StateId { get; set; }
        /// <summary>
        /// The current state of this user. 
        /// Stationery, In Motion
        /// </summary>
        public State State { get; set; }
    }
}
