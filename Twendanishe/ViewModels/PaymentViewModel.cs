using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.ViewModels
{
    public class PaymentViewModel
    {
        public int PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
