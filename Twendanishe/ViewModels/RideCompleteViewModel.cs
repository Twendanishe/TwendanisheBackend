using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.ViewModels
{
    public class RideCompleteViewModel
    {
        public int RideId { get; set; }
        public int DriverId { get; set; }
        /// <summary>
        /// The fare paid for this item
        /// </summary>
        public decimal Amount { get; set; }
    }
}
