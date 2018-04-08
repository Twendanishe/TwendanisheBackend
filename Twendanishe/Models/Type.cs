using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class Type : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Destination> Destinations { get; set; }
        public List<User> Users { get; set; }
        public List<WalletActivity> WalletActivities { get; set; }
    }
}
