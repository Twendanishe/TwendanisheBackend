using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class WalletActivity : Base
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TypeId { get; set; }
        public virtual Type Type { get; set; }
        public int EntryId { get; set; }
        public virtual Entry Entry { get; set; }
        public int StateId { get; set; }
        public virtual State State { get; set; }
        public decimal AmountTransacted { get; set; }
        public decimal Balance { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
