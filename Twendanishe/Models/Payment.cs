using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class Payment : Base
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Payer { get; set; }
        public decimal Amount { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
        public string Reference { get; set; }
    }
}
