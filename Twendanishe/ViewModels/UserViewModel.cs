using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    public class UserViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal WalletBalance { get; set; }
        public int TypeId { get; set; }
        public string Gender { get; set; }
        public string DriverPrefence { get; set; }
        public int StateId { get; set; }
    }
}
