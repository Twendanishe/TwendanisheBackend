using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class UserLocationViewModel
    {
        public int UserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int StateId { get; set; }
    }
}
