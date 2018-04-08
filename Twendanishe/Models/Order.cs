using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class Order : Base
    {
        public int DestinationId { get; set; }
        public virtual Destination Destination { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
    }
}
