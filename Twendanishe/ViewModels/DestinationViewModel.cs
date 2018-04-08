using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.ViewModels
{
    public class DestinationViewModel
    {
        public int UserId { get; set; }
        public int TypeId { get; set; }
        public int VehicleId { get; set; }
        public string SourceCoordinates { get; set; }
        public string SourceName { get; set; }
        public string DestinationCoordinates { get; set; }
        public string DestinationName { get; set; }
    }
}
