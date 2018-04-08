using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class Vehicle : Base
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public int Capacity { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }

        public List<Destination> Destinations { get; set; }
    }
}
